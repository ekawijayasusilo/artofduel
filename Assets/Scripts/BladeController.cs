using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeController : MonoBehaviour {

	public float bladespeed;

	private Rigidbody2D rb;

	public GameObject bladeeffect;
	private bool destroyed;

	[HideInInspector] public int owner;
	private PlayerController pc1;
	private PlayerController pc2;
	private Animator animp1;
	private Animator animp2;
	private Animator anim;

	private BoxCollider2D ct;
	private BoxCollider2D cb;
	private BoxCollider2D cl;
	private BoxCollider2D cr;
	private CapsuleCollider2D cc;

	public double pickradius;
	private int counter = 0;

	MainGameManager mgm;

	[HideInInspector] public bool state;

	void Start () {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		cl = GameObject.Find ("CamLeft").GetComponent<BoxCollider2D> ();
		cr = GameObject.Find ("CamRight").GetComponent<BoxCollider2D> ();
		cc = GetComponent<CapsuleCollider2D> ();
		pc1 = GameObject.Find ("Player1").GetComponent<PlayerController> ();
		pc2 = GameObject.Find ("Player2").GetComponent<PlayerController> ();
		animp1 = GameObject.Find ("Player1").GetComponent<Animator> ();
		animp2 = GameObject.Find ("Player2").GetComponent<Animator> ();
		state = true;
		destroyed = false;
		mgm = GameObject.Find ("Canvas").GetComponent<MainGameManager> ();
	}

	void Update () {
		if (destroyed) {
			Destroy (gameObject);
		}
		if (state) {
			rb.velocity = new Vector2 (bladespeed * transform.localScale.x * Time.timeScale, 0);
		} else {
			anim.SetBool ("grounded", true);
			if (counter == 0) {
				counter++;
				Physics2D.IgnoreCollision (cl, cc);
				Physics2D.IgnoreCollision (cr, cc);
			}
		}
		if (transform.position.y < -8) {
			destroyed = true;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (state) {
			if (other.gameObject.tag == "Player1") {
				if (!((animp1.GetCurrentAnimatorStateInfo (0).IsTag ("Parry") || animp1.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) && other.gameObject.transform.localScale.x != gameObject.transform.localScale.x)) {
					if (!pc1.invincible) {
						pc1.Hit ();
						mgm.whosesimonsays = 2;
						mgm.mgmSetFocus (2);
					}
				} else if (animp1.GetCurrentAnimatorStateInfo (0).IsTag ("Attack") && other.gameObject.transform.localScale.x != gameObject.transform.localScale.x) {
					pc1.Hurt ();
				}
				Instantiate (bladeeffect, transform.position, Quaternion.identity);
				destroyed = true;
			}
			if (other.gameObject.tag == "Player2") {
				if (!((animp2.GetCurrentAnimatorStateInfo (0).IsTag ("Parry") || animp2.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) && other.gameObject.transform.localScale.x != gameObject.transform.localScale.x)) {
					if (!pc2.invincible) {
						pc2.Hit ();
						mgm.whosesimonsays = 1;
						mgm.mgmSetFocus (1);
					}
				} else if (animp2.GetCurrentAnimatorStateInfo (0).IsTag ("Attack") && other.gameObject.transform.localScale.x != gameObject.transform.localScale.x) {
					pc2.Hurt ();
				}
				Instantiate (bladeeffect, transform.position, Quaternion.identity);
				destroyed = true;
			}
			rb.gravityScale = 1;
			rb.velocity = new Vector2 (0, rb.velocity.y);
			state = false;
		} else {
			if (other.gameObject.tag == "Player1") {
				mgm.SetKnife (false, 1);
				destroyed = true;
			}
			if (other.gameObject.tag == "Player2") {
				mgm.SetKnife (true, 1);
				destroyed = true;
			}
		}
	}
}
