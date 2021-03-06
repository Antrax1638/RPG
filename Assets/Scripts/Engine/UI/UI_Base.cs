﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class UI_Base : MonoBehaviour
{
	protected Visiblility Visible = Visiblility.Visible;
	protected Image[] ImageComponents;
	protected RectTransform[] TransformComponents;
	protected Text[] TextComponents;

    protected CanvasGroup CanvasGroupComponent;
    private float OldAlpha;
    private bool OldInteractable, OldRaycast;

	public virtual Image GetImage(int Index)
	{
		if (Index >= 0 && Index < ImageComponents.Length)
			return ImageComponents [Index];
		else
			return null;
	}

	public virtual Image GetImage(string name)
	{
		if (ImageComponents == null)
			return null;

		for (int i = 0; i < ImageComponents.Length; i++){
			if (ImageComponents [i].name == name)
				return ImageComponents [i];
		}
		return null;
	}

	public virtual RectTransform GetTransform(int Index)
	{
		if (Index >= 0 && Index < TransformComponents.Length)
			return TransformComponents [Index];
		else
			return null;
	}

	public virtual RectTransform GetTransform(string name)
	{
		if (TransformComponents == null)
			return null;
		
		for (int i = 0; i < TransformComponents.Length; i++){
			if (TransformComponents [i].name == name)
				return TransformComponents [i];
		}
		return null;
	}

	public virtual Text GetText(int Index)
	{
		if (Index >= 0 && Index < TextComponents.Length)
			return TextComponents [Index];
		else
			return null;
	}

	public virtual Text GetText(string name)
	{
		if (TextComponents == null)
			return null;

		for (int i = 0; i < TextComponents.Length; i++){
			if (TextComponents [i].name == name)
				return TextComponents [i];
		}
		return null;
	}

    public void SetVisibility(Visiblility NewVisiblility)
    {
        if (CanvasGroupComponent)
        {
            Visible = NewVisiblility;
            switch (Visible)
            {
                case Visiblility.Visible:
                    CanvasGroupComponent.alpha = OldAlpha;
                    CanvasGroupComponent.blocksRaycasts = OldRaycast;
                    CanvasGroupComponent.interactable = OldInteractable;
                    break;

                case Visiblility.Hidden:
                    OldAlpha = CanvasGroupComponent.alpha;
                    OldRaycast = CanvasGroupComponent.blocksRaycasts;
                    OldInteractable = CanvasGroupComponent.interactable;

                    CanvasGroupComponent.alpha = 0.0f;
                    CanvasGroupComponent.blocksRaycasts = false;
                    CanvasGroupComponent.interactable = false;
                    break;
            }
        }
    }

	/*public void SetVisibility(Visiblility NewVisibility)
	{
		Visible = NewVisibility;
		Color TempColor;
		switch (Visible) {
		case Visiblility.Hidden:

			if (ImageComponents != null) {
				for (int i = 0; i < ImageComponents.Length; i++) {
					TempColor = ImageComponents [i].color;
					TempColor.a = 0.0f;
					ImageComponents [i].color = TempColor;
				}
			}

			if (TextComponents != null) 
			{
				for (int i = 0; i < TextComponents.Length; i++) {
					TempColor = TextComponents [i].color;
					TempColor.a = 0.0f;
					TextComponents [i].color = TempColor;
				}
			}
			break;

		case Visiblility.Visible:
			if (ImageComponents != null) {
				for (int i = 0; i < ImageComponents.Length; i++) {
					TempColor = ImageComponents [i].color;
					TempColor.a = 1.0f;
					ImageComponents [i].color = TempColor;
				}
			}

			if (TextComponents != null) {
				for (int i = 0; i < TextComponents.Length; i++) {
					TempColor = TextComponents [i].color;
					TempColor.a = 1.0f;
					TextComponents [i].color = TempColor;
				}
			}
			break;
		}
	}*/
}
