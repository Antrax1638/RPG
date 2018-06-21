using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMManager : MonoBehaviour 
{
    [Header("States:")]
    public State Default;

	private GameObject DefaultObject;

	void Start () 
	{
		DefaultObject = Instantiate (Default.gameObject,transform);
		Default = DefaultObject.GetComponent<State> ();
		Default.transform.localPosition = Vector3.zero;
	}

	void Update()
	{
		MakeTransition ();	
	}

	void MakeTransition()
	{
		if(Default.TransitionPass)
		{
			GameObject OldObject = DefaultObject;
			State OldState = Default;
			if (OldState.TransitionState != null) 
			{
				DefaultObject = Instantiate (OldState.TransitionState.gameObject,transform);
				DefaultObject.transform.localPosition = Vector3.zero;
				Default = DefaultObject.GetComponent<State> ();
				Destroy (OldObject);
			}
		}
	}
}
