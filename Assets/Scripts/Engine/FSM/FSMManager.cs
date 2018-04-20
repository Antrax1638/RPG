using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMManager : MonoBehaviour 
{
	public State Default;

	private GameObject DefaultObject;

	void Start () 
	{
		DefaultObject = Instantiate (Default.gameObject,transform);
		Default = DefaultObject.GetComponent<State> ();
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
				Default = DefaultObject.GetComponent<State> ();
				Destroy (OldObject);
			}
		}
	}
}
