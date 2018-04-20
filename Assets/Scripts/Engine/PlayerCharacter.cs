using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
	public int InventorySize = 10;
	public InventoryManager Inventory;

	void Awake ()
	{
		Inventory = new InventoryManager (InventorySize);

	}

	void Update () 
	{
		if (Inventory.GetItemAt (0) != null)
			print (Inventory.GetItemAt (0).name);
		
	}
}
