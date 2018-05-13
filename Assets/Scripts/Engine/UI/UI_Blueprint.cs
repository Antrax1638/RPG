using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_Blueprint
{
	public string Name;
	public UI_Item Item = UI_Item.invalid;
	public Dictionary<UI_Item,int> Materials;
}


