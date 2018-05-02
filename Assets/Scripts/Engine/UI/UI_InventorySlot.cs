﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_InventorySlot : UI_Slot
{
	public enum RemoveType
	{
		RemoveOnDrag,
		RemoveOnDrop
	}

	[Header("Inventory Properties:")]
	public UI_Item Item;
	public bool RemoveEvent = false;
	public RemoveType Remove = UI_InventorySlot.RemoveType.RemoveOnDrop;

	private UI_Inventory ParentInventory;
	private UI_Item TempDrag;

	protected override void Awake()
	{
		base.Awake ();

		ParentInventory = GetComponentInParent<UI_Inventory> ();
		if (!ParentInventory)
			Debug.LogError ("UI_InventorySlot: inventory class component is null");
	}

	protected override void Start()
	{
		base.Start ();
	}

	protected override void Update()
	{
		base.Update ();
	}

	//Operations:
	public override void OnBeginDrag(PointerEventData Data)
	{
		base.OnBeginDrag (Data);
		if (Remove == RemoveType.RemoveOnDrag) {
			TempDrag = Item;
			ParentInventory.RemoveItem (Position);
		}
	}

	public override void OnDrag(PointerEventData Data)
	{
		base.OnDrag (Data);
	}

	public override void OnEndDrag(PointerEventData Data)
	{
		base.OnEndDrag (Data);
	}

	public override void OnPointerEnter(PointerEventData Data)
	{
		base.OnPointerEnter (Data);
	}

	public override void OnPointerExit(PointerEventData Data)
	{
		base.OnPointerExit (Data);
	}

	public override void OnDrop (GameObject Slot)
	{
		base.OnDrop (Slot);

		if (Visible == Visiblility.Hidden)
			return;

		UI_InventorySlot SlotComponent = Slot.GetComponent<UI_InventorySlot> ();
		if (SlotComponent) 
		{
			UI_Item Item = (this.Item != UI_Item.invalid) ? this.Item : TempDrag;
			Vector2Int NewPosition = ParentInventory.AddItem (Item,SlotComponent.Position);
			if (NewPosition != UI_Inventory.InvalidIndex) {
				if (Remove == RemoveType.RemoveOnDrop) {
					ParentInventory.RemoveItem (Position);
				}

			} else {
				if (Remove == RemoveType.RemoveOnDrag) {
					ParentInventory.AddItem (Item, Position);
				}
			}
		}
	}
}