using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AjedrezSupremo {

	/// <summary>
	/// Representa el estado del juego, posciones de piezas en el tablero
	/// </summary>
	public class EstadoDeJuego {
		/// <summary>
		/// Infomacion de color, casilla en un estado
		/// </summary>
		public class PiezaDeEstado {

			/// <summary>
			/// Crea un pieza con la casilla especificada
			/// </summary>
			/// <param name="pieza"></param>
			/// <param name="Casilla"></param>
			public PiezaDeEstado (Juego.Pieza pieza, Juego.Casilla Casilla) {
				this.id = pieza.id;
				this.Nombre = pieza.name;
				this.Casilla = Casilla.nombre;
				this.Color = pieza.ColorPieza.ToString ();
			}
			public PiezaDeEstado (Juego.Pieza pieza) {
				this.id = pieza.id;
				this.Nombre = pieza.name;
				this.Casilla = pieza.casilla.nombre;
				this.Color = pieza.ColorPieza.ToString ();
			}
			public int id;
			public string Nombre;
			public string Casilla;
			public string Color;
			public EstadoDeJuego EstadoDelJuego;
			public void Crear () {
				//AjedrezSupremo.Crear.Ajedrez.CrearPieza(this);
				Juego.Casilla cas = AjedrezSupremo.Crear.Ajedrez.BuscarCasilla (this.Casilla);
				//Debug.Log ("Se va mover " + Nombre + " en la casilla" + cas.nombre);

				Partida.ListaDePiezas[this.id].Mover (cas);
				Partida.ListaDePiezas[this.id].gameObject.SetActive (true);

			}
		}

		public class CasillaDeEstado {
			public int idPorOrdenAgregado;
			public string Nombre;

			public Juego.Casilla CasillaDeJuego;

			public bool EstaOcupada;

			public Juego.Pieza pieza;

			public CasillaDeEstado (Juego.Casilla cas) {
				this.CasillaDeJuego = cas;
				this.Nombre = cas.nombre;
				if (cas.Ocupada) {
					this.EstaOcupada = true;
					this.pieza = cas.pieza;
				}

			}

		}
		public List<PiezaDeEstado> ListaDePiezasDeEstado = new List<PiezaDeEstado> ();

		public List<int> IdsActivos = new List<int> ();

		public List<int> IdsActivosPiezasClaras = new List<int> ();
		public List<int> IdsActivosPiezasOscuras = new List<int> ();

		public List<CasillaDeEstado> CasillasDeEstado = new List<CasillaDeEstado> ();

		public int ValorDeEstado = 0;

		public bool Turno = false; //falso es negras, verdadero es blacas

		public int CantidadCasillasOcupadas;
		public EstadoDeJuego.CasillaDeEstado[] CasillaOcupadas;
		public EstadoDeJuego (EstadoDeJuego estado) {
			CrearListaDePiezasDesdeEstado (estado);
			ClasificarPiezasPorColor ();
			CrearCasillasDesdeEstado (estado);
		}

		public EstadoDeJuego () {
			CrearListaDePiezasDeClasePartida ();
			ClasificarPiezasPorColor ();
			CrearCasillasDesdePartida ();
		}

		public EstadoDeJuego (List<Juego.Pieza> listaPiezas) {

			ListaPiezas (listaPiezas);
		}
		void ListaPiezas (List<Juego.Pieza> listaPiezas) {
			//Debug.Log(listaPiezas.Count.ToString());
			foreach (Juego.Pieza pieza in listaPiezas) {
				PiezaDeEstado pie = new PiezaDeEstado (pieza);
				this.ListaDePiezasDeEstado.Add (pie);

			}
			//string s = ListaDePiezas.Count.ToString();
			//Debug.Log(s);
		}

		public void CrearListaDePiezasDesdeEstado (EstadoDeJuego estado) {

			CrearListaDePiezasDeClasePartida ();
			List<PiezaDeEstado> nuevaListaDos = new List<PiezaDeEstado> ();
			foreach (PiezaDeEstado piezaOriginal in ListaDePiezasDeEstado) {
				foreach (int id in estado.IdsActivos) {
					if (piezaOriginal.id == id) {
						nuevaListaDos.Add (piezaOriginal);
					}
				}
			}
			this.ListaDePiezasDeEstado = nuevaListaDos;
			//this.IdsActivos = estado.IdsActivos;
		}
		public void CrearListaDePiezasDeClasePartida () {
			foreach (Juego.Pieza pieza in Partida.ListaDePiezas) {
				this.IdsActivos.Add (pieza.id);
				PiezaDeEstado piezaEstado = new PiezaDeEstado (pieza);
				this.ListaDePiezasDeEstado.Add (piezaEstado);
			}
		}
		void CrearCasillasDesdeEstado (EstadoDeJuego estado) {
			int idOrden = 0;
			List<CasillaDeEstado> nuevaListaCasillasDeEstado = new List<CasillaDeEstado> ();
			foreach (CasillaDeEstado cas in estado.CasillasDeEstado) {
				cas.idPorOrdenAgregado = idOrden;
				cas.EstaOcupada = false;
				nuevaListaCasillasDeEstado.Add (cas);
				idOrden++;
				//cas.EstaOcupada = false;
			}
			this.CasillasDeEstado = nuevaListaCasillasDeEstado;
		}
		void CrearCasillasDesdePartida () {
			int idOrden = 0;
			foreach (Juego.Casilla cas in Partida.ListaDeCasillas) {
				CasillaDeEstado casEstado = new CasillaDeEstado (cas);
				casEstado.idPorOrdenAgregado = idOrden;
				idOrden++;
				this.CasillasDeEstado.Add (casEstado);
				this.CasillasDeEstado = this.CasillasDeEstado.Distinct ().ToList ();
			}
		}
		public void MostrarPiezasEnSusPosiciones () {
			List<PiezaDeEstado> aPresentar = new List<PiezaDeEstado> ();

			//Debug.Log ("se llamo a presentar con " + aPresentar.Count.ToString () + " piezas");

			foreach (int numPieza in IdsActivos) {
				foreach (PiezaDeEstado pie in ListaDePiezasDeEstado) {
					if (pie.id == numPieza) {
						aPresentar.Add (pie);
					}
				}
			}
			foreach (PiezaDeEstado pieza in aPresentar) {
				pieza.Crear ();

			}
		}

		public void ClasificarPiezasPorColor () {
			foreach (int idPieza in this.IdsActivos) {
				if (Partida.ListaDePiezas[idPieza].ColorPieza == Juego.color.Oscuro) {
					IdsActivosPiezasOscuras.Add (idPieza);
					IdsActivosPiezasOscuras = IdsActivosPiezasOscuras.Distinct ().ToList ();
				}
				if (Partida.ListaDePiezas[idPieza].ColorPieza == Juego.color.Claro) {
					IdsActivosPiezasClaras.Add (idPieza);
					IdsActivosPiezasClaras = IdsActivosPiezasClaras.Distinct ().ToList ();
				}
			}
		}

		public void QuitarPieza (PiezaDeEstado pieza) {
			this.IdsActivos.Remove (pieza.id);
			QuitarDePiezasActivasPorSuColor (pieza);

		}

		public void QuitarDePiezasActivasPorSuColor (PiezaDeEstado pieza) {
			if (Partida.ListaDePiezas[pieza.id].ColorPieza == Juego.color.Oscuro) {
				IdsActivosPiezasOscuras.Remove (pieza.id);
			} else
				IdsActivosPiezasClaras.Remove (pieza.id);
		}
		public CasillaDeEstado BuscarCasilla (string Casilla) {
			EstadoDeJuego.CasillaDeEstado CasillaEnContrada = null;
			foreach (CasillaDeEstado cas in CasillasDeEstado) {
				if (cas.Nombre == Casilla) {
					CasillaEnContrada = cas;
					break;
				}
			}
			return CasillaEnContrada;
		}

		public void ActualizarCasillasOcupadas () {
			int cantidadCasillasOcupadas = 0;
			foreach (EstadoDeJuego.CasillaDeEstado cas in this.CasillasDeEstado) {
				if (cas.EstaOcupada == true) {
					cantidadCasillasOcupadas++;
				}
			}
			EstadoDeJuego.CasillaDeEstado[] casillas = new EstadoDeJuego.CasillaDeEstado[cantidadCasillasOcupadas];
			for (int i = 0; i < casillas.Length; i++) {
				foreach (EstadoDeJuego.CasillaDeEstado cas in this.CasillasDeEstado) {
					if (cas.EstaOcupada == true) {
						casillas[i] = cas;
					}
				}
			}
			this.CasillaOcupadas = casillas;
			this.CantidadCasillasOcupadas = cantidadCasillasOcupadas;

		}

		public PiezaDeEstado BuscarPieza (Juego.Pieza pieza) {
			PiezaDeEstado PiezaEncontrada = null;
			foreach (PiezaDeEstado pie in this.ListaDePiezasDeEstado) {
				if (pie.id == pieza.id) {
					PiezaEncontrada = pie;
				}
			}
			return PiezaEncontrada;
		}
	}

}