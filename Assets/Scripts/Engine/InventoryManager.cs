using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryManager : IDebugMode
{
	public InventoryManager()
	{
        DebugMode = false;
        Inventory = new GameObject[0];
		InventoryStack = new int[0];
        DebugLog("Inventory Initialized Null", 0);
        
    }

	public InventoryManager(int Size,bool Debug = false)
	{
        DebugMode = Debug;
        Inventory = new GameObject[Size];
        InventoryStack = new int[Size];
        DebugLog("Inventory Initialized ["+Size+"]", 0);
    }

	protected GameObject[] Inventory;
	protected int[] InventoryStack;

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
                    DebugLog("New Item Added at ["+index+"]", 0);
                    return index;
				} 
				else
				{
					ItemComponent = Inventory [index].GetComponent<Item> ();
					if (ItemComponent.Equal(Inventory[index]))
					{
                        InventoryStack[index] += 1;
                        DebugLog("Item Stacked at [" + index + "]", 0);
                    }
				}
			}
            DebugLog("Inventory is full", DebugType.Info);
            return -1;
		}
        DebugLog("Inventory is null", DebugType.Error);
        return -1;
	}

	public void AddItemAt(GameObject NewItem,int Index)
	{
		if (NewItem == null || (Index < 0 && Index > Inventory.Length))
        {
            DebugLog("Invalid Inventory Index", DebugType.Error);
            DebugLog("Item: "+ NewItem, DebugType.Error);
        }

		if (Inventory [Index] == null)
		{
			Inventory [Index] = NewItem;
			InventoryStack [Index] = 1;
            DebugLog("New Item Added To ["+Index+"]", 0);
        } 
		else 
		{
			Item Temp = NewItem.GetComponent<Item> ();
			if (Temp.Equal (Inventory [Index])) 
			{
				InventoryStack [Index] += 1;
                DebugLog("Item Stacked To [" + Index + "]", 0);
            } 
			else 
			{
				Inventory [Index] = NewItem;
				InventoryStack [Index] = 1;
                DebugLog("New Item Added To [" + Index + "]", 0);
                DebugLog("Old Item Lost", 0);
            }
		}
	}

	public GameObject RemoveItem(GameObject Other)
	{
		if (Other == null)
        {
            DebugLog("Invalid Item to Remove", 0);
            return null;
        }
			

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
                    DebugLog("Item Removed from [" + i + "]", 0);
                    return ItemTemp;
                } 
				else
				{
                    DebugLog("Item Copied from [" + i + "]", 0);
                    return Inventory[i];
				}
			}
		}

        DebugLog("Item not found", 0);
        return null;
	}

	public GameObject RemoveAt(int Index)
	{
		if (Index >= 0 && Index < Inventory.Length)
        {
			bool StackEmpty = InventoryStack [Index] >= 0;
			if (StackEmpty) {
				GameObject Temp = Inventory [Index];
				Inventory [Index] = null;
                DebugLog("Item Removed from [" + Index + "]", 0);
                return Temp;
			}
            else
            {
                DebugLog("Item Copied from [" + Index + "]", 0);
                return Inventory[Index];
			}
		}

        DebugLog("Inventory Index Invalid", DebugType.Error);
        return null;
	}

	public GameObject GetItemAt(int Index)
	{
		if (Index >= 0 && Index < Inventory.Length) {
            DebugLog("Item Get from [" + Index + "]", 0);
            return Inventory [Index];
		}
        DebugLog("Inventory Index Invalid", DebugType.Error);
        return null;
	}

	public int GetStackAt(int Index)
	{
		if (Index >= 0 && Index < InventoryStack.Length) {
            DebugLog("Stack Get from [" + Index + "]", 0);
            return InventoryStack [Index];
		}
        DebugLog("Inventory Index Invalid", DebugType.Error);
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
            DebugLog("Iventory Resize from ["+Temp.Length+" To "+NewSize+"]", 0);

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
        DebugLog("Ivnetory Cleared", 0);
    }

	public void Empty()
	{
		Inventory = new GameObject[0];
		InventoryStack = new int[0];
        DebugLog("Inventory Empty", 0);
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
