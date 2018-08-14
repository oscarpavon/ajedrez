using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ajedrez = AjedrezSupremo.Crear.Ajedrez;
namespace AjedrezSupremo.Juego {
	public class Peon : Pieza {

		public Casilla posInicial;
		Casilla[] amenazadas;
		bool CasComi = false;
		List<Casilla> casNoMarcables = new List<Casilla> ();

		public override int Valor () {
			return 10;
		}
		public override Juego.Casilla[] Movimientos () {

			Juego.Casilla[] cass = Posiciones ();
			return cass;

		}

		public override void AgregarAmenaza () {
			this.marcables = this.PosAmenazadas ();
			amenazadas = this.marcables;
			List<Casilla> casillas = new List<Casilla> ();
			for (int o = 0; o < marcables.Length; o++) {
				Casilla cas = marcables[o];
				casillas.Add (cas);
				this.CasillasAmenazadas = casillas;
				if (this.gameObject.activeInHierarchy) {
					if (this.ColorPieza == color.Claro) {
						cas.AmenazadaPorClaros.Add (this);
						cas.AmenazadaPorClaros = cas.AmenazadaPorClaros.Distinct ().ToList ();
					}
					if (this.ColorPieza == color.Oscuro) {
						cas.AmenazadaPorOscuros.Add (this);
						cas.AmenazadaPorOscuros = cas.AmenazadaPorOscuros.Distinct ().ToList ();
					}

				}

			}

			this.marcables = this.Movimientos();
		}
		public override void QuitarAmenaza () {
			if (amenazadas != null) {
				foreach (Casilla cas in amenazadas) {
					if (this.ColorPieza == color.Claro) {
						cas.AmenazadaPorClaros.Remove (this);
						cas.AmenazadaPorClaros = cas.AmenazadaPorClaros.Distinct ().ToList ();
					}
					if (this.ColorPieza == color.Oscuro) {
						//cas.AmenazadaPorOscuros.Remove (this);
						//cas.AmenazadaPorOscuros = cas.AmenazadaPorOscuros.Distinct ().ToList ();
					}
				}
			}

		}

		Juego.Casilla[] PosAmenazadas () {
			string[] CasillasPosibles = new string[1];
			string mov1 = null;
			string mov2 = null;
			if (this.ColorPieza == color.Claro) {
				mov1 = Movimiento (this.casillaActual, 1, 1);
				mov2 = Movimiento (this.casillaActual, 1, -1);

			} else //Oscuro
			{
				mov1 = Movimiento (this.casillaActual, -1, -1);
				mov2 = Movimiento (this.casillaActual, -1, 1);
			}

			List<string> posiciones = new List<string> ();
			posiciones.Add (mov1);
			posiciones.Add (mov2);
			string[] posicionesposibles = posiciones.ToArray ();
			posicionesposibles = Ajedrez.ValidadCasillas (posicionesposibles);
			Casilla[] casMenazadas = Ajedrez.BuscarCasillas (posicionesposibles);

			return casMenazadas;
		}
		Juego.Casilla[] Posiciones () {
			string[] CasillasPosibles = new string[1];
			string[] CasillaAmenazada1 = new string[1];
			string[] CasillaAmenazada2 = new string[1];
			string casillaAcual = this.casillaActual;

			//Movimientos Marcables Normales
			if (this.ColorPieza == color.Claro) {
				if (this.casilla == posInicial) {
					//Posibles movimientos
					//Dos pasos
					CasillasPosibles = Repetir (2, casillaAcual, "Frente");
					CasillasPosibles = Ajedrez.CasillasHasta (this, CasillasPosibles);

				} else {
					CasillasPosibles = Repetir (1, casillaAcual, "Frente");
					Casilla[] cas = Ajedrez.BuscarCasillas (CasillasPosibles);
					cas[0].marcar = false;
					casNoMarcables.Add (cas[0]);
				}

			} else {
				if (this.casilla == posInicial) {
					//Posibles movimientos
					//Dos pasos
					CasillasPosibles = Repetir (2, casillaAcual, "Atras");
					CasillasPosibles = Ajedrez.CasillasHasta (this, CasillasPosibles);
				} else {
					CasillasPosibles = Repetir (1, casillaAcual, "Atras");
					Casilla[] cas = Ajedrez.BuscarCasillas (CasillasPosibles);
					cas[0].marcar = false;
					casNoMarcables.Add (cas[0]);
				}

			}

			string[] comibles = PosicionesComibles ();
			List<string> posiciones = new List<string> ();
			if (CasComi && comibles != null) {
				posiciones.AddRange (CasillasPosibles);
				posiciones.AddRange (comibles);
			} else {

				posiciones.AddRange (CasillasPosibles);
			}

			//posiciones.AddRange (CasillaAmenazada1);
			//posiciones.AddRange (CasillaAmenazada2);

			string[] posicionesposibles = posiciones.ToArray ();
			posicionesposibles = Ajedrez.ValidadCasillas (posicionesposibles);
			Casilla[] cass = Ajedrez.BuscarCasillas (posicionesposibles);
			cass = Ajedrez.CacillaOcupada (this, cass);

			return cass;
		}

		string[] PosicionesComibles () {
			string mov1 = null;
			string mov2 = null;
			if (this.ColorPieza == color.Claro) {
				mov1 = Movimiento (this.casillaActual, 1, 1);
				mov2 = Movimiento (this.casillaActual, 1, -1);

			} else //Oscuro
			{
				mov1 = Movimiento (this.casillaActual, -1, -1);
				mov2 = Movimiento (this.casillaActual, -1, 1);
			}
			List<string> posComi = new List<string> ();
			Casilla cas1 = null;
			Casilla cas2 = null;
			if (Ajedrez.ValidarCasilla (mov1)) {
				cas1 = Ajedrez.BuscarCasilla (mov1);
				if (cas1.Ocupada) {
					if (cas1.pieza.ColorPieza != this.ColorPieza) {
						posComi.Add (cas1.nombre);
					}
				}
			}
			if (Ajedrez.ValidarCasilla (mov2)) {
				cas2 = Ajedrez.BuscarCasilla (mov2);
				if (cas2.Ocupada) {
					if (cas2.pieza.ColorPieza != this.ColorPieza) {
						posComi.Add (cas2.nombre);
					}
				}
			}

			//string[] val = null;
			if (posComi != null) {
				//val = Ajedrez.ValidadCasillas(posComi.ToArray());
				CasComi = true;
			}
			return posComi.ToArray ();
		}

		public override void CuandoMovio () {
			foreach (Casilla cas in casNoMarcables) {
				cas.marcar = true;
			}
			this.SeMovio = false;
		}
	}
}