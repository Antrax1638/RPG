using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Expandable : UI_Base, IPointerEnterHandler, IPointerExitHandler
{
	[Header("Expandable Properties:")]
	public bool Force;
	public bool DefaultOff = true;
	[Range(-1,1)] public int Horizontal;
	[Range(-1,1)] public int Vertical;
	public RectTransform Target;
	public List<GameObject> Components = new List<GameObject> (); 
	public bool IsToggle {get{return ToggleValue;}}
	public Vector2 SizeDelta{get{ return InitialDeltaSize; }}

	[Header("Expandable Transition:")]
	public bool ExpandableTransition;
	[SerializeField] Image ToggleImage = null;
	public Sprite ToggleOn;
	public Sprite ToggleOnHighlighted;
	public Sprite ToggleOff;
	public Sprite ToggleOffHighlighted;

	protected RectTransform TransformComponent;
	private VerticalLayoutGroup VerticalLayoutComponent;
	private Toggle ToggleComponent;
	protected Vector2 InitialDeltaSize;
	bool ToggleValue,MouseOver;
	Sprite LastSprite;

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
		
		if (!Target)
			Debug.LogError ("Expandable: Target component is null");

		VerticalLayoutComponent = GetComponentInChildren<VerticalLayoutGroup> ();
		if (!VerticalLayoutComponent)
			Debug.LogError ("Expandable: Vertical layout group component is null");

		ImageComponents = GetComponentsInChildren<Image> ();
		if (ImageComponents == null)
			Debug.LogError ("Expandable: Image components in child are null");

		if (Target && TransformComponent)
			InitialDeltaSize = Target.sizeDelta + TransformComponent.sizeDelta;

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
		if (ExpandableTransition) {
			ToggleImage.sprite = (ToggleComponent.isOn) ? ToggleOn : ToggleOff; 
		}
	}

	protected virtual void Update()
	{
		
	}

	public virtual void OnClick(bool value)
	{
		if(value)
		{
			RectTransform CTransform; 
			Vector2 TotalSize = TransformComponent.sizeDelta;
			
			if(TransformComponent)
			{
				for(int i = 0; i < Components.Count; i++)
				{
					CTransform = Components [i].GetComponent<RectTransform> ();
					TotalSize.x = (CTransform) ? TotalSize.x + (CTransform.sizeDelta.x * Horizontal) : TotalSize.x;
					TotalSize.y = (CTransform) ? TotalSize.y + (CTransform.sizeDelta.y * Vertical) : TotalSize.y;
				}
				Target.sizeDelta = TotalSize;
			}
		}
		else
		{
			Target.sizeDelta = InitialDeltaSize;
			
		}
		ToggleValue = value;
		if (ExpandableTransition) {
			ToggleImage.sprite = (ToggleComponent.isOn) ? ToggleOn : ToggleOff; 
		}


	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		MouseOver = true;
		if (ExpandableTransition) {
			ToggleImage.sprite = (ToggleComponent.isOn) ? ToggleOnHighlighted : ToggleOffHighlighted; 
		}
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		MouseOver = false;
		if (ExpandableTransition) {
			ToggleImage.sprite = (ToggleComponent.isOn) ? ToggleOn : ToggleOff; 
		}
	}
}
