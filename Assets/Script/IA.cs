using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AjedrezSupremo.Juego {

	public class IA : MonoBehaviour {

		public Nivel NivelActual = Nivel.Nivel1;
		public Turno JugarComo = Turno.Oscuros;
		public List<Pieza> piezas;
		public Pieza piezaSeleccionada;

		public Pieza piezaAnterior;

		public Casilla[] casPosPieSelc;

		public bool Debugaa = false;//Detenine el movimeinto automatico de la IA
		public bool siguientePieza = false;
		public bool mov = true;
		public bool seleccionar = false;

		public bool mover = false;

		public int cuadrosAesperar = 0;

		List<int> idPiezaOscuras = new List<int> ();
		List<int> idPiezaClaras = new List<int> ();
		public Rey Rey;

		public bool presesntar = false;

		public float cantidadAnalisis = 0;
		public float cantidadAnalisisContrario = 0;
		public bool analizar = false;
		void Start () {
			JugarComo = Turno.Oscuros;

		}

		void Update () {

			if (Debugaa) {
				if (mov) {
					if (JugarComo == Partida.Turno) {
						//if(mov)
						if (seleccionar) {
							CatalogarPiezas ();
							ElegirPieza ();
							if (siguientePieza) {
								piezaSeleccionada.Seleccionada = false;
								piezaSeleccionada = null;
								siguientePieza = false;
							}

							if (mover)
								Mover ();
						}

					}
				}
			} else {
				if (JugarComo == Partida.Turno) {
					Jugadas2 ();
				}
			}

			if (Debugaa) {
				if (mov) {
					CatalogarPiezas ();
					//MiniMax ();
					PruebaPresentar ();
				}
			}

		}

		//eligue un pieza aleatoria y su movimento aleatorio
		void JugadasUno () {
			CatalogarPiezas ();
			ElegirPieza ();

			if (cuadrosAesperar >= 20) {
				Mover ();
				cuadrosAesperar = 0;
			}
			cuadrosAesperar += 1;
		}
		//mueve aleatoriamiente
		void Jugadas2 () {
			CatalogarPiezas ();
			//ElegirPieza ();

			Movimiento mElegido = ElegirMovimientoAleatorio ();
			Pieza pieza = mElegido.PiezaMovida;
			Casilla cas = mElegido.CasillaElegida;

			if (cuadrosAesperar >= 20) {
				Mover (pieza, cas);
				cuadrosAesperar = 0;
			}
			cuadrosAesperar += 1;
		}
		//Ya sabe que comer
		void Jugadas3 () {
			CatalogarPiezas ();
			Movimiento mElegido = MejorMovimiento ();
			Pieza pieza = mElegido.PiezaMovida;
			Casilla cas = mElegido.CasillaElegida;

			if (cuadrosAesperar >= 20) {
				Mover (pieza, cas);
				cuadrosAesperar = 0;
			}
			cuadrosAesperar += 1;
		}

		Movimiento ElegirMovimientoAleatorio () {
			List<Movimiento> movs = TodosLosMovimientosDeMisPiezas ();

			int m = Random.Range (0, movs.Count);
			Movimiento mElegido = movs[m];
			Pieza pieza = mElegido.PiezaMovida;
			Casilla cas = mElegido.CasillaElegida;
			return mElegido;
		}
		void Mover (Pieza pieza, Casilla cas) {

			piezaAnterior = pieza;
			pieza.MoverA (cas);

			pieza.Seleccionada = false;
			pieza = null;
			Partida.CambiarTurno ();

		}
		void Mover () {

			Casilla casilla = ElegirCasilla ();

			piezaAnterior = piezaSeleccionada;
			piezaSeleccionada.MoverA (casilla);

			piezaSeleccionada.Seleccionada = false;
			piezaSeleccionada = null;
			Partida.CambiarTurno ();

		}
		Casilla ElegirCasilla () {

			casPosPieSelc = piezaSeleccionada.Movimientos ();
			//Debug.Log(casPosPieSelc.Length.ToString());
			int numero = Random.Range (0, casPosPieSelc.Length);
			return casPosPieSelc[numero];
		}
		void ElegirPieza () {

			if (NivelActual == Nivel.Nivel1) {
				if (Rey.Hake != true) {
					PiezaRandon ();
				} else {
					piezaSeleccionada = Rey;
				}
				casPosPieSelc = piezaSeleccionada.Movimientos ();
				if (casPosPieSelc == null || casPosPieSelc.Length == 0) {
					piezaSeleccionada = null;

				}

			}
		}

		void PiezaRandon () {
			if (piezaSeleccionada == null) {
				int numero = Random.Range (0, piezas.Count);
				piezaSeleccionada = piezas[numero];

			}

		}
		void CatalogarPiezas () {
			Piezas ();
			foreach (Pieza pieza in Partida.ListaDePiezas) {
				if (pieza.name == "Rey") {
					Rey = pieza.GetComponent<Rey> ();
				}
			}
		}
		public void Piezas () {
			foreach (Juego.Pieza pieza in Partida.ListaDePiezas) {
				if (pieza.ColorPieza == color.Oscuro) {
					idPiezaOscuras.Add (pieza.id);
					idPiezaOscuras = idPiezaOscuras.Distinct ().ToList ();
				} else {
					idPiezaClaras.Add (pieza.id);
					idPiezaClaras = idPiezaClaras.Distinct ().ToList ();
				}

			}
		}
		public void Piezas (EstadoDeJuego estado) {

			foreach (int idPieza in estado.IdsActivos) {
				if (Partida.ListaDePiezas[idPieza].ColorPieza == color.Oscuro) {
					idPiezaOscuras.Add (idPieza);
					idPiezaOscuras = idPiezaOscuras.Distinct ().ToList ();
				}
				if (Partida.ListaDePiezas[idPieza].ColorPieza == color.Claro) {
					idPiezaClaras.Add (idPieza);
					idPiezaClaras = idPiezaClaras.Distinct ().ToList ();
				}
			}
		}
		List<Movimiento> TodosLosMovimientosDeMisPiezas () {
			//obtener todos los movimientos posibles de las piezas de IA
			List<Movimiento> movimientos = new List<Movimiento> ();
			//Debug.Log ("cantida pieza negras " + idPiezaOscuras.Count.ToString ());
			foreach (int pieza in idPiezaOscuras) {
				Partida.ListaDePiezas[pieza].CasillaAnterior = Partida.ListaDePiezas[pieza].casilla;
				List<Movimiento> movPieza = Partida.ListaDePiezas[pieza].JugadasPosibles ();
				//Debug.Log(movPieza.Count.ToString());
				if (movPieza.Count >= 1) {
					movimientos.AddRange (movPieza);
				}

			}

			return movimientos;
		}
		List<Movimiento> TodosLosMovimientosDelContrario (EstadoDeJuego estado) {

			//obtener todos los movimientos posibles de las piezas de IA
			List<Movimiento> movimientos = new List<Movimiento> ();
			//Debug.Log ("cantida pieza blancas " + idPiezaClaras.Count.ToString ());
			//Debug.Log ("piezas activas claras" + estado.PiezasActivasClaras.Count.ToString ());
			foreach (int pieza in estado.IdsActivosPiezasClaras) {
				Partida.ListaDePiezas[pieza].CasillaAnterior = Partida.ListaDePiezas[pieza].casilla;
				List<Movimiento> movPieza = Partida.ListaDePiezas[pieza].JugadasPosiblesDesdeEstado (estado);

				if (movPieza.Count >= 1) {
					movimientos.AddRange (movPieza);
				}

			}

			return movimientos;
		}

		public static List<Movimiento> CalcularMovimientosPorColorEnEstado (Juego.color color, EstadoDeJuego estado) {

			List<Movimiento> movimientos = new List<Movimiento> ();
			if (color == color.Claro) {
				movimientos = CalcularMovimientosDesdeListaDePiezasEnEstado (estado.IdsActivosPiezasClaras, estado);
			} else {
				movimientos = CalcularMovimientosDesdeListaDePiezasEnEstado (estado.IdsActivosPiezasOscuras, estado);
			}
			return movimientos;
		}

		static List<Movimiento> CalcularMovimientosDesdeListaDePiezasEnEstado (List<int> ListaDeIdsPiezas, EstadoDeJuego estado) {
			List<Movimiento> TodosLosMovimientosDeLaListaDePiezas = new List<Movimiento> ();
			ActualizarPosicionesDePiezas (estado);
			foreach (int idPieza in ListaDeIdsPiezas) {

				Partida.ListaDePiezas[idPieza].CasillaAnterior = Partida.ListaDePiezas[idPieza].casilla;
				
				List<Movimiento> MovimientosDeUnaPieza = Partida.ListaDePiezas[idPieza].JugadasPosiblesDesdeEstado (estado);
				
				if (MovimientosDeUnaPieza.Count >= 1) {
					TodosLosMovimientosDeLaListaDePiezas.AddRange (MovimientosDeUnaPieza);
				}
			}
			return TodosLosMovimientosDeLaListaDePiezas;
		}
		static void ActualizarPosicionesDePiezas (EstadoDeJuego estado) {
			foreach (EstadoDeJuego.PiezaDeEstado piezaEstadoActual in estado.ListaDePiezasDeEstado) {

				Casilla cas = AjedrezSupremo.Crear.Ajedrez.BuscarCasilla (piezaEstadoActual.Casilla);
				Partida.ListaDePiezas[piezaEstadoActual.id].casilla = cas;
				cas.pieza = Partida.ListaDePiezas[piezaEstadoActual.id];
			}			
		}

		Movimiento MejorMovimiento () {
			List<Movimiento> tmos = TodosLosMovimientosDeMisPiezas ();
			Movimiento mejor = null;
			int mejorValor = -999;
			foreach (Movimiento mov in tmos) {
				int valorMovActual = mov.Valor;
				if (valorMovActual > mejorValor) {
					mejorValor = valorMovActual;
					mejor = mov;
				}
			}

			return mejor;
		}
		Movimiento MayorValorMov (List<Movimiento> movs) {
			List<Movimiento> tmos = movs;
			Movimiento mejor = null;
			int mejorValor = -999;
			foreach (Movimiento mov in tmos) {
				int valorMovActual = mov.Valor;
				if (valorMovActual > mejorValor) {
					mejorValor = valorMovActual;
					mejor = mov;
				}
			}

			return mejor;
		}

		void PruebaPresentar () {
			if (presesntar) {
				EstadoDeJuego estadoOriginal = new EstadoDeJuego ();
				List<Movimiento> movs = TodosLosMovimientosDeMisPiezas ();
				if (cantidadAnalisis >= movs.Count) {
					analizar = false;
				} else {
					analizar = true;
				}
				if (analizar) {

					//EstadoDeJuego estado2 = movs[5].estadoFinal;
					//MejorMovBlancas(estado2);

					EstadoDeJuego estado = movs[6].estadoFinal;
					MejorMovBlancas (estado);
					//estado.Presentar ();
					//estadoOriginal.Presentar ();

				}
				presesntar = false;
			}

		}		

		void MejorMovBlancas (EstadoDeJuego estado) {
			estado.MostrarPiezasEnSusPosiciones (); //coloca las posiciones del estado actual
			List<Movimiento> movsContra = TodosLosMovimientosDelContrario (estado);
			Movimiento mejorBlancas = null;
			int mejorValorBlancas = 999;

			foreach (Movimiento mov in movsContra) {
				//mov.estadoFinal.Presentar ();

				int valorMovBlanca = mov.Valor;
				if (valorMovBlanca < mejorValorBlancas) {
					mejorValorBlancas = valorMovBlanca;
					mejorBlancas = mov;
					cantidadAnalisisContrario++;
				}
			}
			Debug.LogWarning ("mejor mov blanca " + mejorBlancas.CasillaElegida.nombre);

		}
		void MiniMax () {

			//todos los movimientos

			if (presesntar) {

				List<Movimiento> movs = TodosLosMovimientosDeMisPiezas ();
				Debug.Log (movs.Count.ToString ());
				if (cantidadAnalisis >= movs.Count) {
					analizar = false;
				} else {
					analizar = true;
				}
				if (analizar) {
					Movimiento mejorNegras = null;
					int mejorValor = -999;
					foreach (Movimiento mov in movs) {
						mov.estadoFinal.MostrarPiezasEnSusPosiciones ();
						cantidadAnalisis++;

						int valorMovActual = mov.Valor;
						if (valorMovActual > mejorValor) {
							mejorValor = valorMovActual;
							mejorNegras = mov;
						}

						MejorMovBlancas (mov.estadoFinal);
					}
					Debug.Log ("mejor mov " + mejorNegras.CasillaElegida.nombre);
					//posicion original antes de calculos
					foreach (int pieza in idPiezaOscuras) {
						Casilla cas = Partida.ListaDePiezas[pieza].CasillaAnterior;
						Partida.ListaDePiezas[pieza].Mover (cas);
					}
				}

				presesntar = false;
			}

			//guarda estado actual ante de calculos

			//movimiento contrario

			//actualizo el estado del juegp
			//hago el movimiento del contrario

		}

	}
	public enum Nivel { Nivel1, Nivel2 };
}