using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Item : MonoBehaviour 
{
	public string DisplayName;
	public string DisplayDescription;

	protected SphereCollider SphereColliderComponent;

	void Awake () {
		SphereColliderComponent = GetComponent<SphereCollider> ();
		if (!SphereColliderComponent)
			Debug.LogError ("Item: sphere collider is null");
		else
			SphereColliderComponent.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider Other)
	{
		GameObject OtherObject;
		OtherObject = Other.gameObject;
		print (OtherObject.name);
	
		if (OtherObject.gameObject.tag == "Player")
		{
			PlayerController ControllerComponent = OtherObject.GetComponent<PlayerController> ();
			PlayerCharacter PlayerComponent = ControllerComponent.Character.GetComponent<PlayerCharacter> ();
			print (PlayerComponent.Inventory.Length);

			PlayerComponent.Inventory.AddItem (gameObject);
		}
	}
}
