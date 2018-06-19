using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour {
	public int forwho;
	public string transname;
	public bool isfinish;
	public bool isitanim;
	private MainGameManager mgm;
	private ParallaxBG pbg;
	// Use this for initialization
	void Start () {
		mgm = GameObject.Find ("Canvas").GetComponent<MainGameManager> ();
		pbg = GameObject.Find ("Main Camera").GetComponent<ParallaxBG> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter2D(Collision2D other) {
		if (forwho == 1 && other.gameObject.tag=="Player1" && pbg.camstate==1) {
			if (!isfinish) {
				mgm.TrasitionToNextScene (transname,forwho,isitanim);
			} else if (isfinish) {
				mgm.TransitionToCenterScene (transname,1);
			}
		} else if (forwho == 2 && other.gameObject.tag=="Player2" && pbg.camstate==2) {
			if (!isfinish) {
				mgm.TrasitionToNextScene (transname,forwho,isitanim);
			} else if (isfinish) {
				mgm.TransitionToCenterScene (transname,2);
			}
		}
	}
}
