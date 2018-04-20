using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EGender
{
	Male = -1,
	Female = 1,
	Other = 0,
}

[System.Serializable]
public class Stats 
{
	public Stats()
	{
		MinMana = 0;
		MinHealth = 0;
		Health = 100;
		MaxHealth = Health;
		Mana = 100;
		MaxMana = Mana;
	}

	public Stats(int HP,int MP)
	{
		MinMana = 0;
		MinHealth = 0;
		Health = HP;
		MaxHealth = Health;
		Mana = MP;
		MaxMana = Mana;
	}

	public int Health,MinHealth,MaxHealth;
	public int Mana, MinMana, MaxMana;

	public bool IsDead;

	public void Update()
	{
		Health = Mathf.Clamp (Health, MinHealth, MaxHealth);
		IsDead = (Health > 0);
	}
}
