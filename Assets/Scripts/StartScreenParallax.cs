using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenParallax : MonoBehaviour {
	public Transform[] backgrounds;
	private float[] parallaxscales;
	public float smoothing;
	//public float smoothingrot;

	private Vector2 mousepos;
	private Vector2 scrwh;
	private Vector2 parallax;
	//private Vector2 parallaxrotation;

	void Start () {
		scrwh.x = Screen.width;
		scrwh.y = Screen.height;
		mousepos = Input.mousePosition;


		parallaxscales = new float[backgrounds.Length];
		for (int i = 0; i < parallaxscales.Length; i++) {
			parallaxscales [i] = backgrounds [i].position.z * -1;
		}

	}

	void Update(){
		

	}

	void LateUpdate () {
		
		for (int i = 0; i < backgrounds.Length; i++) {
			parallax.x = ((2 * mousepos.x - scrwh.x) / scrwh.x) * (parallaxscales [i] / smoothing);
			parallax.y = ((2 * mousepos.y - scrwh.y) / scrwh.y) * (parallaxscales [i] / smoothing);
			//parallaxrotation.x = (mousepos.x / scrwh.x * smoothingrot * 2) - smoothingrot;
			//parallaxrotation.y = -1*((mousepos.y / scrwh.y * smoothingrot * 2) - smoothingrot);
			backgrounds [i].position = new Vector3 (parallax.x, parallax.y, backgrounds[i].position.z);
			//backgrounds [i].eulerAngles = new Vector3 (parallaxrotation.x, parallaxrotation.y, backgrounds[i].eulerAngles.z);
		}
		mousepos = Input.mousePosition;

	}
}
