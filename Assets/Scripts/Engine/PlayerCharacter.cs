using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public PlayerController Controller;

    [Header("HUD")]
    public UI_Hud Hud; 

    [Header("Inventory:")] 
    public InventoryManager Inventory;
    public int InventorySize;

    [Header("Items:")]
    public float DetectionRadius = 2.5f;
	public float DetectionLength = 1.0f;
	public LayerMask DetectionMask;

	[Header("Actions")]
	public string PickUpAction = "PickUpAll";

	

	private Transform[] ObjectTransform;


	void Awake ()
	{
		Inventory = new InventoryManager (InventorySize, Inventory.DebugMode);
	}

	void Update () 
	{
        PickUpAll();

        if (Hud)
        {
            if(Hud.ActionBar)
            {
                Hud.ActionBar.Health = Stat.Health;
                Hud.ActionBar.MaxHealth = Stat.MaxHealth;
            }

        }
	}

    void PickUp()
    {
        RaycastHit[] ItemHits;
        ItemHits = Physics.CapsuleCastAll(transform.position, transform.position, DetectionRadius, Vector3.down, DetectionLength, DetectionMask);
        if (ItemHits != null)
        {
            ObjectTransform = new Transform[ItemHits.Length];
            for (int i = 0; i < ItemHits.Length; i++)
            {
                ObjectTransform[i] = ItemHits[i].collider.transform.parent;
                if (!ObjectTransform[i])
                    ObjectTransform[i] = ItemHits[i].collider.transform;
            }
        }

        Item ThisItem = null;
        for (int i = 0; i < ObjectTransform.Length; i++)
        {
            ThisItem = ObjectTransform[i].GetComponent<Item>();
            if (ThisItem && ThisItem.State > 0) {
                
            }
        }
    }

    void PickUpAll()
    {
        if (Input.GetButtonDown(PickUpAction))
        {
            RaycastHit[] ItemHits;
            ItemHits = Physics.CapsuleCastAll(transform.position, transform.position, DetectionRadius, Vector3.down, DetectionLength, DetectionMask);
            if (ItemHits != null)
            {
                ObjectTransform = new Transform[ItemHits.Length];
                for (int i = 0; i < ItemHits.Length; i++)
                {
                    ObjectTransform[i] = ItemHits[i].collider.transform.parent;
                    if (!ObjectTransform[i])
                        ObjectTransform[i] = ItemHits[i].collider.transform;
                }
            }

            //PickUp:
            if (ObjectTransform != null && ObjectTransform.Length > 0)
            {
                Item ThisItem = null;
                for (int i = 0; i < ObjectTransform.Length; i++)
                {
                    ThisItem = ObjectTransform[i].GetComponent<Item>();
                    if (ThisItem)
                    {
                        
                        ThisItem.PickUp();
                        Inventory.AddItem(ObjectTransform[i].gameObject);
                    }
                    print(ObjectTransform[i].name);
                }
            }
        }
    }

	void OnDrawGizmos()
	{
		if (ObjectTransform != null)
		{
			Gizmos.color = Color.cyan;
			for (int i = 0; i < ObjectTransform.Length; i++) {
				Gizmos.DrawWireCube (ObjectTransform [i].position, new Vector3 (0.5f, 0.5f, 0.5f));
			}
		}
	}
}
