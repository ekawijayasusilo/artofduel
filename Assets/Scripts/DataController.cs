using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour {

	[HideInInspector] public int p1score;
	[HideInInspector] public int p2score;
	[HideInInspector] public float timer;
	[HideInInspector] public int camstate;
	[HideInInspector] public int forwho;
	[HideInInspector] public bool isitanim;
	[HideInInspector] public int p1knife;
	[HideInInspector] public int p2knife;

	// Use this for initialization
	void Start () {
		p1score = 0;
		p2score = 0;
		p1knife = 3;
		p2knife = 3;
		timer = 420f;
		camstate = 0;
		forwho = 0;
		isitanim = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Awake ()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	void OnLevelWasLoaded (int whatscene)
	{
		if(whatscene==0)//load menu, maka destroy
		{
			Destroy(this.gameObject);
		}
	}

	public void SetData(int p1s, int p2s, float timerleft, int camst){
		p1score = p1s;
		p2score = p2s;
		timer = timerleft;
		camstate = camst;
	}
}
