using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorManager : MonoBehaviour {

	public Texture2D fadeoutTexture;
	public float fadeSpeed=1f;

	private int drawDepth = -1000;
	private float alpha=1.0f;
	private int fadeDir = -1;

	public string mainmenuname;

	public GameObject[] panelLine;
	private bool[] panelState;

	// Use this for initialization
	void Start () {
		panelState = new bool[5];
		for (int i = 0; i < 5; i++) {
			panelState [i] = false;
		}
		panelLine [0].SetActive (true);
		panelState [0] = true;
	}

	public void ShowHidePanel(int whichpanel){
		if (!panelState [whichpanel]) {
			panelLine [whichpanel].SetActive (true);
			panelState [whichpanel] = true;
		} else {
			panelLine [whichpanel].SetActive (false);
			panelState [whichpanel] = false;
		}
	}

	IEnumerator WaitFunction(){
		float fadeTime = BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
	}

	void OnGUI(){
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeoutTexture);
	}
	public float BeginFade(int direction){
		fadeDir = direction;
		return (fadeSpeed);
	}
	void OnLevelWasLoaded(){
		BeginFade (-1);
	}

	public void BackToMenu(){
		WaitFunction ();
		SceneManager.LoadScene (mainmenuname);
	}
}
