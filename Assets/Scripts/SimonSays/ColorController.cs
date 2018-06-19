using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour {

	private SpriteRenderer sr;
	private GameManager gm;
	public int colornumber;

	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer> ();
		gm = GameObject.Find ("Colors").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){
		if (!gm.isStarted && gm.startbuttonclicked) {
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
		}
	}
	void OnMouseUp(){
		if (!gm.isStarted && gm.startbuttonclicked) {
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0.4f);
			gm.ColorPressed (colornumber);
		}
	}
}
