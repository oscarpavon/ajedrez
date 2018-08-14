using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AjedrezSupremo {

	public static class Partida {
		public static List<Movimiento> Movimientos = new List<Movimiento> ();

		public static List<Juego.Pieza> ListaDePiezas = new List<Juego.Pieza> ();
		public static List<Juego.Casilla> ListaDeCasillas = new List<Juego.Casilla> ();
		public static Turno Turno = Turno.Claros;

		public static void CambiarTurno () {
			if (Partida.Turno == Turno.Oscuros) {
				Partida.Turno = Turno.Claros;
			} else {
				Partida.Turno = Turno.Oscuros;
			}
		}

	}
	public enum Turno { Claros, Oscuros };
}

namespace AjedrezSupremo.Crear {

	public class Ajedrez {

		public Casilla[][] casillas;

		public static Casilla[] CasillaPeonesClaros;
		public static Casilla[] CasillaPeonesOscuros;

		public string[] letras;
		char letra = 'a';

		public GameObject Tablero;
		public Vector3 centroTablero;
		public GameObject Piezas;

		public List<Juego.Pieza> ListaDePiezas;
		string lll;

		int ANCHO = 8;
		int ALTO = 8;
		int cont = 0;
		public static void LimpiarTablero () {
			List<Juego.Pieza> piezas = Partida.ListaDePiezas;
			foreach (Juego.Pieza pieza in piezas) {
				pieza.Quitar ();
			}
		}
		public void Iniciar () {

			letras = new string[8];
			for (int i = 0; i <= 7; i++) {

				lll = letra.ToString ();
				lll = lll.ToUpper ();
				letras[i] = lll;
				letra++;

			}
			CasillaPeonesClaros = new Casilla[8];
			CasillaPeonesOscuros = new Casilla[8];
			CrearTablero (Color.black, Color.white);

			ListaDePiezas = new List<Juego.Pieza> ();
			CrearPiezas ();

		}

		public void CrearTablero (Color col1, Color col2) {

			Tablero = new GameObject ();
			Tablero.name = "Tablero";

			CrearBorde ();
			CrearCasillas ();
			PonerIdParaLasCasilla();
		}
		void CrearBorde () {
			GameObject BordeTablero = (GameObject) Resources.Load ("Borde");
			float tamanoborde = 0.165f;

			BordeTablero = GameObject.Instantiate (BordeTablero);
			//BordeTablero.transform.Rotate(new Vector3(90,0,0));
			BordeTablero.transform.localScale = new Vector3 (tamanoborde, tamanoborde, tamanoborde);
			Material matborde = new Material (Shader.Find ("Standard"));
			BordeTablero.GetComponent<MeshRenderer> ().material = matborde;
			BordeTablero.GetComponent<MeshRenderer> ().material.color = Color.gray;
			centroTablero = new Vector3 (Tablero.transform.position.x + 3.5f,
				Tablero.transform.position.y + 0.4f,
				Tablero.transform.transform.position.z + 3.5f);

			BordeTablero.transform.position = centroTablero;

			GameObject borde = new GameObject ("Borde");
			borde.transform.position = centroTablero;

			BordeTablero.transform.parent = borde.transform;
		}
		void CrearCasillas () {
			GameObject Casillas = new GameObject ("Casillas");
			Casillas.transform.parent = Tablero.transform;

			int peonCasillaClaroCont = 0;
			int peonCasillaOscuroCont = 0;

			casillas = new Casilla[ANCHO][];
			for (int i = 0; i < ANCHO; i++) {

				for (int x = 0; x < ALTO; x++) {
					casillas[i] = new Casilla[ALTO];
					Vector3 PosicionDeCasilla = new Vector3 (i, 0, x);

					Casilla casilla = new Casilla ();

					casilla.CrearCasilla (Casillas.transform, PosicionDeCasilla);

					if ((i + x) % 2 == 0) {
						

						casilla.materialOriginal = casilla.PonerColor ("Oscuro");

					} else {
						
						casilla.materialOriginal = casilla.PonerColor ("Claro");
					}
					//numero casilla
					casillas[i][x] = casilla;

					int mientras = x + 1;

					int otro = i + 1;

					string nombreDeCasilla = letras[i] + mientras.ToString ();

					casillas[i][x].casilla.name = nombreDeCasilla;

					if (x == 1) {
						CasillaPeonesClaros[peonCasillaClaroCont] = casilla;

						peonCasillaClaroCont++;
					}
					if (x == 6) {
						CasillaPeonesOscuros[peonCasillaOscuroCont] = casilla;
						peonCasillaOscuroCont++;
					}

					cont++;
					string nombrepos = letras[i] + mientras.ToString ();
					casillas[i][x].casilla.GetComponent<Juego.Casilla> ().nombre = nombrepos;
					casillas[i][x].casilla.GetComponent<Juego.Casilla> ().posicion = casillas[i][x].posicion;
					casillas[i][x].casilla.GetComponent<Juego.Casilla> ().casilla = casillas[i][x];
					AgregarAlistaDeCasillas(i,x);
				}

			}
		}
		void AgregarAlistaDeCasillas(int i, int x)
		{
			Juego.Casilla casilla = casillas[i][x].casilla.GetComponent<Juego.Casilla> ();
			Partida.ListaDeCasillas.Add(casilla);

		}
		void PonerIdParaLasCasilla()
		{
			int NumeroId = 0;
			foreach(Juego.Casilla cas in Partida.ListaDeCasillas){
				cas.id = NumeroId;
				NumeroId++;
			}
		}
		public void CrearPiezas () {
			Piezas = new GameObject ("Piezas");

			CargarPiezas ();
			
			
			//Piezas2();
			//Finales1 ();
			//Juego.Rey s = new Juego.Rey("A4");
			//NewMethod ();
			//CrearPieza ("Rey", "H1", "Claro");
			//CrearPeones ();
			// NewMethod1();
			
			
			//PrimerMiniMax ();
			
			//CrearPieza ("Alfil", "B1", "Oscuro");
			//CrearPieza ("Caballo", "A2", "Claro");
			//CrearPieza ("Alfil", "C2", "Claro");
			//MejorValor ();

		}
		void MejorValor () {
			CrearPieza ("Alfil", "B1", "Oscuro");
			CrearPieza ("Alfil", "G8", "Claro");
			CrearPieza ("Peon", "H7", "Claro");
			CrearPieza ("Peon", "A2", "Oscuro");
		}
		void PrimerMiniMax () {
			CrearPieza ("Alfil", "B1", "Oscuro");
			CrearPieza ("Torre", "A5", "Claro");
			//CrearPieza ("Peon", "C2", "Claro");
			//CrearPieza ("Caballo", "A2", "Claro");
			//CrearPieza ("Torre", "B5", "Oscuro");
			//CrearPieza ("Torre", "E5", "Claro");
			//CrearPieza ("Torre", "E8", "Claro");
			//CrearPieza ("Torre", "A2", "Claro");
			//CrearPieza ("Torre", "D3", "Claro");
		}

		private void PeonesPrueba () {
			CrearPieza ("Peon", "E6", "Oscuro");
			CrearPieza ("Peon", "F6", "Oscuro");
			CrearPieza ("Peon", "D6", "Oscuro");
			CrearPieza ("Peon", "E5", "Claro");
		}

		void Piezas2 () {
			//CrearPieza ("Torre", "E4", "Oscuro");
			//CrearPieza ("Peon", "B4", "Oscuro");
			//CrearPieza ("Rey", "F1", "Claro");
			CrearPieza ("Rey", "B8", "Oscuro");
			//CrearPieza ("Torre", "A2", "Claro");
			CrearPieza ("Torre", "C3", "Claro");
			CrearPieza ("Peon", "G7", "Oscuro");
			CrearPieza ("Peon", "F7", "Oscuro");
			CrearPieza ("Alfil", "H8", "Oscuro");
			CrearPieza ("Torre", "A2", "Oscuro");
		}
		private void Finales1 () {
			//CrearPieza("Alfil", "B1", "Oscuro");
			//CrearPieza ("Torre", "C2", "Oscuro");
			CrearPieza ("Torre", "A2", "Oscuro");
			CrearPieza ("Torre", "F8", "Oscuro");
			//CrearPieza ("Reina", "C5", "Claro");
			CrearPieza ("Rey", "H1", "Claro");
			CrearPieza ("Rey", "B4", "Oscuro");
			CrearPieza ("Caballo", "H2", "Oscuro");
			//CrearPieza("Torre", "G1", "Claro");
		}

		private void NewMethod () {
			CrearPieza ("Torre", "F3", "Oscuro");
			CrearPieza ("Torre", "A2", "Claro");
			CrearPieza ("Peon", "B3", "Claro");
			CrearPieza ("Peon", "A5", "Oscuro");
			CrearPieza ("Alfil", "D4", "Oscuro");

			CrearPieza ("Caballo", "E3", "Oscuro");

			CrearPieza ("Rey", "D1", "Claro");
		}

		void CargarPiezas () {
			CrearPeones ();
			CrearDosPiezas ("Rey", "E1", "E8");
			CrearDosPiezas ("Reina", "D1", "D8");
			CrearDosPiezas ("Alfil", "C1", "C8");
			CrearDosPiezas ("Alfil", "F1", "F8");
			CrearDosPiezas ("Caballo", "G1", "G8");
			CrearDosPiezas ("Caballo", "B1", "B8");
			CrearDosPiezas ("Torre", "A1", "A8");
			CrearDosPiezas ("Torre", "H1", "H8");
		}
		/// <summary>
		/// Crea los peones en sus posiciones iniciales
		/// </summary>
		void CrearPeones () {

			Pieza[] peones = new Pieza[16];

			for (int i = 0; i <= 7; i++) {

				peones[i] = new Pieza ();

				Juego.Pieza pie = peones[i].CrearPieza ("Peon", CasillaPeonesClaros[i].posicion);
				peones[i].PonerColor ("Claro");
				ListaDePiezas.Add (pie);
				ListaDePiezas[ListaDePiezas.Count - 1].id = ListaDePiezas.Count - 1;
				pie.gameObject.transform.SetParent (Piezas.transform);
				pie.GetComponent<Juego.Peon> ().posInicial =
					CasillaPeonesClaros[i].casilla.GetComponent<Juego.Casilla> ();

			}
			for (int i = 0; i <= 7; i++) {

				peones[i] = new Pieza ();

				Juego.Pieza pie = peones[i].CrearPieza ("Peon", CasillaPeonesOscuros[i].posicion);
				peones[i].PonerColor ("Oscuro");
				ListaDePiezas.Add (pie);
				ListaDePiezas[ListaDePiezas.Count - 1].id = ListaDePiezas.Count - 1;
				pie.gameObject.transform.SetParent (Piezas.transform);
				pie.GetComponent<Juego.Peon> ().posInicial =
					CasillaPeonesOscuros[i].casilla.GetComponent<Juego.Casilla> ();
			}
			//piezas = ListaDePiezas.ToArray ();
		}
		public static void MarcarCasillas (Juego.Casilla[] casillas) {
			foreach (Juego.Casilla cas in casillas) {
				MarcarCasilla (cas);
			}
		}

		public static void MarcarCasilla (Juego.Casilla cas) {
			if (cas.Ocupable) {
				Transform efec = cas.transform.GetChild (1);
				efec.gameObject.SetActive (true);
			} else {
				Transform efec = cas.transform.GetChild (0);
				efec.gameObject.SetActive (true);
			}

		}

		public static void DesmarcarCasillas (Juego.Casilla[] casillas) {
			for (int i = 0; i < casillas.Length; i++) {
				Juego.Casilla c = casillas[i];
				DesmarcarCasilla (c);
			}
		}
		static void DesmarcarCasilla (Juego.Casilla cas) {
			if (cas.Ocupable) {
				Transform efec = cas.transform.GetChild (1);
				efec.gameObject.SetActive (false);
			} else {
				Transform efec = cas.transform.GetChild (0);
				efec.gameObject.SetActive (false);
			}
			cas.Ocupable = false;
		}

		public void CrearPieza (string piezaAcrear, string pos1, string color) {
			Juego.Casilla casilla = BuscarCasilla (pos1);

			Pieza pieza = new Pieza ();

			Juego.Pieza piezaJuego = pieza.CrearPieza (piezaAcrear, casilla.posicion);
			pieza.PonerColor (color);

			pieza.PiezaPrincipal.transform.SetParent (Piezas.transform);

			ListaDePiezas.Add (piezaJuego);
			ListaDePiezas[ListaDePiezas.Count - 1].id = ListaDePiezas.Count - 1;
			//piezas = ListaDePiezas.ToArray ();

		}

		public void CrearPieza (string piezaAcrear, string pos) {
			Juego.Casilla casilla = BuscarCasilla (pos);
			Pieza pieza = new Pieza ();
			pieza.CrearPieza (piezaAcrear, casilla.posicion);
			pieza.PiezaPrincipal.transform.SetParent (Piezas.transform);
		}
		public void CrearDosPiezas (string piezaAcrear, string pos1, string pos2) {
			Juego.Casilla casilla = BuscarCasilla (pos1);
			Pieza piezaBlanca = new Pieza ();
			Pieza piezaOscura = new Pieza ();
			Juego.Pieza pieza1 = piezaBlanca.CrearPieza (piezaAcrear, casilla.posicion);
			piezaBlanca.PonerColor ("Claro");
			casilla = BuscarCasilla (pos2);
			Juego.Pieza pieza2 = piezaOscura.CrearPieza (piezaAcrear, casilla.posicion);
			piezaOscura.PonerColor ("Oscuro");
			piezaOscura.PiezaPrincipal.transform.SetParent (Piezas.transform);
			piezaBlanca.PiezaPrincipal.transform.SetParent (Piezas.transform);

			ListaDePiezas.Add (pieza1);
			ListaDePiezas[ListaDePiezas.Count - 1].id = ListaDePiezas.Count - 1;
			ListaDePiezas.Add (pieza2);
			ListaDePiezas[ListaDePiezas.Count - 1].id = ListaDePiezas.Count - 1;
			//piezas = ListaDePiezas.ToArray ();
		}

		public static Juego.Casilla[] BuscarCasillas (string[] casillas) {
			List<Juego.Casilla> cass = new List<Juego.Casilla> ();
			foreach (string cas in casillas) {
				cass.Add (BuscarCasilla (cas));
			}
			return cass.ToArray ();
		}
		public static Juego.Casilla BuscarCasilla (string casilla) {
			Juego.Casilla cas = null;
			if (casilla != null) {
				string textoAbuscar = casilla;
				GameObject casGO = GameObject.Find (textoAbuscar);
				if (casGO == null)
					Debug.Log ("Se busco = " + textoAbuscar);
				cas = casGO.GetComponent<Juego.Casilla> ();

			}

			return cas;
		}

		/// <summary>
		/// Convierte la Clase cicilla a string, la posicion
		/// </summary>
		/// <param name="cas"></param>
		/// <returns></returns>
		public static string[] CasillasAstring (Juego.Casilla[] cas) {
			List<string> casillasstring = new List<string> ();
			foreach (Juego.Casilla casilla in cas) {
				casillasstring.Add (casilla.nombre);
			}
			return casillasstring.ToArray ();
		}
		public static string[] CasillasAstring (List<Juego.Casilla> cas)

		{
			List<string> casillasstring = new List<string> ();
			foreach (Juego.Casilla casilla in cas) {
				casillasstring.Add (casilla.nombre);
			}
			return casillasstring.ToArray ();
		}

		public static AjedrezSupremo.Juego.Casilla[] ValidadCasillas (AjedrezSupremo.Juego.Casilla[] casillas) {
			List<AjedrezSupremo.Juego.Casilla> posiciones = new List<AjedrezSupremo.Juego.Casilla> ();
			//string[] casillasValida = posiciones.ToArr ay ();

			for (int i = 0; i < casillas.Length; i++) {
				if (ValidarCasilla (casillas[i])) {

					posiciones.Add (casillas[i]);
				}
			}
			AjedrezSupremo.Juego.Casilla[] casillasValidas = posiciones.ToArray ();
			return casillasValidas;
		}
		public static string[] ValidadCasillas (string[] casillas) {
			List<string> posiciones = new List<string> ();
			//string[] casillasValida = posiciones.ToArray ();

			for (int i = 0; i < casillas.Length; i++) {
				if (ValidarCasilla (casillas[i])) {

					posiciones.Add (casillas[i]);
				}
			}
			string[] casillasValidas = posiciones.ToArray ();
			return casillasValidas;
		}
		public static bool ValidarCasilla (AjedrezSupremo.Juego.Casilla Casilla) {
			bool casillaValidada = false;
			string casillaAct = Casilla.nombre;

			var Chars = casillaAct.ToCharArray ();
			int i = (int) (Chars[1] - '0');
			int o = (int) (Chars[0] - '0');

			int[] letraNumero = new int[8];
			int val = 17;
			for (int j = 0; j < 8; j++) {

				letraNumero[j] = val;
				val++;
			}
			int fl = Array.IndexOf (letraNumero, o); //valor de letra comenzando en 0; 0 == A

			if (i > 8 || i < 1 || fl > 8 || fl < 0) {
				casillaValidada = false;
			} else
				casillaValidada = true;

			return casillaValidada;
		}
		public static bool ValidarCasilla (string Casilla) {
			bool casillaValidada = false;
			string casillaAct = Casilla;

			var Chars = casillaAct.ToCharArray ();
			int i = (int) (Chars[1] - '0');
			int o = (int) (Chars[0] - '0');

			int[] letraNumero = new int[8];
			int val = 17;
			for (int j = 0; j < 8; j++) {

				letraNumero[j] = val;
				val++;
			}
			int fl = Array.IndexOf (letraNumero, o); //valor de letra comenzando en 0; 0 == A

			if (i > 8 || i < 1 || fl > 8 || fl < 0) {
				casillaValidada = false;
			} else
				casillaValidada = true;

			return casillaValidada;
		}
		/// <summary>
		/// No marca si la cacilla esta ocupada o amenazada
		/// </summary>
		/// <param name="pieza"></param>
		/// <param name="Casillas"></param>
		/// <returns></returns>
		public static Juego.Casilla[] CacillaOcupada (Juego.Pieza pieza, Juego.Casilla[] Casillas) {
			List<Juego.Casilla> listacasillas = new List<Juego.Casilla> ();
			foreach (Juego.Casilla casilla in Casillas) {
				Juego.Casilla cas = casilla;
				listacasillas.Add (cas);
				if (cas.Ocupada) {
					listacasillas.Remove (cas);
					if (pieza.name == "Caballo") {
						if (cas.pieza.ColorPieza != pieza.ColorPieza) {
							listacasillas.Add (cas);
							cas.Ocupable = true;
						}
					}
					if (pieza.name == "Rey") {
						if (cas.pieza.ColorPieza != pieza.ColorPieza) {

							if (pieza.ColorPieza == Juego.color.Claro) {
								if (cas.AmenazadaPorOscuros.Count < 1) {
									cas.Ocupable = false;
									listacasillas.Add (cas);
								} else
									listacasillas.Remove (cas);
							} else {
								if (cas.AmenazadaPorClaros.Count < 1) {
									cas.Ocupable = false;
									listacasillas.Add (cas);
								} else
									listacasillas.Remove (cas);
							}

						} else {
							listacasillas.Remove (cas);
						}
					}
					if (pieza.name == "Peon") {
						if (cas.pieza.ColorPieza != pieza.ColorPieza) {
							if (cas.marcar) {
								listacasillas.Add (cas);
								cas.Ocupable = true;
							}

						} else {
							listacasillas.Remove (cas);
						}
					}

				}
			}

			return listacasillas.ToArray ();
		}

		/// <summary>
		/// Deja de marcar las casillas cuando hay algo en el camino
		/// </summary>
		/// <param name="pieza"></param>
		/// <param name="Casillas"></param>
		/// <param name="calcAmenazas">Si es true devuelve una cacilla mas que esta ocupada por un pieza de igual color</param>
		/// <returns></returns>
		public static string[] CasillasHastaQueEsteOcupadaEnEstado (EstadoDeJuego estado,Juego.Pieza pieza, string[] Casillas, bool calcAmenazas) {
			int cantidad = 0;

			foreach (string casBus in Casillas) {
				EstadoDeJuego.CasillaDeEstado cas = estado.BuscarCasilla(casBus);
				//Debug.Log("Se encontro la casilla " + cas.Nombre);
				//Debug.Log("Ocupada? " + cas.EstaOcupada.ToString());
				if (!cas.EstaOcupada) {
					cantidad++;
				} else {
					if (pieza.ColorPieza == cas.pieza.ColorPieza) {
						if (calcAmenazas) {
							cantidad++;
						}

					}

					if (pieza.name != "Peon") {
						if (cas.pieza.ColorPieza != pieza.ColorPieza) {
							cantidad++;
							//cas.Ocupable = true;
						}
					}
					break;
				}

			}
			string[] posibles = new string[cantidad];
			for (int i = 0; i < posibles.Length; i++) {
				posibles[i] = Casillas[i];
			}

			return posibles;
		}
		public static string[] CasillasHasta (Juego.Pieza pieza, string[] Casillas, bool calcAmenazas) {
			int cantidad = 0;

			foreach (string casBus in Casillas) {
				Juego.Casilla cas = BuscarCasilla (casBus);
				if (!cas.Ocupada) {
					cantidad++;
				} else {
					if (pieza.ColorPieza == cas.pieza.ColorPieza) {
						if (calcAmenazas) {
							cantidad++;
						}

					}

					if (pieza.name != "Peon") {
						if (cas.pieza.ColorPieza != pieza.ColorPieza) {
							cantidad++;
							cas.Ocupable = true;
						}
					}
					break;
				}

			}
			string[] posibles = new string[cantidad];
			for (int i = 0; i < posibles.Length; i++) {
				posibles[i] = Casillas[i];
			}

			return posibles;
		}
		public static string[] CasillasHasta (Juego.Pieza pieza, string[] Casillas) {
			int cantidad = 0;

			foreach (string casBus in Casillas) {
				Juego.Casilla cas = BuscarCasilla (casBus);
				if (!cas.Ocupada) {
					cantidad++;
					break;
				} else {
					if (pieza.name != "Peon") {
						if (cas.pieza.ColorPieza != pieza.ColorPieza) {
							cantidad++;
							//cas.Ocupable = true;
						} else { //pieza mismo color
							if(cantidad>=1){
								cantidad--;
							}
							
						}
					}
					break;
				}

			}
			
			string[] posibles = new string[cantidad];
			for (int i = 0; i < posibles.Length; i++) {
				posibles[i] = Casillas[i];
			}

			return posibles;
		}

	}
}