using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int Level;
    public int DamageFactor;

    private SphereCollider ColliderComponent;

    private void Awake()
    {
        ColliderComponent = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        var Dam = GetComponentsInChildren<Damage>();
        for (int i = 0; i < Dam.Length; i++)
        {
            Dam[i].Level = Level;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            print("Hit P");
            PlayerCharacter Ch = other.GetComponent<PlayerController>().Character.GetComponent<PlayerCharacter>();
            Ch.Stat.Health -= (DamageFactor*Level);
            Ch.Stat.Health = Mathf.Clamp(Ch.Stat.Health,0, int.MaxValue);
        }
    }

}
