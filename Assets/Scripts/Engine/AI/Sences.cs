using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sences : MonoBehaviour 
{
	[Header("AI Sences")]
	public float Radius;
	public string[] Tags;
	public string PlayerTag = "Player";

	private SphereCollider SphereComponent;
	private Dictionary<string,GameObject> ObjectsSenced = new Dictionary<string, GameObject>();
	private State StateComponent;
	private RaycastHit Hit;
	private Vector3 Direction;

	void Awake()
	{
		SphereComponent = GetComponent<SphereCollider> ();
		if(!SphereComponent)
			Debug.LogError("AI_Sences: Sphere component is null");

		StateComponent = GetComponent<State> ();
		if(!StateComponent)
			Debug.LogError("AI_Sences: State component is null");
	}

	void OnTriggerEnter(Collider Other)
	{
		for (int i = 0; i < Tags.Length; i++) {
			if (Other.tag == Tags [i]) {
				ObjectsSenced.Add (Tags [i], Other.gameObject);
			}	
		}

	}

	void OnTriggerStay(Collider Other)
	{
		if (!ObjectsSenced.ContainsKey (Other.tag))
			return;
		Vector3 Distance, Direction;

		Distance = ObjectsSenced [Other.tag].transform.position - transform.position;
		Direction = Distance.normalized;
		this.Direction = Direction;

		if (Physics.Raycast (transform.position, Direction,out Hit,Distance.magnitude)) 
		{
			if (Hit.collider.gameObject.tag == Other.tag) 
			{
				if (Other.tag == PlayerTag) {
					StateComponent.GetTransitionByName ("Chase").Enter = true;
				}

			}
		}
	}

	void OnTriggerExit(Collider Other)
	{
		ObjectsSenced.Clear ();

	}

	void OnDrawGizmos()
	{
		Gizmos.DrawRay (transform.position, Direction);
	}
}
