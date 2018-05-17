using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blueprints",menuName = "Database/Blueprint",order = 3)]
public class BlueprintDatabase : ScriptableObject 
{
	public List<UI_Blueprint> Blueprints = new List<UI_Blueprint>();
}
