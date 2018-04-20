using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CEditor : Editor 
{
	public static void ProgressBar(float Value,string Label)
	{
		Rect rect = GUILayoutUtility.GetRect (18, 18, "TextField");
		EditorGUI.ProgressBar (rect, Value, Label);
		EditorGUILayout.Space();
	}

	public static void ProgressBar(float Value,float MaxValue,string Label)
	{
		ProgressBar (Value / MaxValue, Label);
	}
		
	public static void ArrayProperty(SerializedObject Object,string Name)
	{
		SerializedProperty Property = Object.FindProperty (Name);
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (Property,true);
		if (EditorGUI.EndChangeCheck ())
			Object.ApplyModifiedProperties ();
	}

	public static GUIContent ToolTip(string name,string tooltip = "")
	{
		return new GUIContent(name,tooltip);
	}

	public static Quaternion Vector4ToQuaternion(Vector4 Value)
	{
		return new Quaternion (Value.x, Value.y, Value.z, Value.w);
	}

	public static Vector4 QuaternionToVector4(Quaternion Rot)
	{
		return new Vector4(Rot.x,Rot.y,Rot.z,Rot.w);
	}

	public static string[] GetLayerMasks()
	{
		List<string> Temp = new List<string> ();
		for (int i = 0; i < 32; i++) 
		{
			if (LayerMask.LayerToName (i) != "")
				Temp.Add (LayerMask.LayerToName (i));
		}
		return Temp.ToArray ();
	}

}
