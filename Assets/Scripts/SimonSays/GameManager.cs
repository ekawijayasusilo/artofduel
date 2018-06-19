using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public SpriteRenderer[] colors;

	private int colorselect;

	public float littime;
	private float litcounter;
	private float waitcounter;
	public float waittime;

	[HideInInspector]public bool isStarted;
	private bool isLit;
	private bool noLit;

	[HideInInspector] public List<int> activeSequence;
	private int posInSequence;
	private int inputInSequence;

	public int numSequence;
	private int countdown;
	private bool result;

	public Text anytext;
	public GameObject startbutton;
	private MainGameManager mgm;
	[HideInInspector] public float delaybetweentranstion=1f;
	private DataController dcc;

	//public int whichscene;
	public string whichscenetoload;

	public string simonsaysname;
	public int whoownthis;

	[HideInInspector] public bool startbuttonclicked=false;

	// Use this for initialization
	void Start () {
		mgm = GameObject.FindGameObjectWithTag ("cvs").GetComponent<MainGameManager> ();
		for (int i = 0; i < numSequence; i++) {
			activeSequence.Add (0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isLit) {
			litcounter -= Time.deltaTime;
			if (litcounter < 0) {
				Color tempcolor = colors [colorselect].color;
				colors [colorselect].color = new Color (tempcolor.r, tempcolor.g, tempcolor.b, 0.4f);
				isLit = false;
				posInSequence++;
				if (posInSequence < activeSequence.Count) {
					colorselect = activeSequence [posInSequence];
				}

				noLit = true;
				waitcounter = waittime;
			}
		}
		if (noLit) {
			waitcounter -= Time.deltaTime;

			if (posInSequence >= activeSequence.Count) {
				noLit = false;
				isStarted = false;
				anytext.text = "Recording Your Input";
			} else {
				if (waitcounter < 0) {
					noLit = false;
					Color tempcolor = colors [colorselect].color;
					colors [colorselect].color = new Color (tempcolor.r, tempcolor.g, tempcolor.b, 1f);
					countdown--;
					anytext.text = countdown + " Sequences Left";
					litcounter = littime;
					isLit = true;
				}
			}
		}

	}

	public void StartGame(){
		if (isStarted == false) {
			startbuttonclicked = true;
			startbutton.SetActive (false);
			countdown = numSequence;
			anytext.text = countdown + " Sequences Left";
			isStarted = true;
			posInSequence = 0;
			inputInSequence = 0;
			for (int i = 0; i < numSequence; i++) {
				activeSequence [i] = Random.Range (0, colors.Length);
			}
			colorselect = activeSequence [posInSequence];
			Color tempcolor = colors [colorselect].color;
			colors [colorselect].color = new Color (tempcolor.r, tempcolor.g, tempcolor.b, 1f);

			countdown--;
			anytext.text = countdown + " Sequences Left";

			litcounter = littime;
			isLit = true;
		}
	}

	public void ColorPressed(int whichButton){
		if (activeSequence[inputInSequence] == whichButton) {
			//Debug.Log ("C");
			inputInSequence++;
			if (inputInSequence >= activeSequence.Count) {
				anytext.text = "Door Unlocked";
				result = true;
				isStarted = true;
				StartCoroutine ("TransitionNextQuestion");
				//passing parameter benar ke mainscene
			}
		} else {
			//Debug.Log ("W");
			anytext.text = "Password Incorrect";
			result = false;
			isStarted = true;
			StartCoroutine ("TransitionNextQuestion");
			//passing paraeter gagal ke mainscene
		}

	}

	IEnumerator TransitionNextQuestion(){
		yield return new WaitForSeconds (delaybetweentranstion);
		SceneManager.SetActiveScene (SceneManager.GetSceneByName (whichscenetoload));//yang bagian ini bagaimana?
		mgm.StartCoroutine("ReturnSimon",result);
		SceneManager.UnloadScene(SceneManager.GetSceneByName(simonsaysname));
	}
}
