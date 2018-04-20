using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour 
{
	public Stats Stat;

	protected AnimationState AnimationStateComponent;

	void Awake () 
	{
		AnimationStateComponent = GetComponent<AnimationState> ();
		if (!AnimationStateComponent)
			Debug.LogError ("Character: animation state is null");
	}

	void Update ()
	{
		
	}
}
