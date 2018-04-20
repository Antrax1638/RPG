using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryManager
{
	public InventoryManager()
	{
		Inventory = new GameObject[Length];
		InventoryStack = new int[Length];
	}

	public InventoryManager(int Size)
	{
		Inventory = new GameObject[Length];
		InventoryStack = new int[Length];
	}

	protected GameObject[] Inventory;
	protected int[] InventoryStack;

	public int Length { 
		get 
		{
			return (Inventory != null) ? Inventory.Length : 0;
		} 
	}

	public int AddItem(GameObject Item)
	{
		if(Item && Length > 0)
		{
			for (int index = 0; index < Inventory.Length; index++) 
			{
				if (Inventory [index] == null) {
					Inventory [index] = Item;
					InventoryStack [index] = 1;
					return index;
				} 
				else
				{
					if (Inventory [index].Equals (Item)) {
						InventoryStack [index] += 1;
					}
				}
			}
			return -1;
		}
		return -1;
	}

	void AddItemAt(GameObject Item,int Index)
	{
		if (Item || (Index < 0 && Index > Inventory.Length))
			return;

		if (Inventory [Index] == null) {
			Inventory [Index] = Item;
			InventoryStack [Index] = 1;
		} else {
			if (Inventory [Index].Equals (Item)) {
				InventoryStack [Index] += 1;
			}
		}

	}

	public GameObject GetItemAt(int Index)
	{
		if (Index >= 0 && Index < Inventory.Length) {
			return Inventory [Index];
		}
		return null;
	}

	public int GetStackAt(int Index)
	{
		if (Index >= 0 && Index < InventoryStack.Length) {
			return InventoryStack [Index];
		}
		return -1;
	}

	public void Resize(int NewSize)
	{
		GameObject[] Temp = new GameObject[Inventory.Length];
		int[] TempStack = new int[InventoryStack.Length]; 

		bool Continue = (Temp.Length == TempStack.Length); 
		if (Continue)
		{
			for (int i = 0; i < Inventory.Length; i++) {
				Temp [i] = Inventory [i];
				TempStack [i] = InventoryStack [i];
			}

			Inventory = new GameObject[NewSize];
			InventoryStack = new int[NewSize];

			for (int i = 0; i < Inventory.Length; i++) {
				Inventory [i] = Temp [i];
				InventoryStack [i] = TempStack [i];
			}

			Temp = new GameObject[0];
			TempStack = new int[0];
		}
	}

	public void Empty()
	{
		Inventory = new GameObject[Length];
		InventoryStack = new int[Length];
	}
}
