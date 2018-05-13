using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Blueprints",menuName = "Database/Blueprint",order = 3)]
public class BlueprintDatabase : ScriptableObject 
{
	public List<UI_Blueprint> Blueprints = new List<UI_Blueprint>();
}

[CustomEditor(typeof(BlueprintDatabase))]
public class BlueprintDatabaseEditor : Editor
{
	private BlueprintDatabase Target;
	bool ITab;
	int Selection = 0;

	void OnEnable()
	{
		Target = target as BlueprintDatabase;
	}

	public override void OnInspectorGUI ()
	{
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("<-")) Selection--;
		GUILayout.Label ("[" + Selection + "]");
		if (GUILayout.Button ("->")) Selection++;
		EditorGUILayout.EndHorizontal ();
		if (Target.Blueprints != null && Selection >= 0 && Selection < Target.Blueprints.Count)
		{
			Selection = Mathf.Clamp (Selection, 0, Target.Blueprints.Count);
			DrawBlueprint (Target.Blueprints [Selection]);
		}


	}

	void DrawBlueprint (UI_Blueprint Temp)
	{
		Temp.Name = EditorGUILayout.TextField ("Name", Temp.Name);
		ITab = EditorGUILayout.Foldout (ITab, "Item", true);
		if(ITab)
		{
			Temp.Item.Id = EditorGUILayout.IntField ("Id", Temp.Item.Id);
			Temp.Item.Size = EditorGUILayout.Vector2IntField ("Size", Temp.Item.Size);
			Temp.Item.Icon = (Sprite)EditorGUILayout.ObjectField ("Icon", Temp.Item.Icon, typeof(Sprite), true);
		}

		EditorGUILayout.BeginVertical ("Box");

		EditorGUILayout.EndVertical ();
	}
}