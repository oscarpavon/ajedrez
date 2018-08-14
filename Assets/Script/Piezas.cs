using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AjedrezSupremo.Crear;
using UnityEditor;
using UnityEngine;

namespace AjedrezSupremo.Juego {

	public class Pieza : MonoBehaviour {

		public int id = 0;
		public bool SeMovio = false;
		public bool Seleccionada = false;
		public color ColorPieza;
		public bool Amenazada = false;
		public bool Levantada = false;
		public string casillaActual;

		public Casilla casilla;

		public List<Casilla> CasillasAmenazadas;
		public List<Casilla> CasillasAmenazadasAnt;

		public Casilla[] marcables;
		RaycastHit hit;

		Casilla casillaAnterior;

		public int ValorDePieza = 0;

		public List<Movimiento> MovimientosPosibles = new List<Movimiento> ();
		public Casilla CasillaAnterior;
		private void Start () {
			CasillasAmenazadas = new List<Casilla> ();
			CasillasAmenazadasAnt = new List<Casilla> ();
			PosPieza ();
		}

		public virtual int Valor () {
			return this.ValorDePieza;
		}
		public int ValorPorColor () {
			int valor;
			if (this.ColorPieza == color.Oscuro) {
				valor = -this.Valor ();
			} else {
				valor = this.Valor ();
			}
			return valor;
		}
		public virtual void AgregarAmenaza () {
			if (this.Amenazables () != null) { //si se cargo la funcion amenazables
				marcables = this.Amenazables ();
			}
			List<Casilla> casillas = new List<Casilla> ();
			foreach (Casilla cas in marcables) {
				casillas.Add (cas);
				this.CasillasAmenazadas = casillas;
				cas.AgregarAmenaza (this);
			}
			marcables = this.Movimientos ();
		}
		public virtual void QuitarAmenaza () {
			if (marcables != null) {
				Casilla[] marcables_ant = marcables;

				foreach (Casilla cas in marcables) {
					cas.QuitarAmenaza (this);
				}
			}
		}

		public virtual Casilla[] Amenazables () {

			return null;
		}
		/// <summary>
		/// Marcas o desmarca las casillas de la pieza
		/// Del arreglo Pieza.marcables
		/// </summary>
		public void MarcarDesamarcar () {
			if (Seleccionada) {

				Ajedrez.MarcarCasillas (this.marcables);
			} else {
				if (this.marcables != null) {
					Ajedrez.DesmarcarCasillas (this.marcables);
				}

			}

		}
		public void Update () {
			PosPieza ();
			if (SeMovio)
				this.CuandoMovio ();
			this.QuitarAmenaza ();

			this.marcables = this.Movimientos ();

			this.AgregarAmenaza ();

		}

		public virtual Casilla[] Movimientos () {
			return null;
		}
		public virtual Casilla[] MovimientosDesdeEstado (EstadoDeJuego estado) {
			return null;
		}

		public virtual void CuandoMovio () {
			this.SeMovio = false;
		}
		public void Mover (Vector3 pos) {
			this.transform.position = pos;
		}
		public void Mover (Casilla Casilla) {

			if (this.casilla != Casilla) {
				this.CasillaAnterior.Ocupada = false;
				this.CasillaAnterior.Ocupable = false;
				this.CasillaAnterior.pieza = null;
				if (Casilla.Ocupada) {
					//Debug.Log ("Se va desactivar " + Casilla.pieza.name);

					Casilla.pieza.gameObject.SetActive (false);

					//Debug.Log ("Se desactivo " + Casilla.pieza.name);
					Casilla.pieza = this;
					//Debug.Log ("Ahora la casilla tiene a " + Casilla.pieza.name);

				}
				casillaAnterior = this.casilla;
				this.transform.position = Casilla.posicion;
				this.SeMovio = true;
				casillaAnterior.Ocupada = false;
				casillaAnterior.Ocupable = false;
				casillaAnterior.pieza = null;
			}
			if (this.casilla == Casilla) {
				//this.transform.position = Casilla.posicion;
			}

		}

		public void MoverA (Casilla Casilla) {
			Casilla cas = Casilla;
			casillaAnterior = this.casilla;
			if (cas.Ocupada) {
				cas.pieza.gameObject.SetActive (false);

			}
			this.transform.position = cas.posicion;
			this.SeMovio = true;
			casillaAnterior.Ocupada = false;
			casillaAnterior.Ocupable = false;
			Movimiento nuevoMov = new Movimiento (this, Casilla);
			AjedrezSupremo.MostarJugadas.jugadaEscrita = false;
			Partida.Movimientos.Add (nuevoMov);
		}

		/// <summary>
		/// Coloca como ocupada la casilla en la que esta la pieza
		/// </summary>
		private void PosPieza () {
			Vector3 pos = new Vector3 (this.transform.position.x,
				this.transform.position.y + .2f, this.transform.position.z);
			if (Physics.Raycast (pos, -Vector3.up, out hit, 0.4f)) {
				if (hit.collider.gameObject.GetComponent<AjedrezSupremo.Juego.Casilla> () != null) {
					casilla = hit.collider.gameObject.GetComponent<AjedrezSupremo.Juego.Casilla> ();
					casillaActual = casilla.nombre;
					casilla.Ocupada = true;
					casilla.pieza = this;

				}

			}
			if (this.casilla.Ocupable) {
				this.Amenazada = true;
			} else {
				Amenazada = false;
			}
		}

		/// <summary>
		/// Crear un movimiento para caculos de posiciones
		/// </summary>
		/// <returns></returns>

		public Movimiento CrearJugadaLogica (Casilla casAmover) {
			Movimiento movLogico = new Movimiento (this, casAmover);
			return movLogico;
		}
		public List<Movimiento> JugadasPosiblesDesdeEstado (EstadoDeJuego estado) {

			Casilla[] CasillasDondeSePuedeMover = this.MovimientosDesdeEstado (estado);
			List<Movimiento> jugadas = new List<Movimiento> ();
			foreach (Casilla CasillaDondeMover in CasillasDondeSePuedeMover) {
				Movimiento MovimietoEnEstado = CrearJugadaEnEstado (CasillaDondeMover, estado);
				jugadas.Add (MovimietoEnEstado);
			}
			this.MovimientosPosibles = jugadas;
			return jugadas;
		}
		public Movimiento CrearJugadaEnEstado (Casilla casAmover, EstadoDeJuego estado) {
			Movimiento MovimientoEnElEstado = new Movimiento (this, casAmover, estado);
			//Movimiento MovimientoEnElEstado = AjedrezSupremo.Movimiento.CrearMovimientoEnEstado();
			return MovimientoEnElEstado;
		}
		public List<Movimiento> JugadasPosibles () {

			Casilla[] casillasPosibles = this.marcables;
			//Debug.Log("jugadas posibles de movimientos  " + this.name +this.Movimientos().Length.ToString());
			List<Movimiento> jugadas = new List<Movimiento> ();
			foreach (Casilla cas in casillasPosibles) {
				jugadas.Add (CrearJugadaLogica (cas));
			}
			this.MovimientosPosibles = jugadas;
			//Debug.Log("jugadas posibles " + this.name +jugadas.Count.ToString());
			return jugadas;
		}
		public Casilla[] HorizontalVertical () {
			string casillaAct = this.casillaActual;

			string[] frente = Repetir (8, casillaAct, "Frente");
			string[] atras = Repetir (8, casillaAct, "Atras");
			string[] izq = Repetir (8, casillaAct, "Izquierda");
			string[] der = Repetir (8, casillaAct, "Derecha");

			frente = Ajedrez.ValidadCasillas (frente);
			atras = Ajedrez.ValidadCasillas (atras);
			izq = Ajedrez.ValidadCasillas (izq);
			der = Ajedrez.ValidadCasillas (der);

			frente = Ajedrez.CasillasHasta (this, frente);
			atras = Ajedrez.CasillasHasta (this, atras);
			izq = Ajedrez.CasillasHasta (this, izq);
			der = Ajedrez.CasillasHasta (this, der);

			List<string> pos = new List<string> ();
			pos.AddRange (frente);
			pos.AddRange (atras);
			pos.AddRange (izq);
			pos.AddRange (der);
			string[] posicionesposibles = pos.ToArray ();

			Casilla[] cass = Ajedrez.BuscarCasillas (posicionesposibles);
			return cass;
		}

		public Casilla[] Diagonal () {
			string casillaAct = this.casillaActual;

			string[] frente = Repetir (8, casillaAct, "Diagonal Derecha");
			string[] atras = Repetir (8, casillaAct, "Diagonal Izquierda");
			string[] izq = Repetir (8, casillaAct, "Diagonal Izquierda Abajo");
			string[] der = Repetir (8, casillaAct, "Diagonal Derecha Abajo");

			frente = Ajedrez.ValidadCasillas (frente);
			atras = Ajedrez.ValidadCasillas (atras);
			izq = Ajedrez.ValidadCasillas (izq);
			der = Ajedrez.ValidadCasillas (der);

			frente = Ajedrez.CasillasHasta (this, frente);
			atras = Ajedrez.CasillasHasta (this, atras);
			izq = Ajedrez.CasillasHasta (this, izq);
			der = Ajedrez.CasillasHasta (this, der);

			List<string> pos = new List<string> ();
			pos.AddRange (frente);
			pos.AddRange (atras);
			pos.AddRange (izq);
			pos.AddRange (der);
			string[] posicionesposibles = pos.ToArray ();
			Casilla[] cass = Ajedrez.BuscarCasillas (posicionesposibles);
			return cass;

		}

		public string[] Repetir (int cantidad, string casillaAct, string dir) {
			string casillaAumentada = MoverEnDireccion (casillaAct, dir);
			string[] posiciones = new string[cantidad];

			for (int i = 0; i < cantidad; i++) {

				posiciones[i] = casillaAumentada;

				casillaAumentada = MoverEnDireccion (casillaAumentada, dir);

			}

			return posiciones;
		}

		string MoverEnDireccion (string casilla, string dir) {
			string pos = "";
			if (dir == "Frente") {
				pos = this.Movimiento (casilla, 1, 0);
			}
			if (dir == "Atras") {
				pos = this.Movimiento (casilla, -1, 0);
			}
			if (dir == "Derecha") {
				pos = this.Movimiento (casilla, 0, 1);
			}
			if (dir == "Izquierda") {
				pos = this.Movimiento (casilla, 0, -1);
			}
			if (dir == "Diagonal Derecha") {
				pos = this.Movimiento (casilla, 1, 1);
			}
			if (dir == "Diagonal Izquierda") {
				pos = this.Movimiento (casilla, 1, -1);
			}
			if (dir == "Diagonal Izquierda Abajo") {
				pos = this.Movimiento (casilla, -1, -1);
			}
			if (dir == "Diagonal Derecha Abajo") {
				pos = this.Movimiento (casilla, -1, 1);
			}
			return pos;
		}

		public string Movimiento (string casillaActual, int dir, int dir2) {
			string poscionFinal = null;
			if (casillaActual != null) {
				char[] Chars = casillaActual.ToCharArray ();
				if (dir == 1)
					Chars[1]++;
				if (dir == -1)
					Chars[1]--;

				if (dir2 == 1)
					Chars[0]++;
				if (dir2 == -1)
					Chars[0]--;

				string castrn = Chars[0].ToString () + Chars[1].ToString ();
				poscionFinal = castrn.ToUpper ();

			}
			return poscionFinal;
		}

		public void Quitar () {

			this.gameObject.SetActive (false);
		}

	}
	public enum color { Oscuro, Claro };
}