using System.Collections;
using System.Collections.Generic;
using AjedrezSupremo;
using UnityEngine;
using UnityEngine.UI;

namespace AjedrezSupremo {
	public class MostarJugadas : MonoBehaviour {

		public Text textoJugadas; //lugar donde se escribiran las jugadas
		public static bool jugadaEscrita = false;
		public int numeroJugada = 0;
		void Start () {

		}

		
		void Update () {
			EscribirJugada ();
		}
		public void EscribirJugada () {
			if (jugadaEscrita == false) {
				if (Partida.Movimientos.Count >= 1) {

					string num = numeroJugada.ToString();
					string nombrepieza = Partida.Movimientos[numeroJugada].PiezaMovida.name;
					string casilla = Partida.Movimientos[numeroJugada].CasillaElegida.nombre;
					string mov = nombrepieza + " movio a " + casilla + "\n";
					textoJugadas.text += mov;
					jugadaEscrita = true;
					numeroJugada++;
				}
			}

		}
	}
}