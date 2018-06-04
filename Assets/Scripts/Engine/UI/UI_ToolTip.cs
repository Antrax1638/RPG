using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ToolTip : UI_Base 
{
	[Header("General Properties:")]
	public bool Fade = true;
	public float FadeSmooth = 0.01f;
	[Range(0.0f,1.0f)] public float FadeMin = 0.0f;
	[Range(0.0f,1.0f)] public float FadeMax = 1.0f;

    [Header("Title Properties:")]
    public Text Title;
    public string TitleText;

    private float FadeRatio;
	private RectTransform TransformComponent;
    private CanvasGroup CanvasGroupComponent;

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
            CanvasGroupComponent = GetComponent<CanvasGroup>();
            if (!CanvasGroupComponent)
            {
                Debug.LogWarning("UI_ToolTip: Canvas group component is null");
                Debug.LogWarning("UI_ToolTip: Adding component...");

                CanvasGroupComponent = gameObject.AddComponent<CanvasGroup>();
            }
        }
	}

	void Start()
	{
        TitleUpdate();
        if (Fade && CanvasGroupComponent)
            CanvasGroupComponent.alpha = 0.0f;
    }

	void Update () 
	{
        if (Fade && FadeRatio < 0.99 && CanvasGroupComponent)
        {
            FadeRatio = Mathf.Clamp(FadeRatio, FadeMin, FadeMax);
            FadeRatio = Mathf.Lerp(FadeRatio, 1.0f, FadeSmooth);

            CanvasGroupComponent.alpha = FadeRatio;
        }
    }

    void TitleUpdate()
    {
        if (Title)
        {
            Title.text = TitleText;
        }
    }

}
