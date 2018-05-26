using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DB_Item))]
public class DB_ItemEditor : Editor
{
	private DB_Item Target;
	int Selected = 0;
	int ItemsPerPage = 15;

	void OnEnable()
	{
		Target = (DB_Item)target;
	}

	public override void OnInspectorGUI()
	{
		//base.DrawDefaultInspector ();

        bool Valid = Target.Items != null && Target.Items.Count >= 1;

        Selected = (Valid) ? Mathf.Clamp(Selected,0,(Target.Items.Count-1)/ItemsPerPage) : -1;

        ItemsPerPage = EditorGUILayout.IntField("Items Per Page", ItemsPerPage);
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("<-")) Selected--;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label ("Page [" + Selected + "]");
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		if (GUILayout.Button ("->")) Selected++;
		EditorGUILayout.EndHorizontal ();

		if (Valid)
		{
			Selected = Mathf.Clamp (Selected, 0, Target.Items.Count - 1);
			int Index = 0;
			EditorGUILayout.BeginVertical ("Box");
			for (int i = 0; i < ItemsPerPage; i++) {
				Index = i + (Selected * ItemsPerPage);
				EditorGUILayout.BeginHorizontal ();
				if (Index >= 0 && Index < Target.Items.Count)
				{
					Target.Items [Index] = (Item)EditorGUILayout.ObjectField ("Item", Target.Items [Index], typeof(Item), true);
					GUI.backgroundColor = Color.red;
					if (GUILayout.Button ("-"))
						Target.Items.RemoveAt (Index);
					GUI.backgroundColor = Color.white;
					
				}
                EditorGUILayout.EndHorizontal();
            }
			EditorGUILayout.EndVertical ();
		}

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
            Target.Items.Add(new Item());
        if (GUILayout.Button("Clear"))
            Target.Items.Clear();
        EditorGUILayout.EndHorizontal();
	
	}
}
