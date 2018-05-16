using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_Blueprint
{
	public UI_Blueprint()
	{
		Name = "";
		Item = int.MinValue;
		Materials = new Dictionary<int,int> ();
	}

	public UI_Blueprint(string name,int item)
	{
		Name = name;
		Item = item;
		Materials = new Dictionary<int,int> ();
	}

	public string Name;
	public int Item;
	public Dictionary<int,int> Materials;
}


