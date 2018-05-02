using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryManager
{
	public InventoryManager()
	{
		Inventory = new GameObject[0];
		InventoryStack = new int[0];
		InventoryStackRow = new List<GameObject>[0];
	}

	public InventoryManager(int Size)
	{
		Inventory = new GameObject[Size];
		InventoryStack = new int[Size];
		InventoryStackRow = new List<GameObject>[Size];
	}

	protected GameObject[] Inventory;
	protected int[] InventoryStack;
	protected List<GameObject>[] InventoryStackRow;

	public int Length { 
		get { return (Inventory != null) ? Inventory.Length : 0; } 
	}

	public int AddItem(GameObject NewItem)
	{
		if(NewItem && Length > 0)
		{
			Item ItemComponent = null;	
			for (int index = 0; index < Inventory.Length; index++) 
			{
				if (Inventory [index] == null) 
				{
					Inventory [index] = NewItem;
					InventoryStack [index] = 1;
					return index;
				} 
				else
				{
					ItemComponent = Inventory [index].GetComponent<Item> ();
					if (ItemComponent.Equal(Inventory[index]))
					{
						InventoryStack [index] += 1;
						InventoryStackRow [index].Add (NewItem);
					}
				}
			}
			return -1;
		}
		return -1;
	}

	public void AddItemAt(GameObject NewItem,int Index)
	{
		if (NewItem == null || (Index < 0 && Index > Inventory.Length))
			return;

		if (Inventory [Index] == null)
		{
			Inventory [Index] = NewItem;
			InventoryStack [Index] = 1;
		} 
		else 
		{
			Item Temp = NewItem.GetComponent<Item> ();
			if (Temp.Equal (Inventory [Index])) 
			{
				InventoryStack [Index] += 1;
				InventoryStackRow [Index].Add (NewItem);
			} 
			else 
			{
				Inventory [Index] = NewItem;
				InventoryStack [Index] = 1;
			}
		}
	}

	public GameObject RemoveItem(GameObject Other)
	{
		if (Other == null)
			return null;

		Item Temp = Other.GetComponent<Item> ();
		for (int i = 0; i < Inventory.Length; i++)
		{
			if(Temp.Equal(Inventory[i]))
			{
				bool StackEmpty = (InventoryStack [i] <= 0);
				if (StackEmpty)
				{
					GameObject ItemTemp = Inventory [i];
					Inventory [i] = null;
					return ItemTemp;
				} 
				else
				{
					GameObject ItemTemp = InventoryStackRow [i][0];
					InventoryStackRow [i].RemoveAt (0);
					return ItemTemp;
				}
			}
		}
		return null;
	}

	public GameObject RemoveAt(int Index)
	{
		if (Index >= 0 && Index < Inventory.Length) {
			bool StackEmpty = InventoryStack [Index] >= 0;
			if (StackEmpty) {
				GameObject Temp = Inventory [Index];
				Inventory [Index] = null;
				return Temp;
			} else {
				GameObject ItemTemp = InventoryStackRow [Index][0];
				InventoryStackRow [Index].RemoveAt (0);
				return ItemTemp;
			}
		}
		return null;
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

	public void Clear()
	{
		Inventory = new GameObject[Length];
		InventoryStack = new int[Length];
	}

	public void Empty()
	{
		Inventory = new GameObject[0];
		InventoryStack = new int[0];
	}

	public bool Contains(GameObject Other)
	{
		Item Temp = Other.GetComponent<Item> ();
		for (int i = 0; i < Inventory.Length; i++) 
		{
			if (Temp.Equal (Inventory [i]))
				return true;
		}
		return false;
	}
}
