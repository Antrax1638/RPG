using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Database",menuName = "Stat/Database",order = 3)]
public class StatDatabase : ScriptableObject
{
	public Dictionary<string,int> IntValues = new Dictionary<string,int>();

}

[CustomEditor(typeof(StatDatabase))]
public class StatDatabaseEditor : Editor
{
	StatDatabase Target;
	bool IntFold;
	string KeyName = "New Key";
	int KeyValue = 0;

	void OnEnable()
	{
		Target = target as StatDatabase;
	}

	public override void OnInspectorGUI()
	{
		GUILayout.Space (10);
		IntValues ();

		EditorUtility.SetDirty (Target);
	}

	void IntValues()
	{
		EditorGUILayout.BeginVertical ("Button");
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (10);
		IntFold = EditorGUILayout.Foldout (IntFold, "Int Stats", true);
		GUI.skin.label.alignment = TextAnchor.MiddleRight;
		GUILayout.Label ("Length: [" + Target.IntValues.Count + "]");
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		EditorGUILayout.EndHorizontal ();
		if (IntFold) 
		{
			string[] Keys = Target.IntValues.Keys.ToArray ();

			for (int i = 0; i < Keys.Length; i++) 
			{
				EditorGUILayout.BeginHorizontal ();
				Target.IntValues [Keys [i]] = EditorGUILayout.IntField (Keys [i], Target.IntValues [Keys [i]]);
				if (GUILayout.Button ("-",GUILayout.MaxWidth(35)))
					Target.IntValues.Remove(Keys[i]);
				EditorGUILayout.EndHorizontal ();
			}

			EditorGUILayout.Space ();
			EditorGUILayout.BeginVertical ("Box");
			GUILayout.Label ("Panel:");
			EditorGUILayout.BeginHorizontal ();
			KeyName = EditorGUILayout.TextField(KeyName);
			KeyValue = EditorGUILayout.IntField (KeyValue);
			EditorGUILayout.EndHorizontal ();
			if (GUILayout.Button ("Add Key")) {
				Target.IntValues.Add (KeyName, KeyValue);
			}
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndVertical ();
		}
	}
}