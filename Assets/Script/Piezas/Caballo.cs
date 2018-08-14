using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ajedrez = AjedrezSupremo.Crear.Ajedrez;
namespace AjedrezSupremo.Juego {
	public class Caballo : Pieza {
		public override int Valor()
		{
			return 30;
		}

        public override Casilla[] Movimientos()
        {
            
                Casilla[] cass = Posibles();

                return cass;
            
        }

        public Casilla[] Posibles () {

			string[] CasillasAmenazadas = new string[8];

			string casillaAcual = this.casillaActual;

			string[] frente = Repetir (2, casillaAcual, "Frente");
			CasillasAmenazadas[0] = Repetir (1, frente[1], "Derecha") [0];
			CasillasAmenazadas[1] = Repetir (1, frente[1], "Izquierda") [0];

			string[] atras = Repetir (2, casillaAcual, "Atras");
			CasillasAmenazadas[2] = Repetir (1, atras[1], "Izquierda") [0];
			CasillasAmenazadas[3] = Repetir (1, atras[1], "Derecha") [0];

			string[] izq = Repetir (2, casillaAcual, "Izquierda");
			CasillasAmenazadas[4] = Repetir (1, izq[1], "Frente") [0];
			CasillasAmenazadas[5] = Repetir (1, izq[1], "Atras") [0];

			string[] der = Repetir (2, casillaAcual, "Derecha");
			CasillasAmenazadas[6] = Repetir (1, der[1], "Frente") [0];
			CasillasAmenazadas[7] = Repetir (1, der[1], "Atras") [0];

			List<string> posiciones = new List<string> ();
			for (int i = 0; i < CasillasAmenazadas.Length; i++) {
				if (Ajedrez.ValidarCasilla (CasillasAmenazadas[i])) {
					posiciones.Add (CasillasAmenazadas[i]);

				}
			}
			string[] posicionesposibles = posiciones.ToArray ();
			Casilla[] cass = Ajedrez.BuscarCasillas (posicionesposibles);

			cass = Ajedrez.CacillaOcupada (this, cass);
			return cass;
		}
	}
}