using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AjedrezSupremo {
	public class CalculoDeJugada {
		List<Movimiento> ListaDeMovimientosParaCalcular = new List<Movimiento> ();
		EstadoDeJuego estadoOriginal;

		ContadorCuadros cuadrosParaRetomarPos = new ContadorCuadros ();

		ContadorCuadros cuadrosParaMostrarJugada = new ContadorCuadros ();

		bool IAmovio = false;

		bool SePuedeMostrar = false;

		public int NumeroDeMovimiento = 0;

		public bool terminado;

		public CalculoDeJugada CalculoJugadorContrario;

		public bool SePresentoElEstadoOriginal;
		bool posOrinal = false;

		int MejorValorDeMovimiento;

		Movimiento MejorMovimiento;

		EstadoDeJuego EstadoParaCalcular;

		public int valor = 0;

		public int Pro = 0;
		public bool esMax = true;
		public bool parar = false;
		public CalculoDeJugada (List<Movimiento> MovimientosCalcular, EstadoDeJuego estadoOriginal) {
			this.estadoOriginal = estadoOriginal;
			this.ListaDeMovimientosParaCalcular = MovimientosCalcular;
			//Debug.Log(this.estadoOriginal.ListaDePiezas[0].Casilla);

		}
		public void Calcular () {
			MiniMax (1, estadoOriginal, true);
		}
		public Movimiento MiniMax (int Profundidad, EstadoDeJuego estado, bool esMaximo) {
			if (Profundidad == 0) {

				if (estado.Turno == true) { //blancas
					Movimiento mov = MovimientoClaras (estado);
					Debug.Log ("Fin minimax");

					return mov;
				} else { //juegam megram

				}
				
			}

			if (esMaximo) {
				Movimiento mejorMov = new Movimiento (Juego.color.Oscuro);
				int mejor = -999;
				foreach (Movimiento mov in ListaDeMovimientosParaCalcular) {

					//DarValorDeEstado (mov);
					mov.estadoFinal.Turno = true; //juegan blancas
					//Movimiento me = MiniMax (Profundidad - 1, mov.estadoFinal, !esMaximo);
					Debug.Log("Se va calcular movimiento desde estado blancas");
					//Debug.Log("Cantidad de piezas en el estado a calcular " + mov.estadoFinal.IdsActivosPiezasClaras.Count);
					//Debug.Log("Cantidad casillas de estado " + mov.estadoFinal.CasillasDeEstado.Count);
					
					//mov.estadoFinal.LogCantidadCasillasOcupadas();
					Movimiento mejorMovBlanca = MovimientoClaras(mov.estadoFinal);
					Debug.Log ("Mejor valor blancas" + mejorMovBlanca.Valor);
					//Debug.Log("Valor mov negra " + mov.estadoFinal.ValorDeEstado);
					//mejorMov = Max (mejorMov, me);
					
					if(mov.Valor > mejor ){
						mejorMov = mov;
						mejor =mov.Valor;
					}
					
				}
				Debug.Log("Mejor mov negras "+mejorMov.Valor);
				Debug.Log ("se creo un arbol para negras");
				return mejorMov;

			} else {
				List<Movimiento> movimientos = Juego.IA.CalcularMovimientosPorColorEnEstado (Juego.color.Claro, estado);

				Movimiento mejorMov = new Movimiento (Juego.color.Claro);
				foreach (Movimiento mov in movimientos) {
					mov.estadoFinal.Turno = false; //juegan negras
					Movimiento moviVal = MiniMax (Profundidad - 1, mov.estadoFinal, !esMaximo);
					mejorMov = Min (mejorMov, moviVal);
				}
				Debug.Log ("Se creo un arbol par ablancas");

				return mejorMov;
			}

		}
		Movimiento MovimientoClaras (EstadoDeJuego estado) {
			List<Movimiento> movimientos = Juego.IA.CalcularMovimientosPorColorEnEstado (Juego.color.Claro, estado);
			Movimiento mejorMov = new Movimiento (Juego.color.Claro);
			int mejorValor = 999;
			Debug.Log("Cantidad de movimientos a calcular " + movimientos.Count);
			foreach (Movimiento mov in movimientos) {
				//DarValorDeEstado (mov);
				//if(mov.Valor != 1)
					Debug.Log("Valor de movimiento blancas " + mov.Valor);

				//mejorMov = Min (mejorMov, mov);
				if(mov.Valor<mejorValor){
					mejorMov = mov;
					mejorValor = mov.Valor;
				}
				//Debug.Log ("Valor de estado " + mejorMov.estadoFinal.ValorDeEstado);
				
			}
			//Debug.Log("Mejor valor blanca " + mejorMov.Valor);
			return mejorMov;
		}

		Movimiento Min (Movimiento a, Movimiento b) {
			if (a.estadoFinal.ValorDeEstado < b.estadoFinal.ValorDeEstado) {
				return a;
			} else
				return b;
		}
		Movimiento Max (Movimiento a, Movimiento b) {
			if (a.estadoFinal.ValorDeEstado > b.estadoFinal.ValorDeEstado) {
				return a;
			} else
				return b;
		}

		public void CalcularMiniMax () {
			if (!parar)
				MiniMax (3, estadoOriginal, true);
		}
		void DarValorDeEstado (Movimiento mov) {
			mov.estadoFinal.ValorDeEstado =
				mov.Valor;

			if (mov.estadoFinal.Turno == true) {

			} else {

			}

		}
		void ComprobarMejorMovimientoaYactualizar (Movimiento mov, Juego.color col) {

			int ValorDeJugada = mov.Valor;
			if (col == Juego.color.Oscuro) {
				int MejorValor = -999;
				if (ValorDeJugada > MejorValor) {
					MejorValorDeMovimiento = ValorDeJugada;
					this.MejorMovimiento = mov;

				}

			} else {
				int MejorValor = 999;
				if (ValorDeJugada < MejorValor) {
					MejorValorDeMovimiento = ValorDeJugada;
					this.MejorMovimiento = mov;

				}

			}
			if (MejorMovimiento != null) {
				this.valor = MejorMovimiento.Valor;
			}

		}
		public void CalcularCadaTantosCuadros (int cuadrosEsperar) {

			SePuedeMostrar = cuadrosParaMostrarJugada.EsperarCuadros (cuadrosEsperar);

			if (SePuedeMostrar && ListaDeMovimientosParaCalcular != null) {
				CrearArbol ();
			}

			if (CalculoJugadorContrario != null) {
				//Debug.Log(jugadaContrario.estadoOriginal.ListaDePiezas[0].Casilla);

				CalculoJugadorContrario.MostrarJugadasCadaTantosCuadrosYrevertir (5);

				//Debug.Log(jugadaContrario.numMov.ToString());
			}
			if (posOrinal) {
				//Debug.Log (this.estadoOriginal.ListaDePiezasDeEstado[0].Casilla);
				MostrarPosicionOriginalDelEstadoEnTantoCuadros ();
				//Debug.Log ("se movio a la original");

			}

		}

		void DarValorDeEstado (int NumeroDeMovimiento) {
			ListaDeMovimientosParaCalcular[NumeroDeMovimiento].estadoFinal.ValorDeEstado =
				ListaDeMovimientosParaCalcular[NumeroDeMovimiento].Valor;
			if (CalculoJugadorContrario != null) {
				if (CalculoJugadorContrario.MejorValorDeMovimiento < 0) {
					ListaDeMovimientosParaCalcular[NumeroDeMovimiento].estadoFinal.ValorDeEstado =
						CalculoJugadorContrario.MejorValorDeMovimiento;

				}
			}

		}

		public void CrearMini (int cuadrosEsperar) {

			SePuedeMostrar = cuadrosParaMostrarJugada.EsperarCuadros (cuadrosEsperar);

			if (SePuedeMostrar && ListaDeMovimientosParaCalcular != null) {
				CalcularMiniMax ();
			}

			if (CalculoJugadorContrario != null) {
				//Debug.Log(jugadaContrario.estadoOriginal.ListaDePiezas[0].Casilla);

				CalculoJugadorContrario.MostrarJugadasCadaTantosCuadrosYrevertir (5);

				//Debug.Log(jugadaContrario.numMov.ToString());
			}
			if (posOrinal) {
				//Debug.Log (this.estadoOriginal.ListaDePiezasDeEstado[0].Casilla);
				MostrarPosicionOriginalDelEstadoEnTantoCuadros ();
				//Debug.Log ("se movio a la original");

			}
		}
		void CrearArbol () {
			if (CalculoJugadorContrario != null && CalculoJugadorContrario.terminado == true) {

				NumeroDeMovimiento++;
				IAmovio = true;
				SePuedeMostrar = false;
				posOrinal = true;
				CalculoJugadorContrario = null;

			}

			if (NumeroDeMovimiento <= ListaDeMovimientosParaCalcular.Count - 1) {
				//DarValorDeEstado (NumeroDeMovimiento);
				ListaDeMovimientosParaCalcular[NumeroDeMovimiento].estadoFinal.MostrarPiezasEnSusPosiciones ();
				DarValorDeEstado (NumeroDeMovimiento);
				if (CalculoJugadorContrario == null) {
					//Debug.Log("calculo");
					EstadoParaCalcular = ListaDeMovimientosParaCalcular[NumeroDeMovimiento].estadoFinal;
					CrearJugadaDesdeEstado (EstadoParaCalcular);

				}

				//Debug.Log(estado.ListaDePiezas[0].Casilla);
				//Debug.Log("alfinl = " + Partida.ListaDePiezas[0].casilla.nombre);

				//string s= MovimientosDelContrario[0].estadoFinal.ListaDePiezas[0].Casilla;
				//Debug.Log(s);

				//sDebug.Log (estado.ListaDePiezas[0].Nombre + " se guardo " + estado.ListaDePiezas[0].Casilla);

			}
		}
		void ComprobarMejorMovimientoaYactualizar (Movimiento movimiento) {

			int ValorDeJugada = movimiento.Valor;

			if (ValorDeJugada > MejorValorDeMovimiento) {
				MejorValorDeMovimiento = ValorDeJugada;
				this.MejorMovimiento = movimiento;

			}

			if (MejorMovimiento != null) {
				this.valor = MejorMovimiento.Valor;
			}

		}
		void CrearJugadaDesdeEstado (EstadoDeJuego estado) {
			List<Movimiento> MovimientosDelContrario =
				Juego.IA.CalcularMovimientosPorColorEnEstado (Juego.color.Claro, estado);

			CalculoJugadorContrario = new CalculoDeJugada (MovimientosDelContrario, estado);
			//Debug.Log (estado.ListaDePiezas[0].Nombre + " se guardo " + estado.ListaDePiezas[0].Casilla);
			CalculoJugadorContrario.NumeroDeMovimiento = 0;
			CalculoJugadorContrario.terminado = false;

		}
		public void MostrarJugadasYrevertir () {
			if (SePuedeMostrar && ListaDeMovimientosParaCalcular != null) {
				MostrarYaumentarMovimiento ();
			}
			MostrarPosicionOriginalDelEstado ();
		}

		public void MostrarJugadasCadaTantosCuadrosYrevertir (int cuadrosEsperar) {
			SePuedeMostrar = cuadrosParaMostrarJugada.EsperarCuadros (cuadrosEsperar);
			if (SePuedeMostrar && ListaDeMovimientosParaCalcular != null) {
				MostrarYaumentarMovimiento ();
			}
			MostrarPosicionOriginalDelEstadoEnTantoCuadros ();
		}
		void MostrarYaumentarMovimiento () {

			if (NumeroDeMovimiento <= ListaDeMovimientosParaCalcular.Count - 1) {
				Movimiento MovimientoActual = ListaDeMovimientosParaCalcular[NumeroDeMovimiento];
				ComprobarMejorMovimientoaYactualizar (MovimientoActual);

				ListaDeMovimientosParaCalcular[NumeroDeMovimiento].estadoFinal.MostrarPiezasEnSusPosiciones ();

				NumeroDeMovimiento++;
				ActualizarValoresDeCalculo ();

			} else
				terminado = true;
		}

		void ActualizarValoresDeCalculo () {

			IAmovio = true;
			SePuedeMostrar = false;
			terminado = false;
		}
		void MostrarPosicionOriginalDelEstadoEnTantoCuadros () {
			if (IAmovio) {
				//Debug.Log ("ia movio");
				if (cuadrosParaMostrarJugada.EsperarCuadros (5) && estadoOriginal != null) {
					//Debug.Log (estadoOriginal.ListaDePiezas[0].Nombre + " poscion original " + estadoOriginal.ListaDePiezas[0].Casilla);
					//Debug.Log ("se prenseto");
					this.estadoOriginal.MostrarPiezasEnSusPosiciones ();
					IAmovio = false;

					SePresentoElEstadoOriginal = true;
				}
			}
		}
		void MostrarPosicionOriginalDelEstado () {
			if (IAmovio) {
				if (estadoOriginal != null) {
					this.estadoOriginal.MostrarPiezasEnSusPosiciones ();
					IAmovio = false;
					SePresentoElEstadoOriginal = true;
				}
			}
		}

	}
}