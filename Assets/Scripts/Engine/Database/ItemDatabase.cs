using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items",menuName = "Database/Items",order = 3)]
public class ItemDatabase : ScriptableObject
{
	public List<Item> Items = new List<Item>();
}
