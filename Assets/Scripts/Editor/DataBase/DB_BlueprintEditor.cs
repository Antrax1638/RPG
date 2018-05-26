using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DB_Blueprint))]
public class DB_BlueprintEditor : Editor
{
	private DB_Blueprint Target;
	bool MTab;
	int Selection = 0,MaterialKey,MaterialValue;

	void OnEnable()
	{
		Target = target as DB_Blueprint;
	}

	public override void OnInspectorGUI ()
	{
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("<-")) Selection--;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label ("Blueprint [" + Selection + "]");
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		if (GUILayout.Button ("->")) Selection++;
		EditorGUILayout.EndHorizontal ();

		Selection = (Target.Blueprints != null) ? Mathf.Clamp (Selection, 0, Target.Blueprints.Count-1) : 0;
		if (Target.Blueprints != null && Selection >= 0 && Selection < Target.Blueprints.Count)
		{
			Selection = Mathf.Clamp (Selection, 0, Target.Blueprints.Count);

			Target.Blueprints[Selection].Name = EditorGUILayout.TextField ("Name", Target.Blueprints[Selection].Name);
			Target.Blueprints[Selection].Item = EditorGUILayout.IntField ("Item", Target.Blueprints[Selection].Item);
			DrawBlueprint (Target.Blueprints [Selection]);
		}

		EditorGUILayout.BeginHorizontal ("Box");
		if (GUILayout.Button ("Add Blueprint"))
			Target.Blueprints.Add (new UI_Blueprint ());

		if (GUILayout.Button ("Remove Blueprint"))
			Target.Blueprints.RemoveAt(Selection);

		EditorGUILayout.EndHorizontal ();
	}

	void DrawBlueprint (UI_Blueprint Temp)
	{
		EditorGUILayout.BeginVertical ("Box");

		EditorGUILayout.BeginHorizontal ();
		MTab = EditorGUILayout.Foldout (MTab, "Materials", true);
		int Length = (Temp.Materials != null) ? Temp.Materials.Count : -1; 
		GUI.skin.label.alignment = TextAnchor.UpperRight;
		GUILayout.Label ("Length ["+Length+"]");
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		EditorGUILayout.EndHorizontal ();
		GUI.contentColor = Color.green;
		if (MTab && Temp.Materials != null) 
		{
			List<int> Keys = new List<int> (Temp.Materials.Keys);
			for (int i = 0; i < Keys.Count; i++)
			{
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("Key: [" + Keys [i] + "]");
				Temp.Materials [Keys [i]] = EditorGUILayout.IntField ("Cuantity =", Temp.Materials [Keys [i]]);
				GUI.backgroundColor = Color.red;
				if (GUILayout.Button ("-"))
					Temp.Materials.Remove (Keys [i]);
				GUI.backgroundColor = Color.white;
				EditorGUILayout.EndHorizontal ();
			}
		}
		GUI.contentColor = Color.white;
		GUI.backgroundColor = Color.white;

		EditorGUILayout.Space ();

		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Key");
		GUILayout.Space (20); 
		GUILayout.Label ("Cuantity");
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();
		MaterialKey = EditorGUILayout.IntField (MaterialKey);
		MaterialValue = EditorGUILayout.IntField (MaterialValue);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (5);
		bool AddMaterialState = GUILayout.Button ("Add Material");
		bool RemoveMaterialState = GUILayout.Button ("Empty Materials");
		if (AddMaterialState && Temp.Materials != null && !Temp.Materials.ContainsKey (MaterialKey)) {
			Temp.Materials.Add (MaterialKey, MaterialValue);
		} else if (AddMaterialState && Temp.Materials != null && Temp.Materials.ContainsKey (MaterialKey)) {
			Debug.LogWarning ("Current Key exist in materials list for current blueprint");
		}
		if (RemoveMaterialState)
			Temp.Materials.Clear ();
		GUILayout.Space (5);
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();

		EditorGUILayout.EndVertical ();
	}
}