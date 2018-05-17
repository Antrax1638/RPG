using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour 
{
	[Header("Item Properties:")]
	public int Id;
	public string Name;
	public TextAsset Description;
	public EWeapon Type;
	public Sprite Icon;
	public Vector2Int Size;

	[HideInInspector] public UI_Item Interface {get{ return ItemInterface; }}

	private UI_Item ItemInterface;

	void Awake ()
	{
		ItemInterface = new UI_Item (Id, Icon, Size);
	}

	void Update () {
		
	}

	public virtual void PickUp()
	{
		if(gameObject.activeInHierarchy)
		{
			gameObject.SetActive (false);	
		}
	}

	public virtual void Drop(Vector3 Offset)
	{
		if(!gameObject.activeInHierarchy)
		{
			gameObject.SetActive (true);
		}
	}

	public virtual bool Equal(Item Other)
	{
		if (Other == null)
			return false;

		bool Success = false;
		Success = Success || Id == Other.Id;
		Success = Success || Type == Other.Type;
		Success = Success || Name == Other.Name;
		return Success;
	}

	public virtual bool Equal(GameObject Other)
	{
		Item Temp = Other.GetComponent<Item> ();
		return Equal (Temp);
	}

}
