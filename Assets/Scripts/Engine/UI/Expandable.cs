using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Expandable : UI_Base 
{
	[Header("Expandable Properties:")]
	public bool Force;
	public bool DefaultOff = true;
	[Range(-1,1)] public int Horizontal;
	[Range(-1,1)] public int Vertical;
	public RectTransform Target;
	public List<GameObject> Components = new List<GameObject> (); 
	public bool IsToggle {get{return ToggleValue;}}

	private RectTransform TransformComponent;
	private Toggle ToggleComponent;
	private Vector2 InitialDeltaSize;
	private bool ToggleValue;

	protected virtual void Awake()
	{
		ToggleComponent = GetComponent<Toggle> ();
		if (ToggleComponent) {
			ToggleComponent.onValueChanged.AddListener (OnClick);
		} else
			Debug.LogError ("Expandable: Toggle component is null");

		TransformComponent = GetComponent<RectTransform> ();
		if (!TransformComponent)
			Debug.LogError ("Expandable: Transform component is null");
		else
			InitialDeltaSize = TransformComponent.sizeDelta;
		
		if (!Target)
			Debug.LogError ("Expandable: Target component is null");

		EventNames = new List<string> ();
	}

	protected virtual void Start()
	{
		if (Target.childCount > 0 && Force)
		{
			Components = new List<GameObject> ();
			for (int i = 0; i < Target.childCount; i++) {
				Components.Add (Target.GetChild (i).gameObject);
			}
		} else {
			for (int i = 0; i < Components.Count; i++) {
				if (Components [i] != null && !Components [i].activeInHierarchy) {
					string OldName = Components [i].name;
					Components [i] = Instantiate (Components [i], Target);
					Components [i].name = OldName + "[" + i + "]";
				} else {
					Components [i] = Target.GetChild (i).gameObject;
				}
			}
		}

		Target.gameObject.SetActive (!DefaultOff);
	}

	public virtual void OnClick(bool value)
	{
		if(value)
		{
			RectTransform Current; 
			Vector2 TotalSize = TransformComponent.sizeDelta;
			if (!EventNames.Contains ("OnExpand"))
				EventNames.Add ("OnExpand");
			/*if (TransformComponent) 
			{
				for (int i = 0; i < Target.childCount; i++) 
				{
					Current = Target.GetChild (i).GetComponent<RectTransform> ();
					TotalSize.x += Current.sizeDelta.x * Horizontal; 
					TotalSize.y += Current.sizeDelta.y * Vertical; 
				}

				TransformComponent.sizeDelta = TotalSize;
			}*/
		}
		else
		{
			TransformComponent.sizeDelta = InitialDeltaSize;
			if (!EventNames.Contains ("OnContract"))
				EventNames.Add ("OnContract");
		}
		ToggleValue = value;

	}

}
