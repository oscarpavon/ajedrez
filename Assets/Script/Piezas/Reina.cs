using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ajedrez = AjedrezSupremo.Crear.Ajedrez;
namespace AjedrezSupremo.Juego {
	public class Reina : Pieza {

		public override int Valor () {
			return 90;
		}
		public override Casilla[] Movimientos () {

			Casilla[] cass = Posibles ();

			return cass;

		}

		public Casilla[] Posibles () {
			List<Casilla> uno = new List<Casilla> ();
			uno.AddRange (HorizontalVertical ());
			uno.AddRange (Diagonal ());
			Casilla[] posiciones = uno.ToArray ();

			return posiciones;
		}
	}
}