using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Durability 
{
	[Header("Durability Properties:")]
	public float[] Materials;
	public float Multiplier = 1.0f;
	public float Additional = 0.0f;
	public Vector2 Range = new Vector2(0.0f,9999.0f);

	public float GetDurability()
	{
		float Total = 0;
		for (int i = 0; i < Materials.Length; i++) {
			Total += Materials [i];
		}
		Total /= Materials.Length;
		Total += Additional;
		Total *= Multiplier;
		return Mathf.Clamp(Total,Range.x,Range.y);
	}

	public bool AddMaterial(float Value)
	{
		float[] Temp = new float[Materials.Length];
		Materials.CopyTo (Temp,0);
		Materials = new float[Temp.Length+1];
		Temp.CopyTo (Materials,0);
		return (Materials.Length > Temp.Length);
 	}

	public bool RemoveMaterial (int Index)
	{
		List<float> Temp = new List<float> (Materials);
		Temp.RemoveAt (Index);
		Materials = new float[Temp.Count];
		Materials = Temp.ToArray ();
		return (Materials.Length < Temp.Count);
	}

	public void ClearMaterials()
	{
		Materials = new float[0];
	}
}
