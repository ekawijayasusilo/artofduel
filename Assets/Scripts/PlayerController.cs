using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float movespeed;
	public float jumpforce;
	public float slideforce;
	public float dragfactor;

	public KeyCode jump;
	public KeyCode attack;
	public KeyCode throwblade;
	public KeyCode slide;
	public KeyCode parry;

	private Rigidbody2D rb;
	private Animator anim;
	private SpriteRenderer sr;

	public Transform gc;
	public Vector2 gcsize;
	public LayerMask whatisground;
	[HideInInspector] public bool isgrounded;
	private int jumpcount;

	public GameObject blade;
	public Transform throwbladepoint;

	private bool whoami;

	[HideInInspector] public PlayerController otherpc;

	[HideInInspector] public bool hurt;
	private float timerhurt;
	public Vector2 hurtforce;
	[HideInInspector] public bool invincible;
	private float timerinvincible;
	[HideInInspector] public bool delayattack;
	private float timerdelayattack;
	private float limitdelayattack;
	//[HideInInspector] public bool delayparry;
	//private float timerdelayparry;
	//private float limitdelayparry;
	[HideInInspector] public bool delayslide;
	private float timerdelayslide;
	private float limitdelayslide;//limit=ketentuan lama delay, timer adalah counternya

	[HideInInspector] public bool dead;
	private float timerdead;

	[HideInInspector] public bool state;
	[HideInInspector] public float mytimescale;

	private MainGameManager mgm;

	[HideInInspector] public float playerdestination;
	[HideInInspector] public float playerfirstpoint;
	[HideInInspector] public bool isanimated;
	[HideInInspector] public bool callsimonsaysactivated;
	[HideInInspector] public bool reachedfirstpoint;

	public GameObject scoreui;

	void Start () {
		if (gameObject.tag == "Player1") {//player1 axis control
			whoami = false;
			otherpc = GameObject.Find ("Player2").GetComponent<PlayerController> ();
		} else {//player2 axis control
			whoami = true;
			otherpc = GameObject.Find ("Player1").GetComponent<PlayerController> ();
		}
		mytimescale = 1;
		jumpcount = 1;
		rb = gameObject.GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		mgm = GameObject.Find ("Canvas").GetComponent<MainGameManager> ();
		invincible = false;
		timerinvincible = 0f;
		limitdelayattack = 1f;
		//limitdelayparry = 0.5f;
		limitdelayslide = 2.5f;
		delayattack = false;
		//delayparry = false;
		delayslide = false;

		isanimated = false;
		callsimonsaysactivated = false;
		reachedfirstpoint = false;
	}


	//-----------------------------------------END UPDATE----------------------------------------------//
	void Update () {
		isgrounded = Physics2D.OverlapBox (gc.position, gcsize, 90, whatisground);
		if (isgrounded) {
			jumpcount = 1;
		}

		//*********************************if isanimated or not**************************************//
		if (!isanimated) {
			if (!whoami) {//player1 control axis
				if (Input.GetAxisRaw ("p1Horizontal") == -1 && CheckAnimState (1, 1, 1, 0, 0, 1, 0, 0, 0)) {
					anim.SetBool ("Run", true);
					rb.velocity = new Vector2 (-movespeed * mytimescale, rb.velocity.y * mytimescale);
				} else if (Input.GetAxisRaw ("p1Horizontal") == 1 && CheckAnimState (1, 1, 1, 0, 0, 1, 0, 0, 0)) {
					anim.SetBool ("Run", true);
					rb.velocity = new Vector2 (movespeed * mytimescale, rb.velocity.y * mytimescale);
				} else {
					anim.SetBool ("Run", false);
					if (CheckAnimState (1, 1, 0, 1, 1, 1, 1, 1, 1)) {
						rb.velocity = new Vector2 (rb.velocity.x * dragfactor, rb.velocity.y);
					}

				}
			} else {//player2 control axis
				if (Input.GetAxisRaw ("p2Horizontal") == -1 && CheckAnimState (1, 1, 1, 0, 0, 1, 0, 0, 0)) {
					anim.SetBool ("Run", true);
					rb.velocity = new Vector2 (-movespeed * mytimescale, rb.velocity.y * mytimescale);
				} else if (Input.GetAxisRaw ("p2Horizontal") == 1 && CheckAnimState (1, 1, 1, 0, 0, 1, 0, 0, 0)) {
					anim.SetBool ("Run", true);
					rb.velocity = new Vector2 (movespeed * mytimescale, rb.velocity.y * mytimescale);
				} else {
					anim.SetBool ("Run", false);
					if (CheckAnimState (1, 1, 0, 1, 1, 1, 1, 1, 1)) {
						rb.velocity = new Vector2 (rb.velocity.x * dragfactor, rb.velocity.y);
					}

				}
			}

			if (rb.velocity.x < 0) {
				if (CheckAnimState (0, 1, 1, 0, 0, 1, 0, 0, 0)) {
					transform.localScale = new Vector3 (-1, 1, 1);
				} else if(anim.GetCurrentAnimatorStateInfo (0).IsTag ("Hurt")) {
					transform.localScale = new Vector3 (1, 1, 1);
				}
			} else if (rb.velocity.x > 0) {
				if (CheckAnimState (0, 1, 1, 0, 0, 1, 0, 0, 0)) {
					transform.localScale = new Vector3 (1, 1, 1);
				} else if(anim.GetCurrentAnimatorStateInfo (0).IsTag ("Hurt")){
					transform.localScale = new Vector3 (-1, 1, 1);
				}
			}

			if (Input.GetKeyDown (jump) && jumpcount > 0 && CheckAnimState (1, 1, 1, 0, 1, 0, 0, 0, 1)) {
				rb.velocity = new Vector2 (rb.velocity.x * mytimescale, jumpforce * mytimescale);
				jumpcount--;
			}
			if (Input.GetKeyDown (attack) && !delayattack && CheckAnimState (1, 1, 1, 0, 0, 0, 0, 0, 0)) {
				anim.SetTrigger ("Attack");
				delayattack = true;
				timerdelayattack = 0f;
			}
				
			if (Input.GetKeyUp (parry)) {
				anim.SetBool ("HoldParry", false);
				/*
				if (!delayparry) {
					delayparry = true;
					timerdelayparry = 0f;
				}
				*/
			}
			if (Input.GetKeyDown (parry) && CheckAnimState (1, 1, 1, 0, 0, 0, 0, 0, 0)) {
				anim.SetBool ("HoldParry", true);
			}


			if (Input.GetKeyDown (slide) && !delayslide && CheckAnimState (0, 1, 1, 0, 0, 0, 0, 0, 0)) {
				anim.SetTrigger ("Slide");
				rb.velocity = new Vector2 (transform.localScale.x * slideforce * mytimescale, rb.velocity.y * mytimescale);
				delayslide = true;
				timerdelayslide = 0f;
			}
				
			if (Input.GetKeyDown (throwblade) && CheckAnimState (1, 1, 1, 0, 0, 0, 0, 0, 0)) {
				if (mgm.GetKnife(whoami) > 0) {
					Vector3 blrottemp = transform.rotation.eulerAngles;
					if (transform.localScale.x == 1) {
						blrottemp.z = -90f;
					} else if (transform.localScale.x == -1) {
						blrottemp.z = 90f;
					}
					GameObject bladetemp = (GameObject)Instantiate (blade, throwbladepoint.position, Quaternion.Euler (blrottemp));
					bladetemp.transform.localScale = transform.localScale;
					BladeController bladecon = bladetemp.GetComponent<BladeController> ();
					if (!whoami) {
						bladecon.owner = 1;
					} else if (whoami) {
						bladecon.owner = 2;
					}
					anim.SetTrigger ("Throw");
					mgm.SetKnife (whoami, -1);
				}
			}
				
			if (hurt) {
				timerhurt += (Time.deltaTime * mytimescale);
				if (sr.color.a == 1.0f) {
					sr.color -= new Color (0, 0, 0, 0.7f);
				} else if (sr.color.a == 0.3f) {
					sr.color += new Color (0, 0, 0, 0.7f);
				}
				if (timerhurt >= 2.0f) {
					hurt = false;
					if (sr.color.a != 1.0f) {
						sr.color += new Color (0, 0, 0, 0.7f);
					}
				}
			}

			if (dead) {
				timerdead += (Time.deltaTime * mytimescale);
				if (timerdead >= 0.5f) {
					if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Idle")) {
						dead = false;
						mgm.SetKnife (whoami, 3, false);
						if (!whoami) {
							mgm.SetRevival (1);
						} else {
							mgm.SetRevival (2);
						}
							
						hurt = true;
						timerhurt = 0f;
					}
				}
			}
				
			if (invincible) {
				timerinvincible += (Time.deltaTime * mytimescale);
				if (timerinvincible >= 2.5f) {
					invincible = false;
				}
			}

			if (delayattack) {
				timerdelayattack += (Time.deltaTime * mytimescale);
				if (timerdelayattack >= limitdelayattack) {
					delayattack = false;
				}
			}
			/*
			if (delayparry) {
				timerdelayparry += (Time.deltaTime * mytimescale);
				if (timerdelayparry >= limitdelayparry) {
					delayparry = false;
				}
			}
			*/
			if (delayslide) {
				timerdelayslide += (Time.deltaTime * mytimescale);
				if (timerdelayslide >= limitdelayslide) {
					delayslide = false;
				}
			}

			if (transform.position.y <= -9f && !invincible) {
				Hit ();
				if (!whoami) {
					mgm.whosesimonsays = 2;
					mgm.mgmSetFocus (2);
				} else if (whoami) {
					mgm.whosesimonsays = 1;
					mgm.mgmSetFocus (1);
				}
			}


		} else {//elsenya ifanimated

			if (!whoami) {
				if (transform.position.x < playerdestination) {
					anim.SetBool ("Run", true);
					rb.velocity = new Vector2 (movespeed * mytimescale, rb.velocity.y * mytimescale);
					if (transform.position.x >= playerfirstpoint && !reachedfirstpoint ) {
						mgm.score1++;
						SpawnScoreUI ();
						reachedfirstpoint = true;
					}
				} else if(callsimonsaysactivated==false && transform.position.x >= playerdestination) {
					anim.SetBool ("Run", false);
					rb.velocity = new Vector2 (rb.velocity.x * dragfactor, rb.velocity.y);
					mgm.CallSimonSays ();
					callsimonsaysactivated = true;
				}
			} else {
				if (transform.position.x > playerdestination) {
					anim.SetBool ("Run", true);
					rb.velocity = new Vector2 (-movespeed * mytimescale, rb.velocity.y * mytimescale);
					if (transform.position.x <= playerfirstpoint && !reachedfirstpoint ) {
						mgm.score2++;
						SpawnScoreUI ();
						reachedfirstpoint = true;
					}
				} else if(callsimonsaysactivated==false && transform.position.x <= playerdestination){
					anim.SetBool ("Run", false);
					rb.velocity = new Vector2 (rb.velocity.x * dragfactor, rb.velocity.y);
					mgm.CallSimonSays ();
					callsimonsaysactivated = true;
				}
			}


		}
		//*****************************************END if isanimated or not************************************************//

		anim.SetBool ("Grounded",isgrounded);
	}
	//-----------------------------------------END UPDATE----------------------------------------------//

	//#######################################################FUNCTION AREA############################################################//

	public void SpawnScoreUI(){
		Vector3 tscoreui = transform.position;
		tscoreui.y += 1.25f;
		GameObject scoreuitemp = (GameObject)Instantiate (scoreui, tscoreui, Quaternion.identity);
	}

	public void RevivePlace(Vector3 place){
		transform.position = place;
		dead = false;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (!whoami) {
			if (other.gameObject.tag == "Player2" && other.gameObject.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsTag ("Attack") && !anim.GetCurrentAnimatorStateInfo (0).IsTag ("Parry")) {
				if (!invincible) {
					//if ((other.gameObject.transform.localScale.x >0 && transform.position.x+0.35f > other.gameObject.transform.localScale.x) || (other.gameObject.transform.localScale.x < 0 && transform.position.x-0.35f<other.gameObject.transform.localScale.x)) {
						Hit ();
						mgm.whosesimonsays = 2;
						mgm.mgmSetFocus (2);
					//} else {
					//	Debug.Log ("kokbisamasuksini1");
					//}
				}

			} else if ((other.gameObject.tag == "Player2") && other.gameObject.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsTag ("Attack") && anim.GetCurrentAnimatorStateInfo (0).IsTag ("Parry")) {
				//if ((other.gameObject.transform.localScale.x > 0 && transform.position.x+0.35f > other.gameObject.transform.localScale.x) || (other.gameObject.transform.localScale.x < 0 && transform.position.x-0.35f < other.gameObject.transform.localScale.x)) {
					if (other.gameObject.transform.localScale.x != transform.localScale.x) {
						otherpc.Hurt ();
					} else {
						Hit ();
						mgm.whosesimonsays = 2;
						mgm.mgmSetFocus (2);
					}
				//}
			}
		}
		if (whoami) {
			if (other.gameObject.tag == "Player1" && other.gameObject.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsTag ("Attack") && !anim.GetCurrentAnimatorStateInfo (0).IsTag ("Parry")) {
				if (!invincible) {
					//if ((other.gameObject.transform.localScale.x > 0 && transform.position.x+0.35f > other.gameObject.transform.localScale.x) || (other.gameObject.transform.localScale.x < 0 && transform.position.x-0.35f < other.gameObject.transform.localScale.x)) {
						Hit ();
						mgm.whosesimonsays = 1;
						mgm.mgmSetFocus (1);
					//} else {
					//	Debug.Log ("kokbisamasuksini2");
					//}
				}
			} else if ((other.gameObject.tag == "Player1") && other.gameObject.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsTag ("Attack") && anim.GetCurrentAnimatorStateInfo (0).IsTag ("Parry")) {
				//if ((other.gameObject.transform.localScale.x > 0 && transform.position.x+0.35f > other.gameObject.transform.localScale.x) || (other.gameObject.transform.localScale.x < 0 && transform.position.x-0.35f < other.gameObject.transform.localScale.x)) {
					if (other.gameObject.transform.localScale.x != transform.localScale.x) {
						otherpc.Hurt ();
					} else {
						Hit ();
						mgm.whosesimonsays = 1;
						mgm.mgmSetFocus (1);
					}
				//}
			}
		}
		if (other.gameObject.tag == "CameraCollider") {
			if (GameObject.Find("Main Camera").GetComponent<ParallaxBG>().camstate != 0) {
				if (!invincible) {
					Hit ();
					if (!whoami) {
						mgm.whosesimonsays = 2;
						mgm.mgmSetFocus (2);
					} else if (whoami) {
						mgm.whosesimonsays = 1;
						mgm.mgmSetFocus (1);
					}
				}
			}
		}
	}

	public void Hit(){
		dead = true;
		timerdead = 0f;
		anim.SetTrigger ("Dead");
		invincible = true;
		timerinvincible = 0f;
		rb.velocity = new Vector2 (0, 0);
	}

	public void Hurt(){
		if (hurt == false) {
			anim.SetTrigger ("Hurt");
			hurt = true;
			timerhurt = 0f;
			hurtforce = new Vector2 (hurtforce.x * transform.localScale.x*-1, hurtforce.y);
			rb.AddForce (hurtforce, ForceMode2D.Impulse);
		}
	}

	bool CheckAnimState(int idle, int run, int jump, int slide, int pthrow, int attack, int parry, int dead, int hurt){
		if (idle==1 && anim.GetCurrentAnimatorStateInfo (0).IsTag ("Idle")) {
			return true;
		}
		if (run==1 && anim.GetCurrentAnimatorStateInfo (0).IsTag ("Run")) {
			return true;
		}
		if (jump==1 && anim.GetCurrentAnimatorStateInfo (0).IsTag ("Jump")) {
			return true;
		}
		if (slide==1 && anim.GetCurrentAnimatorStateInfo (0).IsTag ("Slide")) {
			return true;
		}
		if (pthrow==1 && anim.GetCurrentAnimatorStateInfo (0).IsTag ("Throw")) {
			return true;
		}
		if (attack==1 && anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) {
			return true;
		}
		if (parry==1 && anim.GetCurrentAnimatorStateInfo (0).IsTag ("Parry")) {
			return true;
		}
		if (dead==1 && anim.GetCurrentAnimatorStateInfo (0).IsTag ("Dead")) {
			return true;
		}
		if (hurt==1 && anim.GetCurrentAnimatorStateInfo (0).IsTag ("Hurt")) {
			return true;
		}
		return false;
	}

	public void AnimateToTreasure(float destination, float firstpoint){
		isanimated = true;
		playerdestination = destination;
		playerfirstpoint = firstpoint;
	}
}
