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
    public string StayAction = "Stay";
	public string PickUpAction = "PickUpAll";
    public string PrimaryAttackAction = "Fire1";
    public string SecondaryAttackAction = "Fire2";



    private Transform[] ObjectTransform;
    private AnimationState AnimationStateComponent;


	void Awake ()
	{
		Inventory = new InventoryManager (InventorySize, Inventory.DebugMode);
        AnimationStateComponent = GetComponent<AnimationState>();

    }

	void Update () 
	{
        Attacks();
        PickUpAll();

        if (Stat.Health <= 0)
        {
            Stat.IsDead = true;
            Destroy(gameObject);
            Destroy(Controller.gameObject);
        }

        if (Hud)
        {
            if(Hud.ActionBar)
            {
                Hud.ActionBar.Health = Stat.Health;
                Hud.ActionBar.MaxHealth = Stat.MaxHealth;
            }

        }
	}

    void Attacks()
    {
        bool Active = Input.GetButton(StayAction);
        Controller.InputMode = (Active) ? InputMode.None : InputMode.Game;

        if (Input.GetButtonDown(PrimaryAttackAction) && Active)
        {
            AnimationStateComponent.AttackValue = 1;
            AnimationStateComponent.Attack(EWeapon.RightSword, 1);
            
        }

        if (Input.GetButtonDown(SecondaryAttackAction) && Active)
        {
            AnimationStateComponent.AttackValue = 1;
            AnimationStateComponent.Attack(EWeapon.RightSword, 2);
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
