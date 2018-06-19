using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
	public Texture2D fadeoutTexture;
	public float fadeSpeed=0.8f;

	private int drawDepth = -1000;
	private float alpha=1.0f;
	private int fadeDir = -1;

	public string maingame;
	public string tutorialname;

	public void StartGame(){
		WaitFunction ();
		SceneManager.LoadScene (maingame);
	}
	IEnumerator WaitFunction(){
		float fadeTime = BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
	}
	public void QuitGame(){
		Application.Quit ();
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

	public void LoadTutorial(){
		WaitFunction ();
		SceneManager.LoadScene (tutorialname);
	}
}
