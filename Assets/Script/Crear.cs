using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AjedrezSupremo.Crear;
using UnityEditor;
using UnityEngine;

namespace AjedrezSupremo.Crear {

	public class Pieza {

		public bool movible;
		public GameObject PiezaPrincipal;
		Material ColorClaro;
		Material ColorOscuro;
		private const float tamano = 0.2f;

		

		public Juego.Pieza CrearPieza (string NombreDePieza, Vector3 pos) {
			GameObject pieza = new GameObject ();
			GameObject piezaGO = Resources.Load (NombreDePieza) as GameObject;
			pieza.name = NombreDePieza;
			piezaGO = GameObject.Instantiate (piezaGO);
			piezaGO.transform.SetParent (pieza.transform);
			piezaGO.transform.position = Vector3.zero;
			//piezaGO.AddComponent<MeshCollider> ();
			//Crear mcollider - tarda mucho por pieza
			//piezaGO.GetComponent<MeshCollider> ().cookingOptions = MeshColliderCookingOptions.InflateConvexMesh;
			//piezaGO.GetComponent<MeshCollider> ().convex = true;
			pieza.AddComponent<CapsuleCollider> ();
			pieza.GetComponent<CapsuleCollider> ().radius = 0.32f;
			Vector3 centro = new Vector3 (0, 0.69f, 0);
			pieza.GetComponent<CapsuleCollider> ().center = centro;
			pieza.GetComponent<CapsuleCollider> ().isTrigger = true;

			//Tamaño
			Vector3 tamano = new Vector3 (Pieza.tamano, Pieza.tamano, Pieza.tamano);
			piezaGO.transform.localScale = tamano;
			pieza.transform.position = pos;

			if(NombreDePieza == "Rey")
			{				
				pieza.AddComponent<AjedrezSupremo.Juego.Rey>();
			}	
			else if(NombreDePieza == "Peon")
			{			
				pieza.AddComponent<AjedrezSupremo.Juego.Peon>();
			}
			else if(NombreDePieza == "Torre")
			{			
				pieza.AddComponent<AjedrezSupremo.Juego.Torre>();
			}
			else if(NombreDePieza == "Alfil")
			{			
				pieza.AddComponent<AjedrezSupremo.Juego.Alfil>();
			}
			else if(NombreDePieza == "Caballo")
			{			
				pieza.AddComponent<AjedrezSupremo.Juego.Caballo>();
			}
			else if(NombreDePieza == "Reina")
			{			
				pieza.AddComponent<AjedrezSupremo.Juego.Reina>();
			}
			
			PiezaPrincipal = pieza;

			return pieza.GetComponent<Juego.Pieza> ();
		}
		public void PonerColor (string s) {
			Materiales ();
			if (s == "Claro") {
				PiezaPrincipal.GetComponentInChildren<MeshRenderer> ().material = ColorClaro;
				PiezaPrincipal.GetComponent<Juego.Pieza> ().ColorPieza = Juego.color.Claro;
			}
			if (s == "Oscuro") {
				PiezaPrincipal.GetComponentInChildren<MeshRenderer> ().material = ColorOscuro;
				PiezaPrincipal.GetComponent<Juego.Pieza> ().ColorPieza = Juego.color.Oscuro;
			}

		}
		void Materiales () {
			ColorClaro = new Material (Shader.Find ("Standard"));
			ColorOscuro = new Material (Shader.Find ("Standard"));
			ColorClaro.color = Color.white;
			ColorOscuro.color = Color.black;
		}

	}
}