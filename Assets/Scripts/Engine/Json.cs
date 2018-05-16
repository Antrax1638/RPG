using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Json
{
	[HideInInspector] public JsonData Data;
	private string DataString;
	protected string Filename;

	public void SaveToFile(string Filename,bool UsePath = true)
	{
		this.Filename = Filename;
		string Path = (UsePath) ? Application.dataPath + "\\" : "";
		File.WriteAllText (Path + Filename, DataString);
	}
		
	public void LoadFromFile(string Filename,bool UsePath = true)
	{
		this.Filename = Filename;
		string Path = (UsePath) ? Application.dataPath + "\\" : "";
		DataString = File.ReadAllText (Path + Filename);
		Data = JsonMapper.ToObject (DataString);
	}
}
