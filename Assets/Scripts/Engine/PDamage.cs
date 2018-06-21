using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDamage : MonoBehaviour
{
    public int Level;
    public int DamageFactor;

    private BoxCollider ColliderComponent;

    void Awake()
    {
        ColliderComponent = GetComponent<BoxCollider>();
    }

    void Update()
    {
        var Dam = GetComponentsInChildren<Damage>();
        for (int i = 0; i < Dam.Length; i++)
        {
            Dam[i].Level = Level;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print("Enter"+other.name);
        if (other.tag == "Zombie")
        {
            print("Hit Z");
            Zombie Zom = other.GetComponent<Zombie>();
            Zom.Stat.Health -= (DamageFactor * Level);
            Zom.Stat.Health = Mathf.Clamp(Zom.Stat.Health, 0, int.MaxValue);
        }
    }

}
