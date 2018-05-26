using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Category : Expandable
{
	[Header("Category:")]
	public bool Enable;
	public DB_Blueprint Database;

	private UI_Crafting CraftingWindowComponent;

	protected override void Awake()
	{
		base.Awake ();

		if (Enable) {
			CraftingWindowComponent = GetComponentInParent<UI_Crafting> ();
			if (!CraftingWindowComponent)
				Debug.LogError ("UI_Crategory: Crafting window component is null");
		}

	}

	protected override void Start ()
	{
		base.Start ();
	
	}
	
	public override void OnClick(bool Value)
	{
		base.OnClick (Value);

		if (CraftingWindowComponent) {
			CraftingWindowComponent.Changed = true;
		}

		if (Value) 
		{
			Vector2 NewSize = Vector2.zero;
			NewSize.x = TransformComponent.sizeDelta.x + Target.sizeDelta.x * Horizontal;
			NewSize.y = TransformComponent.sizeDelta.y + Target.sizeDelta.y * Vertical;
			TransformComponent.sizeDelta = NewSize;
		}
		else
		{
			TransformComponent.sizeDelta = InitialDeltaSize;
		}
	}

	void AddBlueprint()
	{
		
	}
}
