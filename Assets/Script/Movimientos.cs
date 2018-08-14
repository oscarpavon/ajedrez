using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AjedrezSupremo {
	public class Movimiento {
		public Juego.Pieza PiezaMovida;
		public Juego.Casilla CasillaElegida;
		Juego.Casilla CasillaAnterior;

		public int Valor = 1;
		public EstadoDeJuego estadoFinal;

		EstadoDeJuego EstadoAnterior;
		Juego.Casilla CasillaDondeMover;
		Juego.Pieza PiezaQueSeVaMover;

		public Movimiento (Juego.color col) {
			this.estadoFinal = new EstadoDeJuego ();
			if (col == Juego.color.Claro) {
				this.estadoFinal.ValorDeEstado = 999;
			} else {
				this.estadoFinal.ValorDeEstado = -999;
			}
		}
		public Movimiento (Juego.Pieza Pieza, Juego.Casilla Casilla) {
			EstadoDeJuego.PiezaDeEstado pieza = new EstadoDeJuego.PiezaDeEstado (Pieza, Casilla);
			//Debug.Log("Se va crear un movimiento logico para la pieza " + Pieza.name + " de la casilla " + Pieza.casilla.nombre);
			Debug.Log ("Se creo un pieza de estado de juego " +
				pieza.Nombre + " con la casilla " + pieza.Casilla);
			this.PiezaMovida = Pieza;
			this.CasillaElegida = Casilla;
			this.CasillaAnterior = this.PiezaMovida.casilla;

			this.PiezaMovida.casilla = Casilla;
			//Juego.Pieza piezaMovida = Pieza;
			EstadoDeJuego.PiezaDeEstado piezaAquitar = null;
			if (Casilla.pieza != null) {
				//Debug.Log("se va quitar");

				piezaAquitar = new EstadoDeJuego.PiezaDeEstado (Casilla.pieza);
			}
			//piezaMovida.casilla = Casilla;
			CrearEstadoFinal (piezaAquitar);
			if (Casilla.pieza != null) {

				this.Valor = Casilla.pieza.ValorPorColor ();
			}
			this.PiezaMovida.casilla = CasillaAnterior;
		}
		public Movimiento (Juego.Pieza PiezaQueSeVaMover, Juego.Casilla CasillaDondeMover, EstadoDeJuego estadoAnterior) {

			this.EstadoAnterior = estadoAnterior;
			
			this.CasillaDondeMover = CasillaDondeMover;
			this.PiezaQueSeVaMover = PiezaQueSeVaMover;
			DarValorDeMovimiento ();
			this.EstadoAnterior = new EstadoDeJuego();
			
			

			EstadoDeJuego.PiezaDeEstado piezaAquitar = null;
			if (CasillaDondeMover.pieza != null) {

				piezaAquitar = new EstadoDeJuego.PiezaDeEstado (CasillaDondeMover.pieza);
			}

			CrearEstadoFinalDesdeEstadoAnterior (piezaAquitar);
			//ActualizarEstadoFinal ();
			QuitarTodasLasCasillasOcupadas ();
			AgregarCasillaOcupada ();
			//int id = CasillaDondeMover.id;
			//Debug.Log(id);
			//estadoFinal.CasillasDeEstado[id].EstaOcupada = true;
			//estadoFinal.ActualizarCasillasOcupadas ();
			
		}

		void DarValorDeMovimiento () {
			EstadoDeJuego.CasillaDeEstado casEstado = EstadoAnterior.BuscarCasilla (CasillaDondeMover.nombre);
			if (casEstado.EstaOcupada) {
				if (casEstado.pieza.ColorPieza != PiezaQueSeVaMover.ColorPieza) {
					this.Valor = casEstado.pieza.ValorPorColor ();
				} else
					Debug.Log ("Error no se puede ocupar la casilla");
			}
		}

		public static Movimiento CrearMovimientoEnEstado () {
			return null;
		}
		void CrearEstadoFinalDesdeEstadoAnterior (EstadoDeJuego.PiezaDeEstado piezaAquitar) {

			EstadoDeJuego estado = new EstadoDeJuego (this.EstadoAnterior);
			//estado.ActualizarCasillasOcupadas();
			if (piezaAquitar != null) {
				estado.QuitarPieza (piezaAquitar);

			}

			estadoFinal = estado;

		}

		void CrearEstadoFinal (EstadoDeJuego.PiezaDeEstado piezaAquitar) {

			EstadoDeJuego estado = new EstadoDeJuego ();
			if (piezaAquitar != null) {
				Debug.Log ("se quito " + piezaAquitar.Nombre + " de la lista a presentar");
				estado.QuitarPieza (piezaAquitar);
				Debug.Log ("cantidad de piezas despues de quitar= " + estado.IdsActivos.Count.ToString ());
				Debug.Log ("Cantidad de piezas blancas despues de quitar " + estado.IdsActivosPiezasClaras.Count.ToString ());
			}

			Debug.Log ("Cantidad de piezas " + estado.ListaDePiezasDeEstado.Count.ToString ());
			//pieza.EstadoDelJuego = estado;
			//Debug.Log("Se creo un estado para la pieza " + pieza.Nombre);
			//Debug.Log("La pieza " + estado.ListaDePiezas[0].Nombre +
			//" se guadro en la casilla " + estado.ListaDePiezas[0].Casilla);

			estadoFinal = estado;

		}

		void QuitarTodasLasCasillasOcupadas () {
			EstadoDeJuego.PiezaDeEstado piezaAmover = this.EstadoAnterior.BuscarPieza (this.PiezaQueSeVaMover);
			string casAnterior = piezaAmover.Casilla;
			EstadoDeJuego.CasillaDeEstado casAnteriorParaActualizar = estadoFinal.BuscarCasilla (casAnterior);
			estadoFinal.CasillasDeEstado[casAnteriorParaActualizar.idPorOrdenAgregado].EstaOcupada = false;
			estadoFinal.CasillasDeEstado[casAnteriorParaActualizar.idPorOrdenAgregado].pieza = null;
		}
		void AgregarCasillaOcupada () {
			EstadoDeJuego.CasillaDeEstado cas = estadoFinal.BuscarCasilla (this.CasillaDondeMover.nombre);
			estadoFinal.CasillasDeEstado[cas.idPorOrdenAgregado].EstaOcupada = true;
			estadoFinal.CasillasDeEstado[cas.idPorOrdenAgregado].pieza = PiezaQueSeVaMover;

		}
		void ActualizarEstadoFinal () {
			EstadoDeJuego.PiezaDeEstado piezaAmover = this.EstadoAnterior.BuscarPieza (this.PiezaQueSeVaMover);
			string casAnterior = piezaAmover.Casilla;
			EstadoDeJuego.CasillaDeEstado casAnteriorParaActualizar = estadoFinal.BuscarCasilla (casAnterior);
			//estadoFinal.CasillasDeEstado[casAnteriorParaActualizar.idPorOrdenAgregado].EstaOcupada = false;
			estadoFinal.CasillasDeEstado[casAnteriorParaActualizar.idPorOrdenAgregado].pieza = null;

			EstadoDeJuego.CasillaDeEstado cas = estadoFinal.BuscarCasilla (this.CasillaDondeMover.nombre);
			estadoFinal.CasillasDeEstado[cas.idPorOrdenAgregado].EstaOcupada = true;
			estadoFinal.CasillasDeEstado[cas.idPorOrdenAgregado].pieza = PiezaQueSeVaMover;

			EstadoDeJuego.CasillaDeEstado casAnt = estadoFinal.CasillasDeEstado[casAnteriorParaActualizar.idPorOrdenAgregado];
			EstadoDeJuego.CasillaDeEstado casAct = estadoFinal.CasillasDeEstado[cas.idPorOrdenAgregado];
		}

	}

}