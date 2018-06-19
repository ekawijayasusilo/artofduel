using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
	void DestroyGameObject ()
	{
		// Destroy this gameobject, this can be called from an Animation Event.
		Destroy (gameObject);
	}
}
