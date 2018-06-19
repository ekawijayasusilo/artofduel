using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour {

	public Transform[] backgrounds;
	private float[] parallaxscales;
	public float smoothing;

	private Vector3 prevcamerapos;

	[HideInInspector] public int camstate=0;


	public Transform pf;


	private Vector3 temp;
	private float gapping;
	private Vector3 gappingtemp;
	private float cheight;
	private float cwidth;
	private float distx;
	private float limitdistx;

	/*--------------------------Dari Standard Asset---------------------*/
	[SerializeField] private float m_MoveSpeed = 3; // How fast the rig will move to keep up with target's position

	private Vector3 lerptemp;
	private float m_LastFlatAngle; // The relative angle of the target and the rig from the previous frame.
	private float m_CurrentTurnAmount; // How much to turn the camera
	private float m_TurnSpeedVelocityChange; // The change in the turn speed velocity
	/*----------------------End Standard Asset---------------------*/
	void Start () {
		prevcamerapos = transform.position;

		parallaxscales = new float[backgrounds.Length];
		for (int i = 0; i < parallaxscales.Length; i++) {
			parallaxscales [i] = backgrounds [i].position.z * -1;
		}
		if (camstate == 0) {
			pf = gameObject.transform;
		}
		cheight = 2 * Camera.main.orthographicSize;
		cwidth = cheight * Camera.main.aspect;
		limitdistx = 0.025f * cwidth;
	}

	void Update(){


	}

	void LateUpdate () {
		FollowTarget (Time.deltaTime);
		for (int i = 0; i < backgrounds.Length; i++) {
			Vector3 parallax = (prevcamerapos - transform.position) * (parallaxscales [i] / smoothing);
			backgrounds [i].position = new Vector3 (backgrounds[i].position.x+parallax.x, backgrounds[i].position.y+parallax.y, backgrounds[i].position.z);
		}
		prevcamerapos = transform.position;
	}

	public void SetFocus(int camstatenow){
		camstate = camstatenow;
		if (camstate == 1) {
			pf = GameObject.Find ("Player1").GetComponent<Transform> ();
			gapping = 0.15f * cwidth;
		} else if (camstate == 2) {
			pf = GameObject.Find ("Player2").GetComponent<Transform> ();
			gapping = -(0.15f * cwidth);
		}
	}

	public void FollowTarget(float deltaTime){
		gappingtemp = pf.position;
		if (camstate != 0) {
			gappingtemp = new Vector3 (gappingtemp.x + gapping, gappingtemp.y, gappingtemp.z);
		}
		if (!(transform.position.x >= -19.5f && transform.position.x <= 19.5f && transform.position.y >= -3f && transform.position.y <= 3f)) {
			if (gappingtemp.x < -19.5f) {
				gappingtemp = new Vector3 (-19.5f, gappingtemp.y, gappingtemp.z);
			} else if (gappingtemp.x > 19.5f) {
				gappingtemp = new Vector3 (19.5f, gappingtemp.y, gappingtemp.z);
			}

			if (gappingtemp.y < -3f) {
				gappingtemp = new Vector3 (gappingtemp.x, -3f, gappingtemp.z);
			} else if (gappingtemp.y > 3f) {
				gappingtemp = new Vector3 (gappingtemp.x, 3f, gappingtemp.z);
			}
		}
		distx = Mathf.Abs (gappingtemp.x - transform.position.x);
		if (distx > limitdistx) {
			lerptemp = Vector3.Lerp (transform.position, gappingtemp, deltaTime * m_MoveSpeed);
			transform.position = new Vector3 (lerptemp.x, lerptemp.y, transform.position.z);
		} else {
			lerptemp = Vector3.Lerp (transform.position, gappingtemp, deltaTime * m_MoveSpeed);
			transform.position = new Vector3 (transform.position.x, lerptemp.y, transform.position.z);
		}
	}
}
