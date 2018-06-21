using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetive : MonoBehaviour
{

    public GameObject Portal;
    int LeftToKill = 0;

    GameObject[] List;
	
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        List = GameObject.FindGameObjectsWithTag("Zombie");
        LeftToKill = List.Length;

        if (LeftToKill <= 0) {
            Portal.SetActive(true);
        }
	}
}
