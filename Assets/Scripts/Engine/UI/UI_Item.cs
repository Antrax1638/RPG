﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UI_Item
{
	public static UI_Item invalid = new UI_Item (int.MinValue, null, new Vector2Int (-1, -1));

	public int Id;
	public Sprite Icon;
	public Vector2Int Size;

	public UI_Item()
	{
		Id = int.MinValue;
		Icon = null;
		Size = new Vector2Int (-1, -1);
	}

	public UI_Item(int id,Sprite icon,Vector2Int size)
	{
		Id = id;
		Icon = icon;
		Size = size;
	}

	public static bool operator==(UI_Item x,UI_Item y)
	{
		return (x.Id == y.Id) && (x.Size == y.Size) && (x.Icon == y.Icon);
	}

	public static bool operator!=(UI_Item x,UI_Item y)
	{
		return (x.Id != y.Id) && (x.Size != y.Size) && (x.Icon != y.Icon);
	}

	public override bool Equals(object other)
	{
		return this == (UI_Item)other;
	}
}