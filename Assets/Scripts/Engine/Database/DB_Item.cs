using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items",menuName = "DataBase/Item",order = 3)]
public class DB_Item : Database
{
	public List<Item> Items = new List<Item>();
}
