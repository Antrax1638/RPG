using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blueprints",menuName = "DataBase/Blueprint",order = 3)]
public class DB_Blueprint : Database 
{
	public List<UI_Blueprint> Blueprints = new List<UI_Blueprint>();
}
