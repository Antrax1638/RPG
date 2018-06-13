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
    public int State = -1;

    [Header("Interactable:")]
    public GameObject Interactable;
    public Vector3 InteractableOffset;

	[HideInInspector] public UI_Item Interface {get{ return ItemInterface; }}

	private UI_Item ItemInterface;
    private GameObject InteractableObject;

	void Awake ()
	{
		ItemInterface = new UI_Item (Id, Icon, Size);
        State = 0;

        //Aca empieza el hardcodeo
        Button BT = GetComponentInChildren<Button>();
        BT.onClick.AddListener(PickUp);
        Text Txt = GetComponentInChildren<Text>();
        Txt.text = Name;
        InteractableObject = transform.Find("Interactable").gameObject;
        InteractableObject.transform.localPosition = InteractableObject.transform.localPosition + InteractableOffset;
	}

	void Update ()
    {
        if (InteractableObject) {
            InteractableObject.transform.LookAt(Camera.main.transform);
        }
	}

	public virtual void PickUp()
	{
		if(gameObject.activeInHierarchy)
		{
            State = 1;
            gameObject.SetActive (false);	
		}
	}

	public virtual void Drop(Vector3 Offset)
	{
		if(!gameObject.activeInHierarchy)
		{
            State = 0;
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
