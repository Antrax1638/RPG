using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition
{
	public State State;
	public bool Enter;
}

//[CreateAssetMenu(fileName = "State",menuName = "IA/Action", order = 1)]
public class State : MonoBehaviour
{
	[Header("Transition Properties:")]
	public Transition[] States;

	[Header("State Properties:")]
	public string Name;
	public bool DebugMode;

	[HideInInspector]
	public bool TransitionPass = false;
	[HideInInspector]
	public State TransitionState;

	void Update()
	{
		TransitionPass = false;
		for (int i = 0; i < States.Length; i++) {
			if (States [i].Enter)
			{
				TransitionPass = States [i].Enter;
				TransitionState = States [i].State;
			}
		}
			
		if (!TransitionPass) 
		{
			UpdateState ();
		}
	}

	protected virtual void UpdateState()
	{
		if (DebugMode)
			Debug.Log (Name);
	}

	protected Transition GetTransitionByName(string Name)
	{
		for (int i = 0; i < States.Length; i++) {
			if (States [i].State.Name == Name) {
				return States [i];
			}
		}
		return null;
	}

	protected Transition GetTransitionAt(int Index)
	{
		if (Index >= 0 && Index < States.Length) 
			return States [Index];
		else
			return null;
	}
}
