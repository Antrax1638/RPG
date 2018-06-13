using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Character : UI_Window
{
    [Header("Character Properties:")]
    public UI_Inventory Inventory;
    public DB_Item ItemDatabase;

    [Header("Equipament:")]
    public UI_EquipSlot[] EquipamentSlots;

	void Start ()
    {
		
	}

	protected override void Update ()
    {
        base.Update();
        UpdateSlots();
	}

    public void UpdateSlots()
    {
        Item TempItem = null;
        for (int i = 0; i < EquipamentSlots.Length; i++)
        {
            EquipamentSlots[i].Id = i;
            if (EquipamentSlots[i] && Inventory.ValidateItemFormat(EquipamentSlots[i].Item))
            {
                for (int j = 0; j < ItemDatabase.Items.Count; j++)
                {
                    if (ItemDatabase.Items[j].Id == EquipamentSlots[i].Item.Id)
                    {
                        TempItem = ItemDatabase.Items[i];
                        
                    }
                }
            }
        }
    }
}
