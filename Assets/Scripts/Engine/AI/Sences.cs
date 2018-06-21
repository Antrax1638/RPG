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
    public float Memory = 0.0f;
	public float FieldOfView = 90.0f;
    public Detection State;
    public float Instinct;

    [Header("Filters:")]
    [Tooltip("Currently used player controller-character tag.")] public string PlayerTag = "Player";
    [Tooltip("Filter all objects that this object can detect in the inmediate range.")]public string[] Tags;

    [Header("Threat:")]
	public float ThreatGainRate;
	public float ThreatLossRate;
    public float ThreatLimit;
    public float ThreatDelay;
	public float ThreatLossMultiplier = 2.0f;
 
	private SphereCollider SphereComponent;
	private State StateComponent;
    private Rigidbody BodyComponent;
	private Dictionary<string,GameObject> ObjectsSenced = new Dictionary<string, GameObject>();
	private Dictionary<string,float> ObjectsThreat = new Dictionary<string, float> ();
	RaycastHit Hit;
	Vector3 Direction,Velocity;

	GameObject Target;

	void Awake()
	{
		SphereComponent = GetComponent<SphereCollider> ();
		if(!SphereComponent)
			Debug.LogError("AI_Sences: Sphere component is null");

		StateComponent = GetComponent<State> ();
		if(!StateComponent)
			Debug.LogError("AI_Sences: State component is null");

        BodyComponent = GetComponent<Rigidbody>();
        if (!BodyComponent) BodyComponent = GetComponentInChildren<Rigidbody>();
        if (!BodyComponent) Debug.LogWarning("Body component is null");

        if (Tags == null || Tags.Length <= 0) {
			Tags = new string[1];
			Tags [0] = PlayerTag;
		}

	}

	void Update()
	{
        if (BodyComponent) Velocity = BodyComponent.velocity;    
	}

	void OnTriggerEnter(Collider Other)
	{
		if (!Enable)
			return;

		for (int i = 0; i < Tags.Length; i++)
        {
			if (Other.tag == Tags [i])
            {
                if(!ObjectsSenced.ContainsKey(Other.tag))
                    ObjectsSenced.Add (Tags [i], Other.gameObject);

                if(!ObjectsThreat.ContainsKey(Other.name))
                    ObjectsThreat.Add (Other.name, 0.0f);
			}	
		}
        State = Detection.Relaxed;
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
		if(Angle < (FieldOfView/2.0f))
		{
            State = Detection.Alerted;
            if (Physics.Raycast (transform.position, Direction,out Hit,Distance.magnitude))
			{
				string HitTag = Hit.collider.tag;
				TargetHit = (HitTag == Other.tag);
				ObjectsThreat [Other.name] += (1.0f/ThreatGainRate);
                Invoke("ThreatInvoke", ThreatDelay);
            }
        }
		else
		{
            State = Detection.Alerted;
            if (ObjectsThreat.ContainsKey(Other.name))
            {
                float VelocityFactor = (Velocity.magnitude > 0.0f) ? ThreatLossMultiplier : 1.0f;
                ObjectsThreat[Other.name] -= (1.0f / ThreatLossRate) * VelocityFactor;
       
            }
        }
        ObjectsThreat[Other.name] = Mathf.Clamp(ObjectsThreat[Other.name], 0.0f, 100.0f);
      
        if (ObjectsThreat[Other.name] > ThreatLimit)
            Invoke("ThreatInvoke", ThreatDelay);
	}

	void OnTriggerExit(Collider Other)
	{
        Invoke("MemoryInvoke", Memory);
	}

    void MemoryInvoke()
    {
        ObjectsSenced.Clear();
        ObjectsThreat.Clear();
        State = Detection.Relaxed;
    }

    void ThreatInvoke()
    {
        State = Detection.Detected;

        StateComponent.GetTransitionByName("Chase").Enter = true;
    }

    void OnDrawGizmos()
	{
		Gizmos.DrawRay (transform.position, Direction);
		Gizmos.color = Color.red;
		Gizmos.DrawRay (transform.position, transform.forward);
	}
}
