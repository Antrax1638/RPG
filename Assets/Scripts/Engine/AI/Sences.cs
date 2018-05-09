using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(State))]
public class Sences : MonoBehaviour 
{
	public enum Detection
	{
		Relaxed,
		Scared,
		Alerted,
		Detected,
	}

	[Header("General Sences")]
	public bool Enable;
	public float AudioMemory = 0.0f;
	public float SightMemory = 0.0f;
	public float FieldOfView = 90.0f;
	[Tooltip("Filter all objects that this object can detect in the inmediate range.")]
	public string[] Tags;
	[Tooltip("Currently used player controller-character tag.")]
	public string PlayerTag = "Player";
	public Detection State;
	public float Instinct;
	public float ThreatGainRate;
	public float ThreatLossRate;

	[Header("Movement Sences:")]
	public float UpdateRate = 0.1f;
	public float ReductionFactor = 0.5f;

	private SphereCollider SphereComponent;
	private State StateComponent;
	private Dictionary<string,GameObject> ObjectsSenced = new Dictionary<string, GameObject>();
	private Dictionary<string,float> ObjectsThreat = new Dictionary<string, float> ();
	RaycastHit Hit;
	Vector3 Direction,LastPosition;
	bool isMoving;

	GameObject Target;

	void Awake()
	{
		SphereComponent = GetComponent<SphereCollider> ();
		if(!SphereComponent)
			Debug.LogError("AI_Sences: Sphere component is null");

		StateComponent = GetComponent<State> ();
		if(!StateComponent)
			Debug.LogError("AI_Sences: State component is null");

		if (Tags == null || Tags.Length <= 0) {
			Tags = new string[1];
			Tags [0] = PlayerTag;
		}

		InvokeRepeating ("OnMovementInvoke", 0, UpdateRate);
	}

	void Update()
	{
		isMoving = (transform.position != LastPosition);
	}

	void OnTriggerEnter(Collider Other)
	{
		if (!Enable)
			return;

		for (int i = 0; i < Tags.Length; i++) {
			if (Other.tag == Tags [i]) {
				ObjectsSenced.Add (Tags [i], Other.gameObject);
				ObjectsThreat.Add (Tags [i], 0.0f);
			}	
		}

	}

	void OnTriggerStay(Collider Other)
	{
		if (!ObjectsSenced.ContainsKey (Other.tag))
			return;

		bool TargetHit;
		Vector3 Distance, Direction, Position;
		Position = ObjectsSenced [Other.tag].transform.position;
		Distance = ObjectsSenced [Other.tag].transform.position - transform.position;
		Direction = Distance.normalized;
		this.Direction = Direction;
		float Angle = Vector3.Angle (Direction, transform.forward);
		if(Angle < (FieldOfView/2.0f) )
		{
			if(Physics.Raycast (transform.position, Direction,out Hit,Distance.magnitude))
			{
				string HitTag = Hit.collider.tag;
				TargetHit = (HitTag == Other.tag);
				ObjectsThreat [Other.tag] += ThreatGainRate;
			}
		}
		else
		{
			ObjectsThreat [Other.tag] = (!isMoving) ? ObjectsThreat[Other.tag] - ThreatLossRate : ObjectsThreat[Other.tag];

		}
		/*if (Physics.Raycast (transform.position, Direction,out Hit,Distance.magnitude)) 
		{
			string HitTag = Hit.collider.gameObject.tag;
			if (HitTag == Other.tag) 
			{
				if (Other.tag == PlayerTag) {
					StateComponent.GetTransitionByName ("Chase").Enter = true;
				}

			}
		}*/
	}

	void OnTriggerExit(Collider Other)
	{
		ObjectsSenced.Clear ();

	}

	void UpdateThreat()
	{
		
	}

	void OnMovementInvoke()
	{
		LastPosition = transform.position;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawRay (transform.position, Direction);
	}
}
