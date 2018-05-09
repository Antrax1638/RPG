using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UI_Crafting : UI_Window 
{
	[Header("Crafting Properties:")]

	[Tooltip("the size of the container of all ui objects list.")]
	public int Size;
	[Tooltip("The ui object to create in the container list.")]
	public GameObject Prefab;
	public bool Changed;

	private Scrollbar[] ScrollBarComponents;
	private ScrollRect ScrollRectComponent;
	private GameObject Content;

	GameObject[] ContentContainer;
	Expandable ContentBase;

	protected override void Awake()
	{
		base.Awake ();

		ScrollBarComponents = GetComponentsInChildren<Scrollbar>();
		if (ScrollBarComponents == null)
			Debug.LogError ("UI_Crafting: scrollbar components are null");

		ScrollRectComponent = GetComponentInChildren<ScrollRect> ();
		if (!ScrollRectComponent)
			Debug.LogError ("UI_Crafting: scrollrect component is null");

		Content = GetChild ("Content").gameObject;
	}

	void Start()
	{
		RectTransform CurrentTransform;
		for(int i = 0; i < Size; i++)
		{
			GameObject CurrentObject = Instantiate (Prefab, Content.transform);
			CurrentTransform = CurrentObject.GetComponent<RectTransform> ();
			CurrentTransform.anchoredPosition = Vector2.zero;
			AddContainerObject (CurrentObject);
		}
	}

	protected override void Update()
	{
		base.Update ();
	
		if(Changed)
		{
			//Toggle TransformComponent;
			Vector2 SizeDelta = Vector2.zero;
			RectTransform LastTransform,Current;
			Current = ContentContainer [0].GetComponent<RectTransform> ();
			LastTransform = Current;
			for(int i = 1; i < ContentContainer.Length;i++)
			{
				Current = ContentContainer [i].GetComponent<RectTransform> ();
				Vector2 NewPosition = new Vector2 (
					LastTransform.anchoredPosition.x,
					-i * LastTransform.sizeDelta.y
				);
				SizeDelta.x += Current.sizeDelta.x * System.Convert.ToInt32(i == 1);
				SizeDelta.y += Current.sizeDelta.y;
				Current.anchoredPosition = NewPosition;
				LastTransform = Current;
			}

			Content.GetComponent<RectTransform> ().sizeDelta = SizeDelta;

			Changed = false;
		}
	}

	Transform GetChild(string name)
	{
		Transform[] AllChilds = GetComponentsInChildren<Transform> ();
		for (int i = 0; i < AllChilds.Length; i++) {
			if (AllChilds [i].name == name)
				return AllChilds [i];
		}
		return null;
	}

	//Funciones Publicas:
	public int AddContainerObject(GameObject Object,int Index)
	{
		if(Index >= 0 && Index < ContentContainer.Length && Object != null)
		{
			List<GameObject> Temp = new List<GameObject> (ContentContainer);
			Temp.Insert (Index, Object);
			ContentContainer = Temp.ToArray ();
			Index = (Temp.Count > ContentContainer.Length) ? Index : -1;
			return Index;
		}
		return -1;
	}

	public int AddContainerObject(GameObject Object)
	{
		if (ContentContainer == null) 
		{
			if (Object == null)
				return -1;

			ContentContainer = new GameObject[1];
			ContentContainer [0] = Object;
			return 0;
		}
		else
		{
			if(Object != null)
			{
				int Index = -1;
				GameObject[] Temp = new GameObject[ContentContainer.Length];
				for (int i = 0; i < ContentContainer.Length; i++){
					Temp [i] = ContentContainer [i];
				}
				ContentContainer = new GameObject[Temp.Length + 1];
				for (int i = 0; i < ContentContainer.Length; i++){
					if(i >= 0 && i < ContentContainer.Length && i < Temp.Length)
						ContentContainer [i] = Temp [i];
				}
				Index = Temp.Length;
				ContentContainer [Index] = Object;
				Temp = new GameObject[0];
				return Index;
			}
		}

		return -1;
	}

	public bool RemoveContainerObject(GameObject Object)
	{
		ArrayUtility.Remove (ref ContentContainer, Object);
		return true;
	}

	public bool RemoveContainerObject(int index)
	{
		if (index < 0 || index >= ContentContainer.Length)
			return false;
		
		ArrayUtility.RemoveAt (ref ContentContainer, index);
		return true;
	}

	public void EmptyContainer()
	{
		int OldLength = ContentContainer.Length;
		ContentContainer = new GameObject[OldLength];
	}

}
