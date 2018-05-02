using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum KeyModifier
{
	None,
	Control,
	Alt,
	Shift
}

public enum Visiblility
{
	Hidden,
	Visible
}

public class UI_Manager : MonoBehaviour
{
	[HideInInspector]
	public static UI_Manager Instance {get{return ManagerInstance;}}
	public GameObject PlayerController;

	protected List<GameObject> Windows = new List<GameObject>();
	private static UI_Manager ManagerInstance;
	private GameObject DragOperation;

	void Awake()
	{
		if (ManagerInstance == null)
		{
			ManagerInstance = this;
			DontDestroyOnLoad (gameObject);
		} 
		else
		{
			Destroy (gameObject);
		}
	}

	void Start()
	{
		RectTransform[] WindowGameObjects = GameObject.FindObjectsOfType<RectTransform> ();
		for (int i = 0; i < WindowGameObjects.Length; i++) 
		{
			if (WindowGameObjects [i].gameObject.layer == LayerMask.NameToLayer ("UI"))
				Windows.Add (WindowGameObjects [i].gameObject);
		}
		WindowGameObjects = new RectTransform[0];

	}

	void Update()
	{
		
	}
		
	public bool InputKeyModifier(KeyModifier Key)
	{
		bool Success = false;
		switch(Key)
		{
		//Control:
		case KeyModifier.Control:
			Success = Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl);
			break;
		//Alt:
		case KeyModifier.Alt:
			Success = Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt) || Input.GetKey (KeyCode.AltGr);
			break;
		//Shift:
		case KeyModifier.Shift:
			Success = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
			break;
		case KeyModifier.None: 
			Success = true;
			break;
		}
		return Success;
	}

	public int ToLength(int X,int Y,int Width)
	{
		return (Y * Width) + X;
	}

	public int ToLength(Vector2Int Coords,int Width)
	{
		return (Coords.y * Width) + Coords.x;
	}

	public Vector2Int ToCoord(int Index,int Width)
	{
		return new Vector2Int (Index%Width,Index/Width);
	}

}
