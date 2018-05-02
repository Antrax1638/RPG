using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
	public int InventorySize = 10;
	public float DetectionRadius = 2.5f;
	public float DetectionLength = 1.0f;
	public LayerMask DetectionMask;


	[Header("Actions")]
	public string PickUpAction = "PickUpAll";

	[HideInInspector()]
	public InventoryManager Inventory;

	private Transform[] ObjectTransform;

	void Awake ()
	{
		Inventory = new InventoryManager (InventorySize);
		print(Inventory.Length);
	}

	void Update () 
	{
		ItemCheck ();
		ItemUpdate ();
	}

	void ItemCheck()
	{
		RaycastHit[] ItemHits;
		ItemHits = Physics.CapsuleCastAll (transform.position, transform.position, DetectionRadius, Vector3.down, DetectionLength,DetectionMask);
		if(ItemHits != null)
		{
			ObjectTransform = new Transform[ItemHits.Length];
			for (int i = 0; i < ItemHits.Length; i++) {
				ObjectTransform[i] = ItemHits [i].collider.transform;
			}
		}
	}

	void ItemUpdate()
	{
		//PickUp:
		if (ObjectTransform != null && Input.GetButtonDown(PickUpAction)) 
		{
			for (int i = 0; i < ObjectTransform.Length; i++)
			{
				ObjectTransform[i].GetComponent<Item>().PickUp();
				Inventory.AddItem (ObjectTransform [i].gameObject);
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
