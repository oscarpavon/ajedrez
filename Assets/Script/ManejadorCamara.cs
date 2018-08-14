using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManejadorCamara : MonoBehaviour {

		public Transform pivot;
		float mouseX;
		float mouseY;
		float smoothX;
		float smoothY;
		float smoothXvelocity;
		float smoothYvelocity;
		float lookAngle;
		float tilAngle;

		public ValoresDeCamara valores;


	// Use this for initialization
	void Start () {
		ManejarRotacion();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(1))
		{
			ManejarRotacion();
		}
		
	}

	void ManejarRotacion()
	{
		
			mouseX = Input.GetAxis ("Mouse X");
			mouseY = Input.GetAxis ("Mouse Y");
			if (valores.turnSmooth > 0) {
				smoothX = Mathf.SmoothDamp (smoothX, mouseX, ref smoothXvelocity, valores.turnSmooth);
				smoothY = Mathf.SmoothDamp (smoothY, mouseY, ref smoothYvelocity, valores.turnSmooth);
			} else {
				smoothX = mouseX;
				smoothY = mouseY;

			}
			lookAngle += smoothX * valores.y_rotate_speed;
			Quaternion targetRot = Quaternion.Euler (0, lookAngle, 0);
			this.transform.rotation = targetRot;

			tilAngle -= smoothY * valores.x_rotate_speed;
			tilAngle = Mathf.Clamp (tilAngle, valores.minAngle, valores.maxAngle);
			pivot.localRotation = Quaternion.Euler (tilAngle, 0, 0);


	}
}
