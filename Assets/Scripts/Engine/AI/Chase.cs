using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(State))]
[RequireComponent(typeof(CharacterController))]
public class Chase : MonoBehaviour
{
    [Header("General:")]
    public GameObject Target;
    public float Radius;
    public LayerMask Layer;

    [Header("Movement")]
    public float Speed;
    public float JumpSpeed;

    [Header("Attack")]
    public float AttackSpeed;
    public float AttackRange;


    private Animator AnimComponent;
    private State StateComponent;
    private RaycastHit DetectionHit;
    private bool Detection;
    private Vector3 OriginalPosition;
    GameObject Parent;


    void Awake ()
    {
        Parent = transform.parent.gameObject;

        AnimComponent = Parent.GetComponentInChildren<Animator>();
        if (!AnimComponent) Debug.LogError("Animator component is null");

        StateComponent = GetComponent<State>();
        if (!StateComponent) Debug.LogError("State component is null");

	}

    void Start()
    {
        OriginalPosition = transform.position;
        Target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update ()
    {
        if (Target)
        {
            if (AnimComponent)
            {
                AnimComponent.SetBool("Walk", true);
                AnimComponent.SetFloat("Multiplier", Speed);
            }

            Vector3 TargetTransform = new Vector3(Target.transform.position.x,transform.position.y,Target.transform.position.z);
            Parent.transform.LookAt(TargetTransform);
            Vector3 Direction = Target.transform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Direction, out hit, AttackRange))
            {
               GameObject ObjectHit = hit.collider.gameObject;
               if(ObjectHit.tag == "Player")
               {
                    AnimComponent.SetFloat("AttackSpeed", AttackSpeed);
                    AnimComponent.SetTrigger("Attack");
               }
            }

        }
        else
        {
            AnimComponent.SetBool("Walk", false);
        }
	}
}
