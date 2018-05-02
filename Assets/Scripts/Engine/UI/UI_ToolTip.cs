using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ToolTip : UI_Base 
{
	[Header("ToolTip Properties:")]
	public string Title;
	public bool Fade = true;
	public float FadeSmooth = 0.01f;
	[Range(0.0f,1.0f)] public float FadeMin = 0.0f;
	[Range(0.0f,1.0f)] public float FadeMax = 1.0f;

	private float FadeRatio;
	private RectTransform TransformComponent;

	void Awake () 
	{
		TextComponents = GetComponentsInChildren<Text> ();
		if (TextComponents.Length <= 0)
			Debug.LogError ("UI_ToolTip: Text components are null");

		ImageComponents = GetComponentsInChildren<Image> ();
		if (ImageComponents.Length <= 0)
			Debug.LogError ("UI_ToolTip: Image components are null");

		TransformComponents = GetComponentsInChildren<RectTransform> ();
		if (TransformComponents.Length <= 0)
			Debug.LogError ("UI_ToolTip: Transform components are null");

		TransformComponent = GetComponent<RectTransform> ();
		if (!TransformComponent)
			Debug.LogError ("UI_ToolTip: Transform component is null");

		if (Fade) 
		{
			Color OldColor;
			for (int i = 0; i < ImageComponents.Length; i++) 
			{
				OldColor = ImageComponents [i].color;
				OldColor.a = 0.0f;
				ImageComponents [i].color = OldColor;
			}

			for (int i = 0; i < TextComponents.Length; i++)
			{
				OldColor = TextComponents [i].color;
				OldColor.a = 0.0f;
				TextComponents [i].color = OldColor;
			}
		}
	}

	void Start()
	{
		GetText ("Title").text = Title;
	}

	void Update () 
	{
		if (Fade) 
		{
			Color OldColor;
			for (int i = 0; i < ImageComponents.Length; i++)
			{
				OldColor = ImageComponents [i].color;
				FadeRatio = Mathf.Lerp (FadeRatio, 1.0f, FadeSmooth);
				FadeRatio = Mathf.Clamp (FadeRatio, FadeMin, FadeMax);
				ImageComponents [i].color = new Color (OldColor.r,OldColor.g,OldColor.b, FadeRatio );
			}

			for (int i = 0; i < TextComponents.Length; i++)
			{
				OldColor = TextComponents [i].color;
				FadeRatio = Mathf.Lerp (FadeRatio, 1.0f, FadeSmooth);
				FadeRatio = Mathf.Clamp (FadeRatio, FadeMin, FadeMax);
				TextComponents [i].color = new Color (OldColor.r,OldColor.g,OldColor.b, FadeRatio );
			}
		}
	}



}
