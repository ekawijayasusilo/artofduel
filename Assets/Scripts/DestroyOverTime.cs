using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour {

	public float lifetime;
	//[HideInInspector] public GameManager gm;
	//[HideInInspector] public float mytimescale=1;

	// Use this for initialization
	void Start () {
		//gm = GameObject.Find ("Canvas").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		lifetime -= Time.deltaTime;//*mytimescale);

		if (lifetime < 0) {
			Destroy (gameObject);
		}
	}
}
