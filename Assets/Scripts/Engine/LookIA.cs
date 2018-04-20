using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookIA : MonoBehaviour 
{

	private SphereCollider ColliderComponent;

	void Awake () 
	{
		ColliderComponent = GetComponent<SphereCollider> ();
	}

	void Update () 
	{
		
	}

	void OnTriggerEnter (Collider Other)
	{
		if (Other.gameObject.tag == "Player") {
			Debug.Log ("Te Veo!!!!");
		}
	}
}
