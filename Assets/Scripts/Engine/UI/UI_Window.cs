using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Window : UI_Base, IPointerClickHandler, IDragHandler , IBeginDragHandler, IEndDragHandler {
	
	[Header("Window Properties:")]
	public bool Focusable;
	public bool Draggable;
	public KeyModifier ToggleModifier; 
	public string ToggleAction;
	public KeyCode CloseKey = KeyCode.Escape;

	[HideInInspector] public bool Activated { get{return IsActivated;} }

	protected Vector2 InitialPivotPoint = Vector2.zero;
	protected Vector2 LocalPivotPoint = Vector2.zero;
	protected RectTransform TransformComponent;
	protected bool Dragged,IsActivated;


	protected virtual void Awake ()
	{
		TransformComponent = GetComponent<RectTransform> ();
		if (!TransformComponent)
			Debug.LogError ("UI_Window: transform component is null");

		TransformComponents = GetComponentsInChildren<RectTransform> ();
		if (TransformComponents == null)
			Debug.Log("UI_Window: transform components are null");

		InitialPivotPoint = TransformComponent.pivot;
		IsActivated = gameObject.activeInHierarchy;
	}
		
	protected virtual void Update()
	{	
		if (Input.GetButtonDown (ToggleAction) && UI_Manager.Instance.InputKeyModifier(ToggleModifier))
		{
			ToggleWindow ();
		}

		if (Input.GetKeyDown (CloseKey))
			CloseWindow ();
	}

	public virtual void OnPointerClick (PointerEventData eventData)
	{
		if (!IsActivated || Visible == Visiblility.Hidden)
			return;
		
		if (Focusable)
		{
			if (!EventNames.Contains ("OnPointerClick"))
				EventNames.Add ("OnPointerClick");

			TransformComponent.SetAsLastSibling ();
			UI_Manager.Instance.Focus = gameObject;
		}
	}
		
	public virtual void OnDrag (PointerEventData eventData)
	{
		if (!IsActivated || Visible == Visiblility.Hidden)
			return;
		
		if (Draggable)
		{
			if (!EventNames.Contains ("OnDrag"))
				EventNames.Add ("OnDrag");
			TransformComponent.anchoredPosition = Input.mousePosition;
		}
	}
		
	public virtual void OnBeginDrag (PointerEventData eventData)
	{
		if (!IsActivated || Visible == Visiblility.Hidden
		)
			return;

		if (Focusable) {
			TransformComponent.SetAsLastSibling ();
			UI_Manager.Instance.Focus = gameObject;
		}

		if (Draggable) {
			if (!EventNames.Contains ("OnBeginDrag"))
				EventNames.Add ("OnBeginDrag");

			Canvas ParentCanvas = GetComponentInParent<Canvas> ();
			RectTransformUtility.ScreenPointToLocalPointInRectangle (TransformComponent, eventData.position, ParentCanvas.worldCamera, out LocalPivotPoint);
			LocalPivotPoint = Rect.PointToNormalized (TransformComponent.rect, LocalPivotPoint);
			TransformComponent.pivot = LocalPivotPoint;
			Dragged = true;
		}	
	}

	public virtual void OnEndDrag (PointerEventData eventData)
	{
		Dragged = false;
		if (!EventNames.Contains ("OnEndDrag"))
			EventNames.Add ("OnEndDrag");
	}
		
	public void CloseWindow()
	{
		if (TransformComponents != null) {
			for (int i = 0; i < TransformComponents.Length; i++) {
				TransformComponents [i].gameObject.SetActive (false);
			}
		}
		IsActivated = false;
		this.gameObject.SetActive (true);

		if (!EventNames.Contains ("OnWindowClose"))
			EventNames.Add ("OnWindowClose");
	}

	public void OpenWindow()
	{
		if (TransformComponents != null) {
			for (int i = 0; i < TransformComponents.Length; i++) {
				TransformComponents [i].gameObject.SetActive (true);
			}
		}
		IsActivated = true;
		this.gameObject.SetActive (true);

		if (!EventNames.Contains ("OnWindowOpen"))
			EventNames.Add ("OnWindowOpen");
	}

	public void ToggleWindow()
	{
		if (IsActivated)
			CloseWindow ();
		else
			OpenWindow ();
	}
		
}
