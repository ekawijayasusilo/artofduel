using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour {
	public Texture2D fadeoutTexture;
	public float fadeSpeed=0.8f;

	private int drawDepth = -1000;
	private float alpha=1.0f;
	private int fadeDir = -1;

	[HideInInspector] public float timer=0;
	public Text timertext;
	[HideInInspector] public int score1=0;
	[HideInInspector] public int score2=0;
	[HideInInspector] public int knifep1 = 0;
	[HideInInspector] public int knifep2 = 0;

	public GameObject arrowleft;
	public GameObject arrowright;
	public Text scorep1;
	public Text scorep2;
	public Text knifep1text;
	public Text knifep2text;
	public Text winningtext;

	public GameObject pauseui;
	public GameObject gameoverui;
	public string mainmenuname;
	public string leftscene;
	public string rightscene;

	private ParallaxBG pbg;

	public GameObject scoresaver;
	private DataController dc;

	private float preventiontimer=0;//untuk menunggu load dr datasaver saat ganti scene

	public GameObject[] Revivespot;

	public Vector3[] leftspawn;
	public Vector3[] rightspawn;
	//[HideInInspector] public int forwhochangepos;

	[HideInInspector] public float mytimescale=1;

	[HideInInspector] public int whosesimonsays=0;

	private int loopbrpkali=0;
	private MonoBehaviour[] scriptComponents;
	public string simonsaysname;

	private PlayerController playerscript1;
	private PlayerController playerscript2;
	private Transform camtrans;
	private bool stopchangingtimescale = false;

	void Start () {
		pbg = GameObject.Find ("Main Camera").GetComponent<ParallaxBG> ();
		scriptComponents = GameObject.Find("Main Camera").GetComponents<MonoBehaviour>();
		if (GameObject.Find ("ScoreSaver") == null) {
			GameObject ScoreSaver = (GameObject)Instantiate (scoresaver, transform.position, Quaternion.identity);
			ScoreSaver.name = "ScoreSaver";
		}
		StartCoroutine ("WaitForInit");
	}

	void Update () {
		if (timer > 0) {
			timer -= Time.deltaTime*mytimescale;
		}
		preventiontimer += Time.deltaTime;
		timertext.text = "TIME : " + Mathf.Round (timer).ToString();
		if (timer <= 0 && !stopchangingtimescale) {//perlu ditambahi
			if (preventiontimer > 1f) {//awal=3
				SetWinningText ();
				Time.timeScale = 0;
				gameoverui.SetActive (true);
				stopchangingtimescale = true;
			}
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			PauseButtonFunction ();
		}

		if (loopbrpkali == 2) {
			SceneManager.SetActiveScene (SceneManager.GetSceneByName (simonsaysname));
			loopbrpkali += 1;
		}
		if (loopbrpkali == 1) {
			loopbrpkali += 1;
		}
		knifep1text.text = "KNIFE : " + knifep1.ToString ();
		knifep2text.text = "KNIFE : " + knifep2.ToString ();
	}

	IEnumerator WaitForInit(){
		yield return new WaitForSeconds (0.1f);
		dc = GameObject.Find ("ScoreSaver").GetComponent<DataController> ();
		timer = dc.timer;
		score1 = dc.p1score;
		score2 = dc.p2score;
		knifep1 = dc.p1knife;
		knifep2 = dc.p2knife;
		//whosesimonsays = dc.camstate;
		whosesimonsays = dc.forwho;
		playerscript1 = GameObject.Find ("Player1").GetComponent<PlayerController> ();
		playerscript2 = GameObject.Find ("Player2").GetComponent<PlayerController> ();
		camtrans = GameObject.Find ("Main Camera").transform;
		if (whosesimonsays == 1) {
			//kiri
			GameObject.Find ("Player1").transform.position = leftspawn [0];
			GameObject.Find ("Player2").transform.position = leftspawn [1];
			camtrans.position = new Vector3 (-19.5f, camtrans.position.y, camtrans.position.z);
		} else if (whosesimonsays == 2) {
			//kanan
			GameObject.Find ("Player2").transform.position = rightspawn [0];
			GameObject.Find ("Player1").transform.position = rightspawn [1];
			camtrans.position = new Vector3 (19.5f, camtrans.position.y, camtrans.position.z);
		}
		if (dc.isitanim) {
			mytimescale = 0;
			if (whosesimonsays == 1) {
				GameObject.Find ("Player2").SetActive (false);
				playerscript1.AnimateToTreasure (8.5f, 1f);
			} else if (whosesimonsays == 2) {
				GameObject.Find ("Player1").SetActive (false);
				playerscript2.AnimateToTreasure (-8.5f,-1f);
			}
		} else {
			mytimescale = 1;
			playerscript1.isanimated = false;
			playerscript2.isanimated = false;
		}
		timertext.text = "TIME : " + timer.ToString ();
		scorep1.text = "SCORE : " + score1.ToString ();
		scorep2.text = "SCORE : " + score2.ToString ();

		pbg.SetFocus (whosesimonsays);
		SetArrow ();
	}

	public IEnumerator ReturnSimon(bool result){
		if (result == true) {
			if (whosesimonsays == 1) {
				score1++;
				playerscript1.SpawnScoreUI ();
				//GameObject.Find ("Player1").GetComponent<PlayerController> ().callsimonsaysactivated = false;
				//trigger ui anim, copy data, start coroutine delay, change scene;
			} else if (whosesimonsays == 2) {
				score2++;
				playerscript2.SpawnScoreUI ();
				//GameObject.Find ("Player2").GetComponent<PlayerController> ().callsimonsaysactivated = false;
				//trigger ui anim, copy data, start coroutine delay, change scene;
			}
		}
		yield return new WaitForSeconds (1.5f);

		if (whosesimonsays == 1) {
			//SaveData (1, false);
			TransitionToCenterScene ("MapCenter", 1);
		} else if (whosesimonsays == 2) {
			//SaveData (2, false);
			TransitionToCenterScene ("MapCenter", 2);
		}
	}

	public void mgmSetFocus(int whogothit){
		pbg.SetFocus (whogothit);
		SetArrow ();
	}

	public void PauseButtonFunction(){
		if (timer > 0f) {
			Time.timeScale = 0;
			pauseui.SetActive (true);
			scriptComponents [2].enabled = true;
		}
	}
	public void ResumeButtonFunction(){
		if (timer > 0f) {
			Time.timeScale = 1;
			pauseui.SetActive (false);
			scriptComponents [2].enabled = false;
		}
	}
	public void RestartButtonFunction(){
		WaitFunction ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
	public void MainMenuButtonFunction(){
		Time.timeScale = 1;
		WaitFunction ();
		SceneManager.LoadScene (mainmenuname);
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

	public void SetRevival(int whorevived){//perlu diganti
		Vector3 newplace=new Vector3(0f,0f,0f);
		double maxdist=0;
		GameObject leftcamcoll = GameObject.Find ("CamLeft");
		GameObject rightcamcoll = GameObject.Find ("CamRight");
		if (whorevived == 1) {
			maxdist = 100;
			for (int i = 0; i < Revivespot.Length; i++) {
				if (Revivespot [i].transform.position.x < maxdist && Revivespot [i].transform.position.x > (leftcamcoll.transform.position.x+0.3f)) {
					maxdist = Revivespot [i].transform.position.x;
					newplace = Revivespot [i].transform.position;
				}
			}
			GameObject.Find ("Player1").GetComponent<PlayerController> ().RevivePlace (newplace);
		} else if (whorevived == 2) {
			maxdist = -100;
			for (int i = 0; i < Revivespot.Length; i++) {
				if (Revivespot [i].transform.position.x > maxdist && Revivespot [i].transform.position.x < (rightcamcoll.transform.position.x - 0.3f)) {
					maxdist = Revivespot [i].transform.position.x;
					newplace = Revivespot [i].transform.position;
				}
			}
			GameObject.Find ("Player2").GetComponent<PlayerController> ().RevivePlace (newplace);
		}
	}

	public void SetWinningText(){
		Animator anim1 = GameObject.Find ("Player1").GetComponent<Animator> ();
		Animator anim2 = GameObject.Find ("Player2").GetComponent<Animator> ();
		if (score1 > score2) {
			winningtext.text = "PLAYER 1 WINS!";
			anim1.SetTrigger ("Win");
			anim2.SetTrigger ("Lose");
		}else if(score1<score2){
			winningtext.text = "PLAYER 2 WINS!";
			anim2.SetTrigger ("Win");
			anim1.SetTrigger ("Lose");
		}else{
			winningtext.text = "DRAW";
			anim1.SetTrigger ("Lose");
			anim2.SetTrigger ("Lose");
		}
	}

	public void TrasitionToNextScene(string nextname, int forwho, bool isitanim){
		SaveData (forwho,isitanim);
		WaitFunction ();
		SceneManager.LoadScene (nextname);
	}
	public void TransitionToCenterScene(string nextname,int who){
		SaveData (0,false);
		WaitFunction ();
		SceneManager.LoadScene (nextname);
	}

	void SaveData(int forwho,bool isitanim){//bagaimana savedata untuk ke center
		dc.timer = timer;
		dc.p1score = score1;
		dc.p2score = score2;
		dc.camstate = whosesimonsays;
		dc.forwho = forwho;
		dc.isitanim = isitanim;
		if (forwho == 0) {
			dc.p1knife = 3;
			dc.p2knife = 3;
		} else {
			dc.p1knife = knifep1;
			dc.p2knife = knifep2;
		}
	}
	void SetArrow(){
		if (whosesimonsays == 0) {
			arrowleft.SetActive (false);
			arrowright.SetActive (false);
			playerscript1.movespeed = 6;
			playerscript2.movespeed = 6;
		} else if (whosesimonsays == 1) {
			arrowleft.SetActive (false);
			arrowright.SetActive (true);
			playerscript1.movespeed = 6;
			playerscript2.movespeed = 8;
		}else if (whosesimonsays == 2) {
			arrowleft.SetActive (true);
			arrowright.SetActive (false);
			playerscript1.movespeed = 8;
			playerscript2.movespeed = 6;
		}
	}
	void DisableSimonSaysArrow(){
		if (whosesimonsays == 1) {
			arrowright.SetActive (false);
		} else if (whosesimonsays == 2) {
			arrowleft.SetActive (false);
		}
	}

	public void CallSimonSays(){
		loopbrpkali = 1;
		DisableSimonSaysArrow ();
		SceneManager.LoadScene (simonsaysname, LoadSceneMode.Additive);
	}

	public int GetKnife(bool whichplayer){
		return !whichplayer ? knifep1 : knifep2;
	}

	public void SetKnife(bool whichplayer, int addknife, bool additive=true){
		if (!additive) {
			if (!whichplayer) {
				knifep1 = addknife;
			} else {
				knifep2 = addknife;
			}
		} else {
			if (!whichplayer) {
				knifep1 += addknife;
			} else {
				knifep2 += addknife;
			}
		}
	}
}
