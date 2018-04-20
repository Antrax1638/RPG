using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RagdollWindow : EditorWindow 
{
	GameObject SelectedObject;
	string RootName,ObjectName;
	bool Expand,FingerBones;
	GameObject RootBone;
	GameObject[] Bones;
	Vector2 ScroolPos = Vector2.zero;
	float CapsuleRadius = 0.005f,CapsuleHeigth = 0.03f;
	int HandBoneIndex;

	[MenuItem("Window/RagdollWindow")]
	static void Init()
	{
		RagdollWindow NewWindow = (RagdollWindow)GetWindow (typeof(RagdollWindow));
		NewWindow.Show ();
		NewWindow.name = "Ragdoll";
	}

	void OnGUI()
	{
		Color DefaultColor = GUI.backgroundColor;
		GUILayout.Space (10);
		GUILayout.Label ("Ragdoll Character Mesh Window:");

		SelectedObject = Selection.activeGameObject;
		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (SelectedObject) 
		{
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Space (5);
			EditorGUILayout.BeginVertical ("Box",GUILayout.MaxWidth(700),GUILayout.MaxHeight(300));
			ObjectName = (SelectedObject) ? SelectedObject.name : "Not Object Selected";
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label ("["+ObjectName+"]");
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
		
			RootName = EditorGUILayout.TextField ("Root Bone Name", RootName);
			CapsuleRadius = EditorGUILayout.FloatField ("Capsule Radius", CapsuleRadius);
			CapsuleHeigth = EditorGUILayout.FloatField ("Capsule Height", CapsuleHeigth);
			HandBoneIndex = EditorGUILayout.IntField ("Hand Bone Index", HandBoneIndex);

			FingerBones = EditorGUILayout.Toggle ("Finger Bones [30]", FingerBones);

			EditorGUILayout.BeginVertical ("Button");
			Expand = EditorGUILayout.Foldout (Expand, "Expand/Hide Bone List",true);
			if (Expand) 
			{
				int InnerSpace = 25;
				Rigidbody RigidBodyTemp = null;
				Collider ColliderTemp = null;
				Color OldColor = GUI.backgroundColor;

				ScroolPos = EditorGUILayout.BeginScrollView (ScroolPos,GUILayout.MaxWidth(600),GUILayout.MaxHeight(150));
				if (Bones != null) 
				{
					for (int i = 0; i < Bones.Length; i++) 
					{
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (InnerSpace);

						RigidBodyTemp = Bones[i].GetComponent<Rigidbody>();
						ColliderTemp = Bones [i].GetComponent<Collider> ();
						if(RigidBodyTemp && ColliderTemp)
						{
							if(GUILayout.Button("-"))
								RemoveAt (i);
							GUI.backgroundColor = Color.green;
						}
						else
						{
							if(GUILayout.Button("+"))
								AddAt (i);
							GUI.backgroundColor = Color.red;
						}

						Bones [i] = (GameObject)EditorGUILayout.ObjectField (Bones [i].name, Bones [i], typeof(GameObject), true);
						GUI.backgroundColor = OldColor;

						GUILayout.Space (InnerSpace);
						EditorGUILayout.EndHorizontal ();
					}
				}
				EditorGUILayout.EndScrollView();
				
			}
			EditorGUILayout.EndVertical ();


			GUILayout.Space (10);
			if (GUILayout.Button ("Refresh"))
				UpdateBones ();
			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("Clear"))
				Clear();
			GUI.backgroundColor = DefaultColor;
			EditorGUILayout.BeginHorizontal ();
			//GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Add Ragdoll")) 
			{
				AddRagdoll ();
			}
			if (GUILayout.Button ("Remove Ragdoll")) 
			{
				RemoveRagdoll ();
			}
			//GUILayout.FlexibleSpace ();
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndHorizontal ();
		}
		GUILayout.FlexibleSpace ();
		EditorGUILayout.EndHorizontal ();
	}

	void AddRagdoll()
	{
		for (int i = 0; i < Bones.Length; i++)
		{
			if (i > (Bones.Length-31) && FingerBones)
				break;

			Bones [i].AddComponent (typeof(Rigidbody));
			Bones [i].GetComponent<Rigidbody> ().isKinematic = true;
			Bones [i].AddComponent (typeof(CapsuleCollider));
			Bones [i].GetComponent<CapsuleCollider> ().radius = CapsuleRadius;
			Bones [i].GetComponent<CapsuleCollider> ().direction = 1;
			Bones [i].GetComponent<CapsuleCollider> ().height = CapsuleHeigth;
		}
	}

	void AddAt(int index)
	{
		if (index >= 0 && index < Bones.Length)
		{
			Bones [index].AddComponent (typeof(Rigidbody));
			Bones [index].GetComponent<Rigidbody> ().isKinematic = true;
			Bones [index].AddComponent (typeof(CapsuleCollider));
			Bones [index].GetComponent<CapsuleCollider> ().radius = CapsuleRadius;
			Bones [index].GetComponent<CapsuleCollider> ().direction = 1;
			Bones [index].GetComponent<CapsuleCollider> ().height = CapsuleHeigth;
		}
	}

	void RemoveRagdoll()
	{
		for (int i = 0; i < Bones.Length; i++)
		{
			DestroyImmediate (Bones[i].GetComponent<Rigidbody>());
			DestroyImmediate (Bones[i].GetComponent<Collider> ());
		}
	}

	void RemoveAt(int index)
	{
		if (index >= 0 && index < Bones.Length)
		{
			DestroyImmediate( Bones[index].GetComponent<Rigidbody> ());
			DestroyImmediate( Bones[index].GetComponent<CapsuleCollider> ());
		}
	}
		
	void UpdateBones()
	{
		SkinnedMeshRenderer MeshRenderer = null;
		var Smr = SelectedObject.GetComponentsInChildren<SkinnedMeshRenderer> ();
		foreach (SkinnedMeshRenderer SMR in Smr)
		{
			if (SMR.rootBone.name == RootName)
				MeshRenderer = SMR;
			else
				MeshRenderer = null;
		}
		if (MeshRenderer) 
		{
			RootBone = MeshRenderer.rootBone.gameObject;
			Bones = new GameObject[MeshRenderer.bones.Length];
			for (int i = 0; i < Bones.Length; i++)
			{
				Bones [i] = MeshRenderer.bones [i].gameObject;
			}
		}
	}

	void Clear()
	{
		SelectedObject = null;
		RootBone = null;
		Bones = new GameObject[0];
	}
}
