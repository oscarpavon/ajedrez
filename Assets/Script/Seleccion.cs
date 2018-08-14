using System.Collections;
using System.Collections.Generic;
using AjedrezSupremo.Juego;
using UnityEngine;
using Ajedrez = AjedrezSupremo.Crear.Ajedrez;
public class Seleccion : MonoBehaviour {

	Ray rayCamaraMouse;

	public Pieza piezaColicionada;
	public Pieza piezaAnterior;
	public Pieza piezaSeleccionada;
	GameObject ObjetoColisionado;
	Casilla[] casillasMarcadas;
	Casilla[] casillasMarcadasAnt;

	bool Sel = false;
	void Update () {
		rayCamaraMouse = Camera.main.ScreenPointToRay (Input.mousePosition);

		GameObject colisionado = Clic (0);
		if (colisionado != null) {
			piezaColicionada = colisionado.GetComponent<Pieza> ();
			if (piezaColicionada != null) {

				if (piezaSeleccionada != null) {
					if (piezaColicionada != piezaSeleccionada) {

						piezaAnterior = piezaSeleccionada;
						piezaAnterior.Seleccionada = false;
						piezaAnterior.MarcarDesamarcar ();
						EsComible ();
					}
				}
				if (!Sel) {
					piezaSeleccionada = piezaColicionada;
					piezaSeleccionada.Seleccionada = true;
					piezaSeleccionada.MarcarDesamarcar ();
					Sel = false;
				}
				Sel = false;
			}
		}

		MoverACasilla ();

	}
	void EsComible () {
		if (piezaColicionada.ColorPieza != piezaSeleccionada.ColorPieza) {
			Casilla casAmover = piezaColicionada.casilla;
			MoverA (casAmover);
			piezaColicionada.gameObject.SetActive (false);
			Sel = true;

		}
	}
	void SeleccionDePieza () {

	}
	void Pieza () {

	}
	void MoverA (Casilla cas) {
		piezaSeleccionada.MoverA (cas);
		piezaSeleccionada.Seleccionada = false;
		piezaSeleccionada.MarcarDesamarcar ();
		piezaSeleccionada = null;
		AjedrezSupremo.Partida.CambiarTurno ();
	}
	void MoverACasilla () {
		GameObject colisionado = Clic (0);
		if (colisionado != null) {
			Casilla CasillaSeleccionada = colisionado.GetComponent<Casilla> ();
			if (CasillaSeleccionada != null) {
				if (piezaSeleccionada != null) {
					MoverA (CasillaSeleccionada);
				}

			}
		}

	}
	GameObject Clic (int clic) {
		GameObject colisionado = null;
		RaycastHit hit = new RaycastHit ();
		if (Input.GetMouseButtonDown (clic)) {
			if (Physics.Raycast (rayCamaraMouse, out hit)) {
				colisionado = hit.collider.gameObject;

			}
		}
		return colisionado;
	}
}