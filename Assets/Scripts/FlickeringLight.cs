using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour {
	Light lt;
	float originalRange;

	[Space(10)]

	[Header("Light customization")]
	[Space(10)]
	[SerializeField]
	[Tooltip("The color of your light.")]
	Color lightColor = Color.yellow;

	[SerializeField]
	[Tooltip("The min intensity of your light.")]
	float minIntensity = 8f;
	[SerializeField]
	[Tooltip("The max intensity of your light.")]
	float maxIntensity = 3f;
	[SerializeField]
	[Tooltip("The minimal range of your light (radius).")]
	float minRange = 79f;
	[SerializeField]
	[Tooltip("The maximum range of your light (radius).")]
	float maxRange = 100f;

	float timer;
	//[HideInInspector] public float mytimescale=1;

	void Start() {
		lt = GetComponent<Light>();
		timer = 0f;
	}
	void FixedUpdate() {
		timer += Time.deltaTime;//*mytimescale;
		if (timer >= 0.1f) {
			lt.intensity = Random.Range (minIntensity, maxIntensity);
			lt.range = Random.Range (minRange, maxRange);
			lt.color = lightColor;
			timer = 0f;
		}
	}
}