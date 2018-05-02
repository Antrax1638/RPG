using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
	public enum SearchType
	{
		OneDimensional,
		TwoDimensional,
	}

	public static Vector2Int InvalidIndex = new Vector2Int(-1,-1);
	public const int Null = 0;

	[Header("Inventory Properties:")]
	public bool Symetric;
	public GameObject Slot;
	public bool SlotSize;
	public Vector2Int Size;
	public int Length {
		get { return Size.x * Size.y; }
	}
	public bool Safe;

	[Header("Debug Properties:")]
	public bool DebugMode = false;
	public Sprite DebugIcon;
	public int DebugId;
	public Vector2Int DebugSize;
	public Vector2Int DebugPos;


	GameObject GridPanel,SlotTemp;
	private RectTransform TransformComponent;
	private RectTransform GridPanelTransformComponent;
	private GridLayoutGroup GridPanelComponent;
	private UnSymmetricalGridLayoutGroup UnSymGridPanelComponent;

	protected GameObject[,] GridCells;
	//protected Dictionary<int,int> GridCellData = new Dictionary<int, int>();

	int Width,Height;
	UI_Item Temp = new UI_Item();

	void Awake() 
	{
		if (transform.childCount > 0) 
			GridPanel = transform.GetChild (0).gameObject;
		else
			Debug.LogError("UI_Inventory: Grid panel is null.");

		TransformComponent = GetComponent<RectTransform> ();
		if (!TransformComponent)
			Debug.LogError ("UI_Inventory: Transform component is null.");

		if (Symetric)
		{
			GridPanelComponent = GridPanel.GetComponent<GridLayoutGroup> ();
			if (!GridPanelComponent)
				Debug.LogError ("UI_Inventory: grid panel component is null.");

			RectTransform SlotTransform = Slot.GetComponent<RectTransform> ();
			GridPanelComponent.cellSize = (SlotSize) ? SlotTransform.sizeDelta : GridPanelComponent.cellSize;

			GridCells = new GameObject[Size.x, Size.y];
			Width = (int)GridPanelComponent.cellSize.x * Size.x;
			Height = (int)GridPanelComponent.cellSize.y * Size.y;

		} 
		else
		{
			UnSymGridPanelComponent = GridPanel.GetComponent<UnSymmetricalGridLayoutGroup> ();
			if (!UnSymGridPanelComponent)
				Debug.LogError ("UI_Inventory: grid panel component is null.");

			RectTransform SlotTransform = Slot.GetComponent<RectTransform> ();
			UnSymGridPanelComponent.CellSize = (SlotSize) ? SlotTransform.sizeDelta : UnSymGridPanelComponent.CellSize;

			GridCells = new GameObject[Size.x, Size.y];
			Width = (int)UnSymGridPanelComponent.CellSize.x * Size.x;
			Height = (int)UnSymGridPanelComponent.CellSize.y * Size.y;

			UnSymGridPanelComponent.MaxFillWidth = Size.x;
			UnSymGridPanelComponent.MaxFillHeight = Size.y;
			UnSymGridPanelComponent.LockCellSize = false;
		}

		GridPanelTransformComponent =  GridPanel.GetComponent<RectTransform> ();
		if (!GridPanelTransformComponent)
			Debug.LogError ("UI_Inventory: grid panel transform component is null.");

		TransformComponent.sizeDelta = new Vector2 (Width,Height);
		GridPanelTransformComponent.sizeDelta = new Vector2 (Width,Height);

		//Create Grid:
		UI_InventorySlot SlotComponent;
		for (int y = 0; y < GridCells.GetLength (1); y++)
		{
			for (int x = 0; x < GridCells.GetLength (0); x++)
			{
				GridCells [x, y] = Instantiate (Slot, GridPanel.transform);
				GridCells [x, y].name = "Slot [" + x + "-" + y + "]";
				SlotComponent = GridCells [x, y].GetComponent<UI_InventorySlot> ();
				SlotComponent.Position = new Vector2Int (x, y);
				SlotComponent.Item = UI_Item.invalid;
			}
		}
	}

	void Update () 
	{
		
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			Temp = new UI_Item(DebugId,DebugIcon,DebugSize);
			AddItem(Temp,DebugPos);
		}

		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			//GetDataMap ();
		}

		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			print(RemoveItem (DebugPos));
		}
	}

	/*protected int GetSlotData(int InstanceID)
	{
		List<int> Keys = new List<int> (GridCellData.Keys);
		for(int i = 0; i < Keys.Count; i++)
		{
			if(Keys[i] == InstanceID)
				return GridCellData [Keys [i]];
		}
		return 0;
	}*/

	void DebugLog(string info)
	{
		if(DebugMode)
			Debug.Log (info);
	}

	/*int[,] GetDataMap()
	{
		int[,] Map = new int[GridCells.GetLength(0),GridCells.GetLength(1)];
		for (int y = 0; y < GridCells.GetLength (1); y++)
		{
			for (int x = 0; x < GridCells.GetLength (0); x++)
			{
				Map [x, y] = GridCellData [GridCells [x, y].GetInstanceID ()];
				DebugLog ("Map:" + x + y + ": " + Map [x, y]);
			}
		}
		return Map;
	}*/
		
	//Publicas:
	public Vector2Int AddItem(UI_Item NewItem,Vector2Int Position)
	{
		if (NewItem == UI_Item.invalid)
		{
			DebugLog ("Invalid inventory item.");
			return UI_Inventory.InvalidIndex;
		}
			
		bool ValidIndex = false, ValidSlot = true;
		Vector2Int Index = UI_Inventory.InvalidIndex;

		int ItemWidth = Position.x + NewItem.Size.x;
		int ItemHeight = Position.y + NewItem.Size.y;

		ValidIndex = (Position.x >= 0 && Position.y >= 0) && (ItemWidth-1 < Size.x) && (ItemHeight-1 < Size.y);
		//DebugLog (ItemWidth + "-" + ItemHeight);
		/*ValidIndex &= (ItemWidth < Size.x);
		ValidIndex &= (ItemHeight < Size.y);*/

		List<int> HierarchyIndex = new List<int> ();
		List<GameObject> DeactivatedSlots = new List<GameObject> ();

		bool PointerValid;
		UI_InventorySlot Slot;
		for(int y = Position.y; y < ItemHeight; y++)
		{
			for (int x = Position.x; x < ItemWidth; x++) 
			{
				PointerValid = (x < GridCells.GetLength(0)) && (y < GridCells.GetLength(1));
				if (PointerValid)
				{
					Slot = GridCells [x, y].GetComponent<UI_InventorySlot> ();
					if (Slot && Slot.Item == UI_Item.invalid) {
						ValidSlot &= true;
						Slot.SetVisibility (Visiblility.Hidden);
						Slot.Item = NewItem;
						DeactivatedSlots.Add (Slot.gameObject);
						HierarchyIndex.Add (Slot.transform.GetSiblingIndex ());
					} else {
						ValidSlot &= false;
					}
				} 
				else
					ValidSlot &= false;
			}
		}
			
		if (ValidIndex && ValidSlot)
		{
			DebugLog ("Item Added: to [" + Position.x + "-" + Position.y + "] Id: [" + NewItem.Id + "]");
			int MaxIndex = Mathf.Max (HierarchyIndex.ToArray ());
			Slot = GridCells [Position.x, Position.y].GetComponent<UI_InventorySlot> ();
			Slot.SetVisibility (Visiblility.Visible);
			Slot.SetIcon (NewItem.Icon);
			Slot.SetScale (NewItem.Size);
			Slot.Item = NewItem;
			Slot.transform.SetSiblingIndex (MaxIndex);
			Index = Position;
		}
		else 
		{
			DebugLog ("inventory space unavailable.");
			for (int i = 0; i < DeactivatedSlots.Count; i++)
			{
				Slot = DeactivatedSlots [i].GetComponent<UI_InventorySlot> ();
				if (Slot && Slot.Item == NewItem || Slot.Item == UI_Item.invalid) 
				{
					Slot.SetVisibility (Visiblility.Visible);
					Slot.Item = UI_Item.invalid;
					Slot.SetIcon (null);
					Slot.SetScale (new Vector2 (1, 1));
				}
			}
			DeactivatedSlots.Clear ();
		}

		return Index;
	}

	public bool RemoveItem(Vector2Int Position)
	{
		UI_InventorySlot Slot = GridCells [Position.x, Position.y].GetComponent<UI_InventorySlot> ();
		if (Slot.Item == UI_Item.invalid)
			return false;

		bool Success = false;
		int OffsetWidth = Position.x + Slot.Item.Size.x;
		int OffsetHeight = Position.y + Slot.Item.Size.y;

		Slot = GridCells [Position.x,Position.y].GetComponent<UI_InventorySlot> ();
		if (Slot && Slot.Item != UI_Item.invalid) 
		{
			Slot.SetIcon (null);
			Slot.SetScale (new Vector2Int (1, 1));
			Slot.Item = UI_Item.invalid;
			Slot.SetVisibility (Visiblility.Visible);
			Success = true;
		}

		for (int x = Position.x; x < OffsetWidth; x++)
		{
			for(int y = Position.y; y < OffsetHeight; y++)
			{
				Slot = GridCells [x, y].GetComponent<UI_InventorySlot> ();
				if (Slot) 
				{
					Slot.SetIcon (null);
					Slot.SetScale (new Vector2Int (1, 1));
					Slot.Item = UI_Item.invalid;
					Slot.SetVisibility (Visiblility.Visible);
					Success &= true;
				} 
			}
		}

		return Success;
	}

	public bool RemoveItem(UI_Item Item)
	{
		return false;
		/*bool Valid = false;
		UI_Slot Slot;
		for (int y = 0; y < GridCells.GetLength (1); y++)
		{
			for(int x = 0; x < GridCells.GetLength(0); x++)
			{
				Slot = GridCells [x, y].GetComponent<UI_Slot> ();
				if(GridCellData [GridCells [x, y].GetInstanceID ()] == Item.Id)
				{
					GridCellData [GridCells [x, y].GetInstanceID ()] = 0;
					Slot.SetVisibility (Visiblility.Visible);
				}

				if(Slot.GetIcon() == Item.Icon)
				{
					GridCellData [GridCells [x, y].GetInstanceID ()] = 0;
					Slot.AdjustSize = true;
					Slot.SetIcon (null);
					Slot.SetScale (new Vector2 (1, 1));
					Valid = true;
				}
			}
		}

		return Valid;*/
	}

	public void ClearItems()
	{
		UI_InventorySlot Slot;
		for (int y = 0; y < GridCells.GetLength (1); y++)
		{
			for(int x = 0; x < GridCells.GetLength(0); x++)
			{
				Slot = GridCells [x, y].GetComponent<UI_InventorySlot> ();
				//GridCellData [GridCells [x, y].GetInstanceID ()] = 0;
				Slot.SetVisibility (Visiblility.Visible);
				Slot.AdjustSize = true;
				Slot.SetIcon (null);
				Slot.SetScale (new Vector2 (1, 1));
				Slot.Item = UI_Item.invalid;
			}
		}
	}

}
