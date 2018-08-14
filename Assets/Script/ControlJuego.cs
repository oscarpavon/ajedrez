using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AjedrezSupremo {
	/// <summary>
	/// Espera un determinado numero de cuadros para realizar algo
	/// </summary>
	public class ContadorCuadros {
		public int numCuadro = 0;
		bool Repetir = true;
		public bool EsperarCuadros (int CuadrosAesperar) {
			bool Retorno = false;
			if (numCuadro < CuadrosAesperar) {
				numCuadro++;
				Retorno = false;
				if (numCuadro >= CuadrosAesperar) {
					Retorno = true;
					numCuadro = 0;

				}
			}
			return Retorno;
		}
		public bool EsperarCuadrosYparar (int CuadrosAesperar) {

			if (EsperarCuadros (CuadrosAesperar)) {
				Repetir = false;
				return false;
			}

			return Repetir;
		}
	}

	public class ControlJuego : MonoBehaviour {

		public List<EstadoDeJuego> estados = new List<EstadoDeJuego> ();
		public int CantidadDeEstados = 0;
		public int estadoSelecionado = 0;
		public int movimintos;

		public int numMovArealizar = 0;

		public bool presentar = false;
		List<Movimiento> movs;
		EstadoDeJuego estadoOriginal;

		CalculoDeJugada calculo1;
		CalculoDeJugada calculo2;

		void Update () {

			if (Input.GetKeyDown (KeyCode.A)) {
				EstadoDeJuego estado = new EstadoDeJuego ();
				estadoOriginal = estado;
				//CantidadDeCasillasOcupadasEnEstado (estadoOriginal);

				//estado.ActualizarCasillasOcupadas();
				movs = Juego.IA.CalcularMovimientosPorColorEnEstado (Juego.color.Oscuro, estado);

				foreach(Movimiento mov in movs){
						//CantidadDeCasillasOcupadasEnEstado(mov.estadoFinal);
				}
				CalculoDeJugada calculo1 = new CalculoDeJugada(movs,estado);
				calculo1.Calcular();
			}

			if (Input.GetKeyDown (KeyCode.Z)) {

			}

			if (Input.GetKeyDown (KeyCode.B)) {

			}
			if (Input.GetKeyDown (KeyCode.G)) {
				Debug.Log (Partida.ListaDePiezas.Count.ToString ());
				EstadoDeJuego estado = new EstadoDeJuego (Partida.ListaDePiezas);

				estados.Add (estado);

			}
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				if (estadoSelecionado < estados.Count - 1) {
					estadoSelecionado++;
				}
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				if (estadoSelecionado > 0) {
					estadoSelecionado--;

				}
			}
			//Mostrar PIezas
			if (Input.GetKeyDown (KeyCode.M)) {
				estados[estadoSelecionado].MostrarPiezasEnSusPosiciones ();

			}

			CantidadDeEstados = estados.Count;
		}
		Movimiento CrearUnMovimiento (EstadoDeJuego estado) {
			Juego.Pieza pieza = Partida.ListaDePiezas[0];
			Juego.Casilla casilla = Partida.ListaDeCasillas[1];
			Movimiento movimientoUno = new Movimiento (pieza, casilla, estado);
			return movimientoUno;
		}
		void CrearMovimientosPrueba (EstadoDeJuego estado) {
			Juego.Pieza alfil = Partida.ListaDePiezas[0];

			Juego.Casilla h2 = AjedrezSupremo.Crear.Ajedrez.BuscarCasilla ("H2");
			Movimiento movUno = new Movimiento (alfil, h2, estado);
			Juego.Casilla cas = AjedrezSupremo.Crear.Ajedrez.BuscarCasilla ("C2");

			Movimiento mov2 = new Movimiento (alfil, cas, estado);

			CantidadDeCasillasOcupadasEnEstado (estadoOriginal);
			CantidadDeCasillasOcupadasEnEstado (mov2.estadoFinal);
			CantidadDeCasillasOcupadasEnEstado (movUno.estadoFinal);
		}
		void CantidadDeCasillasOcupadasEnEstado (EstadoDeJuego estado) {
			int cantidadCasillasOcupadas = 0;
			foreach (EstadoDeJuego.CasillaDeEstado cas in estado.CasillasDeEstado) {
				if (cas.EstaOcupada == true) {
					cantidadCasillasOcupadas++;
					Debug.Log (cas.Nombre);
				}
			}
			Debug.Log ("Cantidad de casillas ocupadas en estado " + cantidadCasillasOcupadas);
		}
	}
}