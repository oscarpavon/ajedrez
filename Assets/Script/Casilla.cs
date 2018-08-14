using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AjedrezSupremo.Crear {

	public class Casilla {

		
		public Vector3 posicion;
		public GameObject casilla;

		public Material materialOriginal;
		Material ColorClaro;
		Material ColorOscuro;
		Material ColorMovimiento;

		public void CrearCasilla (Transform tablero, Vector3 pos) {
			CrearMateriales ();
			casilla = GameObject.CreatePrimitive (PrimitiveType.Cube);
			PonerNombreAlObjetoDeJuego ("Casilla");
			casilla.AddComponent<AjedrezSupremo.Juego.Casilla> ();
			casilla.transform.SetParent (tablero);
			casilla.transform.position = pos;

			float offset = 0.5f;//Porque es un cubo
			Vector3 nuevaPos = new Vector3 (casilla.transform.position.x,
				casilla.transform.position.y +
				offset, casilla.transform.position.z);
			posicion = nuevaPos;
			CrearEfectoSeleccionCasilla (posicion, tablero, casilla);
		}
		public void PonerNombreAlObjetoDeJuego (string nombre) {
			this.casilla.name = nombre;
		}
		public Material PonerColor (string s) {
			Material mat = new Material (Shader.Find ("Standard"));
			if (s == "Claro") {
				mat = ColorClaro;
			}
			if (s == "Oscuro") {
				mat = ColorOscuro;
			}
			if (s == "Mov") {
				mat = ColorMovimiento;

			}
			casilla.GetComponent<MeshRenderer> ().material = mat;
			return mat;
		}
		public void PonerColor (Color c) {
			casilla.GetComponent<MeshRenderer> ().material.color = c;
		}
		public void PonerColor (Material mat) {
			casilla.GetComponent<MeshRenderer> ().material = mat;
		}
		void CrearMateriales () {
			ColorClaro = new Material (Shader.Find ("Standard"));
			ColorOscuro = new Material (Shader.Find ("Standard"));
			ColorMovimiento = new Material (Shader.Find ("Standard"));
			ColorMovimiento.color = Color.blue;
			ColorClaro.color = Color.grey;
			ColorOscuro.color = Color.black;
		}

		void CrearEfectoSeleccionCasilla (Vector3 pos, Transform tablero, GameObject casilla) {
			GameObject plano = GameObject.CreatePrimitive (PrimitiveType.Plane);
			Vector3 planopos = new Vector3 (pos.x, pos.y + 0.001f, pos.z);
			plano.transform.position = planopos;
			Material matcolor = new Material (Shader.Find ("Standard"));
			matcolor.color = Color.blue;
			plano.GetComponent<MeshRenderer> ().material = matcolor;
			float tam = 0.08f;
			plano.transform.localScale = new Vector3 (tam, tam, tam);
			plano.GetComponent<MeshCollider> ().enabled = false;
			GameObject efecto = new GameObject ("Efecto Seleccion");
			GameObject efectocomcont = new GameObject ("Efecto Comida");
			efecto.transform.parent = tablero;
			GameObject efectoComida = GameObject.Instantiate (plano);
			efectoComida.GetComponent<MeshRenderer> ().material.color = Color.red;
			efectoComida.transform.parent = efectocomcont.transform;
			plano.transform.parent = efecto.transform;
			efecto.transform.parent = casilla.transform;
			efectocomcont.transform.parent = casilla.transform;

			efectocomcont.SetActive (false);
			efecto.SetActive (false);
		}

	}
}

namespace AjedrezSupremo.Juego {
	public class Casilla : MonoBehaviour {

		public int id;
		public string nombre;
		[HideInInspector]
		public Vector3 posicion;
		public Crear.Casilla casilla;

		public bool CasillaMarcada;

		public bool Ocupada;
		public Pieza pieza;

		public bool Ocupable;

		public List<Pieza> AmenazadaPorClaros;
		public List<Pieza> AmenazadaPorOscuros;
		public List<Pieza> amenaza;

		public bool marcar = true;

		private void Start () {
			AmenazadaPorClaros = new List<Pieza> ();
			AmenazadaPorOscuros = new List<Pieza> ();

			amenaza = new List<Pieza> ();
		}

		void Update () {

		}
		public void QuitarAmenaza (Pieza pieza) {
			if (pieza.ColorPieza == color.Claro) {
				this.AmenazadaPorClaros.Remove (pieza);
				this.AmenazadaPorClaros = this.AmenazadaPorClaros.Distinct ().ToList ();
			}
			if (pieza.ColorPieza == color.Oscuro) {
				this.AmenazadaPorOscuros.Remove (pieza);
				this.AmenazadaPorOscuros = this.AmenazadaPorOscuros.Distinct ().ToList ();
			}
		}
		public void AgregarAmenaza(Pieza pieza)
		{
				if (pieza.ColorPieza == color.Claro) {
				this.AmenazadaPorClaros.Add (pieza);
				this.AmenazadaPorClaros = this.AmenazadaPorClaros.Distinct ().ToList ();
			}
			if (pieza.ColorPieza == color.Oscuro) {
				this.AmenazadaPorOscuros.Add (pieza);
				this.AmenazadaPorOscuros = this.AmenazadaPorOscuros.Distinct ().ToList ();
			}
		}
		public void Marcar()
		{
			AjedrezSupremo.Crear.Ajedrez.MarcarCasilla(this);
		}

	}
}