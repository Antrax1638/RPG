using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UnSymmetricalGridLayoutGroup : MonoBehaviour 
{
	public enum CellSizeMode
	{
		FirstElement,
		CellSize,
	}

	[Header("Panel Properties:")]
	public CellSizeMode CellMode = CellSizeMode.CellSize;
	public Vector2 CellSize = Vector2.zero;
	[Tooltip("all childs objects transforms size are locked or not.")]
	public bool LockCellSize = false;
	public GridLayoutGroup.Corner StartCorner = GridLayoutGroup.Corner.UpperLeft;

	[Tooltip("all childs objects are deleted if this option is enabled")]
	public bool ClampFill = true;
	public int MaxFillWidth = 0;
	public int MaxFillHeight = 0;

	protected GameObject[,] GridObjects;
	protected RectTransform[] GridTransforms;
	private RectTransform TransformComponent;

	void Awake ()
	{
		TransformComponent = GetComponent<RectTransform> ();
		if (!TransformComponent)
			Debug.LogError ("UnSymmetricalGridLayout: Transform component is null");
	}

	void Start()
	{
		RefreshGrid ();
	}

	public void RefreshGrid()
	{
		int ChildSize = TransformComponent.childCount;
		GridObjects = new GameObject[MaxFillWidth,MaxFillHeight];
		RectTransform ChildTransform;
		Vector2 ChildPos = Vector2.zero;
		Vector2 GridSize = Vector2.zero;

		TransformComponent.sizeDelta = new Vector2 (CellSize.x * MaxFillWidth,CellSize.y * MaxFillHeight);
		GridTransforms = new RectTransform[MaxFillWidth * MaxFillHeight];

		switch (CellMode)
		{
		case CellSizeMode.CellSize: 
			GridSize = CellSize; 
			break;
		case CellSizeMode.FirstElement:
			ChildTransform = TransformComponent.GetChild (0).GetComponent<RectTransform> ();
			if (ChildTransform)
				GridSize = ChildTransform.sizeDelta;
			break;
		}

		int Corner = 0;
		int CornerRight = 0;
		switch(StartCorner)
		{
			case GridLayoutGroup.Corner.LowerLeft:  Corner = 1;  CornerRight = 0; break;
			case GridLayoutGroup.Corner.UpperLeft:  Corner = -1; CornerRight = 0; break;
			case GridLayoutGroup.Corner.LowerRight: Corner = 1;  CornerRight = 1; break;
			case GridLayoutGroup.Corner.UpperRight: Corner = -1; CornerRight = 1; break;
		}

		for (int i = 0; i < ChildSize; i++)
		{
			Vector2Int Position = UI_Manager.Instance.ToCoord (i, MaxFillWidth);
			bool ValidIndex = (i >= 0 && i < MaxFillWidth*MaxFillHeight);
			if (Position.x < MaxFillWidth && Position.y < MaxFillHeight & ValidIndex)
			{
				GridObjects [Position.x, Position.y] = TransformComponent.GetChild (i).gameObject;
				ChildTransform = GridObjects [Position.x, Position.y].GetComponent<RectTransform> ();
				GridTransforms [i] = ChildTransform;

				if(CornerRight == 1)
					ChildPos.x = ((MaxFillWidth - 1) * GridSize.x) - (Position.x * GridSize.x);
				else
					ChildPos.x = (Position.x * GridSize.x);

				if (Corner == 1)
					ChildPos.y = (Position.y * GridSize.y) - ((MaxFillHeight - 1) * GridSize.y);
				else
					ChildPos.y = (Position.y * GridSize.y) * Corner;

				ChildTransform.anchoredPosition = ChildPos;
			} 
			else if (ClampFill)
			{
				Destroy (TransformComponent.GetChild (i).gameObject);
			}
		}

	}

	void Update ()
	{
		if (LockCellSize) 
		{
			for (int i = 0; i < GridTransforms.Length; i++)
			{
				GridTransforms [i].sizeDelta = CellSize;
			}
		}	
	}

	public RectTransform[] GetGridTransforms()
	{
		return GridTransforms;
	}

	public GameObject[,] GetGridObjects()
	{
		return GridObjects;
	}
}
