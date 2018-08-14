using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ajedrez = AjedrezSupremo.Crear.Ajedrez;

namespace AjedrezSupremo.Juego {
	public class Alfil : Pieza {

		public override int Valor () {
			return 30;
		}
		public override Casilla[] MovimientosDesdeEstado (EstadoDeJuego estado) {
			string casillaAct = null;
			foreach (EstadoDeJuego.PiezaDeEstado pieza in estado.ListaDePiezasDeEstado) {
				if (pieza.id == this.id) {
					casillaAct = pieza.Casilla;
				}
			}

			//Debug.Log (this.name + " " + "color "+ this.ColorPieza.ToString()+" "+"Casilla actual " + casillaAct);

			string[] frente = Repetir (8, casillaAct, "Diagonal Derecha");
			string[] atras = Repetir (8, casillaAct, "Diagonal Izquierda");
			string[] izq = Repetir (8, casillaAct, "Diagonal Izquierda Abajo");
			string[] der = Repetir (8, casillaAct, "Diagonal Derecha Abajo");

			frente = Ajedrez.ValidadCasillas (frente);
			atras = Ajedrez.ValidadCasillas (atras);
			izq = Ajedrez.ValidadCasillas (izq);
			der = Ajedrez.ValidadCasillas (der);

			bool calcAmenazas = false;
			frente = Ajedrez.CasillasHastaQueEsteOcupadaEnEstado (estado, this, frente, calcAmenazas);
			atras = Ajedrez.CasillasHastaQueEsteOcupadaEnEstado (estado, this, atras, calcAmenazas);
			izq = Ajedrez.CasillasHastaQueEsteOcupadaEnEstado (estado, this, izq, calcAmenazas);
			der = Ajedrez.CasillasHastaQueEsteOcupadaEnEstado (estado, this, der, calcAmenazas);

			List<string> pos = new List<string> ();
			pos.AddRange (frente);
			pos.AddRange (atras);
			pos.AddRange (izq);
			pos.AddRange (der);
			string[] posicionesposibles = pos.ToArray ();

			Casilla[] cass = Ajedrez.BuscarCasillas (posicionesposibles);

			return cass;
		}

		public override Casilla[] Movimientos () {

			Juego.Casilla[] cass = Marcables ();

			return cass;

		}

		public Casilla[] Marcables () {
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
	}

}