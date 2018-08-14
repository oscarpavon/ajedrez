using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ajedrez = AjedrezSupremo.Crear.Ajedrez;
namespace AjedrezSupremo.Juego {
	public class Rey : Pieza {
		public bool Hake;

		Casilla[] amenazadas;

		public override int Valor () {
			return 900;
		}
		public override Casilla[] Movimientos () {

			Juego.Casilla[] cass = Marcables ();
			ControlHake ();
			return cass;

		}

		public override void AgregarAmenaza () {
			this.marcables = this.Posiciones ();
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
						cas.AmenazadaPorOscuros.Remove (this);
						cas.AmenazadaPorOscuros = cas.AmenazadaPorOscuros.Distinct ().ToList ();
					}
				}
			}

		}

		Casilla[] PosicionesPosiblesSinAmenaza (Casilla[] cass) {
			//Convertir a lista
			List<Casilla> finales = cass.ToList ();

			if (this.ColorPieza == color.Claro) {
				foreach (Casilla cas in cass) {
					if (cas.AmenazadaPorOscuros.Count >= 1) {
						finales.Remove (cas);
					}
				}

			}
			if (this.ColorPieza == color.Oscuro) {
				foreach (Casilla cas in cass) {

					if (cas.AmenazadaPorClaros.Count >= 1) {
						finales.Remove (cas);
					}

				}
			}

			return finales.ToArray ();
		}

		Casilla[] Marcables () {
			Casilla[] pos = Posiciones ();
			pos = PosicionesPosiblesSinAmenaza (pos);
			return pos;
		}
		//Devuelve las posiciones posibles del rey validadas y si no estan ocupadas
		Casilla[] Posiciones () {
			string casillaAct = this.casillaActual;

			string[] pos = new string[8];
			pos[0] = Movimiento (casillaAct, 1, 1);
			pos[1] = Movimiento (casillaAct, 1, 0);
			pos[2] = Movimiento (casillaAct, 0, 1);
			pos[3] = Movimiento (casillaAct, -1, -1);
			pos[4] = Movimiento (casillaAct, -1, 0);
			pos[5] = Movimiento (casillaAct, 0, -1);
			pos[6] = Movimiento (casillaAct, 1, -1);
			pos[7] = Movimiento (casillaAct, -1, 1);

			pos = Ajedrez.ValidadCasillas (pos);
			Casilla[] cass = Ajedrez.BuscarCasillas (pos);
			cass = Ajedrez.CacillaOcupada (this, cass);

			return cass;

		}

		void ControlHake () {
			Hake = false;
			if (this.ColorPieza == color.Claro) {
				if (this.casilla.AmenazadaPorOscuros.Count >= 1) {
					this.Hake = true;
				}
			} else {
				if (this.casilla.AmenazadaPorClaros.Count >= 1) {
					this.Hake = true;
				}
			}
		}
	}
}