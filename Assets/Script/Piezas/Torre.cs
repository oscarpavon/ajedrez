using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ajedrez = AjedrezSupremo.Crear.Ajedrez;

namespace AjedrezSupremo.Juego {
	public class Torre : Pieza {

		public override int Valor () {
			return 50;
		}
		public override Casilla[] Movimientos () {
			Juego.Casilla[] cass = Casillas (false);

			return cass;
		}
		public override Casilla[] MovimientosDesdeEstado (EstadoDeJuego estado) {
			string casillaAct = null;
			foreach(EstadoDeJuego.PiezaDeEstado pieza in estado.ListaDePiezasDeEstado) {
				if(pieza.id == this.id){
					casillaAct = pieza.Casilla;
				}
			}
			//Debug.Log("Casilla desde donde se va calcular " + casillaAct);
			string[] frente = Repetir (8, casillaAct, "Frente");
			string[] atras = Repetir (8, casillaAct, "Atras");
			string[] izq = Repetir (8, casillaAct, "Izquierda");
			string[] der = Repetir (8, casillaAct, "Derecha");

			frente = Ajedrez.ValidadCasillas (frente);
			atras = Ajedrez.ValidadCasillas (atras);
			izq = Ajedrez.ValidadCasillas (izq);
			der = Ajedrez.ValidadCasillas (der);

			
			
			bool calcAmenazas = false;
			frente = Ajedrez.CasillasHastaQueEsteOcupadaEnEstado (estado,this, frente, calcAmenazas);
			atras = Ajedrez.CasillasHastaQueEsteOcupadaEnEstado (estado, this, atras, calcAmenazas);
			izq = Ajedrez.CasillasHastaQueEsteOcupadaEnEstado (estado, this, izq, calcAmenazas);
			der = Ajedrez.CasillasHastaQueEsteOcupadaEnEstado (estado, this, der, calcAmenazas);
			
			//Debug.Log("Cantidad frente " + frente.Length);


			List<string> pos = new List<string> ();
			pos.AddRange (frente);
			pos.AddRange (atras);
			pos.AddRange (izq);
			pos.AddRange (der);
			string[] posicionesposibles = pos.ToArray ();

			Casilla[] cass = Ajedrez.BuscarCasillas (posicionesposibles);

			return cass;
		}
		public override Casilla[] Amenazables () {

			Casilla[] cass = Casillas (true);
			return cass;
		}
		/// <summary>
		/// Calculas las cacillas a marcar(posibles movimientos)
		/// </summary>
		/// <param name="calcAmenazas">Si es true, calcula las cacillas a amenzar</param>
		/// <returns></returns>
		Casilla[] Casillas (bool calcAmenazas) {
			string casillaAct = this.casillaActual;

			string[] frente = Repetir (8, casillaAct, "Frente");
			string[] atras = Repetir (8, casillaAct, "Atras");
			string[] izq = Repetir (8, casillaAct, "Izquierda");
			string[] der = Repetir (8, casillaAct, "Derecha");

			frente = Ajedrez.ValidadCasillas (frente);
			atras = Ajedrez.ValidadCasillas (atras);
			izq = Ajedrez.ValidadCasillas (izq);
			der = Ajedrez.ValidadCasillas (der);

			frente = Ajedrez.CasillasHasta (this, frente, calcAmenazas);
			atras = Ajedrez.CasillasHasta (this, atras, calcAmenazas);
			izq = Ajedrez.CasillasHasta (this, izq, calcAmenazas);
			der = Ajedrez.CasillasHasta (this, der, calcAmenazas);

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