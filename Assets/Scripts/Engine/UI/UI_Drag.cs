﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_Drag : MonoBehaviour
{
	[Header("Drag Properties")]
	public Color DragColor = Color.white;
	public Color DropColor = Color.white;
	public Vector2 DragOffset = Vector2.zero;
	public Sprite DragIcon;
	public bool IsDrag { get{ return MouseDrag; } }

	private bool MouseDrag;
	private Image ImageComponent;
	private RectTransform TransformComponent;
	private Color DefaultColor;

	void Awake ()
	{
		ImageComponent = GetComponent<Image> ();
		if (!ImageComponent)
			Debug.LogError ("UI_Drag: image component is null");

		TransformComponent = GetComponent<RectTransform> ();
		if (!TransformComponent)
			Debug.LogError ("UI_Drag: transform component is null.");

		DefaultColor = ImageComponent.color;
	}

	void Update ()
	{
		
	}

	public void OnBeginDrag()
	{
		MouseDrag = true;
		gameObject.SetActive (true);
		ImageComponent.sprite = DragIcon;
		ImageComponent.color = DragColor;
	}

	public void OnDrag(Vector2 Position)
	{
		TransformComponent.anchoredPosition = Position + DragOffset;
	}

	public void OnEndDrag()
	{
		MouseDrag = false;
		Destroy (gameObject);
	}
}