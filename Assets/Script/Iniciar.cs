using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AjedrezSupremo.Juego;
using UnityEngine;

public class Iniciar : MonoBehaviour {

	Vector3 posAnt;
	Ray rayCamaraMouse;

	AjedrezSupremo.Crear.Ajedrez ajedrez;

	public AjedrezSupremo.Turno turno = AjedrezSupremo.Turno.Claros;
	public bool movio;
	public bool agarroPieza;

	public Pieza piezaSeleccionada;

	public string[] casillasAmenazadas;

	int contadorJugada = 12;
	bool Inicio = false;	


	bool CambiarDeJugador = false;
	public bool IA = false;

	public List<Casilla> Casillas;
	void Start () {
		//Activar Inteligencia Artificial super basica
		if(IA)
			this.gameObject.AddComponent<IA> ();
		
		ajedrez = new AjedrezSupremo.Crear.Ajedrez ();
		ajedrez.Iniciar ();		
		string s = ajedrez.ListaDePiezas.Count.ToString();
		//Debug.Log(s);
		AjedrezSupremo.Partida.ListaDePiezas.AddRange(ajedrez.ListaDePiezas);
		s = AjedrezSupremo.Partida.ListaDePiezas.Count.ToString();
		//Debug.Log(s);
		//Piezas = ajedrez.ListaDePiezas;
		//AjedrezSupremo.Partida.ListaDePiezas = Piezas;
		
		CamaraConf ();
		Inicio = true;
		casillasAmenazadas = new string[1];

		Casillas = AjedrezSupremo.Partida.ListaDeCasillas;
		//

	}

	void Update () {
		rayCamaraMouse = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		turno = AjedrezSupremo.Partida.Turno;
		
		//AjedrezSupremo.Partida.ListaDePiezas = Piezas;
		if(CambiarDeJugador)
		{
			AjedrezSupremo.Partida.CambiarTurno();
			CambiarDeJugador = false;
		}
		
	}





	void ArrastrarPiezaConMouse () {
		RaycastHit hit = new RaycastHit ();
		Vector3 distancia = Vector3.zero;
		if (Input.GetMouseButtonDown (0)) {
			if (piezaSeleccionada != null) {
				posAnt = piezaSeleccionada.transform.position;
				distancia = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0)) - piezaSeleccionada.transform.position;

			}
		}
		if (Input.GetMouseButton (0)) {
			if (Physics.Raycast (rayCamaraMouse, out hit)) {
				if (hit.collider.GetComponent<AjedrezSupremo.Juego.Pieza> () != null) {
					agarroPieza = true;
					piezaSeleccionada.Levantada = true;

				}
				if (agarroPieza) {
					Vector3 distanciaDEpantalla = Camera.main.WorldToScreenPoint (piezaSeleccionada.transform.position);

					Vector3 moupos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distanciaDEpantalla.z);
					//moupos = new Vector3(disx,distanciaDEpantalla.y,disz);
					Vector3 posActual = Camera.main.ScreenToWorldPoint (moupos);

					Vector3 final = new Vector3 (posActual.x, piezaSeleccionada.transform.position.y, posActual.z);
					piezaSeleccionada.transform.position = final;
				}
			}

		}

		if (Input.GetMouseButtonUp (0)) {
			if (piezaSeleccionada != null) {
				if (!movio)
					piezaSeleccionada.transform.position = posAnt;
				movio = false;
				agarroPieza = false;
				piezaSeleccionada.Levantada = false;
			}

		}

	}
	public void CamaraConf () {

		GameObject ContenedorCamara = new GameObject ("Contenedor Camara");
		GameObject pivot = new GameObject ("Pivot");

		Vector3 centroTablero = new Vector3 (ajedrez.Tablero.transform.position.x + 3.5f,
			ajedrez.Tablero.transform.position.y + 0.5f,
			ajedrez.Tablero.transform.transform.position.z + 3.5f);

		ContenedorCamara.transform.position = centroTablero;

		ContenedorCamara.AddComponent<ManejadorCamara> ();
		ContenedorCamara.GetComponent<ManejadorCamara> ().pivot = pivot.transform;
		ContenedorCamara.GetComponent<ManejadorCamara> ().valores = Resources.Load ("Valores Camara") as ValoresDeCamara;

		Vector3 posCam = new Vector3 (centroTablero.x, centroTablero.y, centroTablero.z - 12);
		pivot.transform.position = centroTablero;

		ContenedorCamara.transform.parent = ajedrez.Tablero.transform;
		pivot.transform.parent = ContenedorCamara.transform;

		Camera.main.transform.parent = pivot.transform;
		Camera.main.transform.position = Vector3.zero;
		Camera.main.fieldOfView = 45;
		Camera.main.transform.position = posCam;
	}

}