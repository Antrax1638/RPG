using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("General")]
    public int Level;
    public Stats Stat;
    
    [Header("Audio")]
    public AudioClip[] SoundsList;
    public float MinSoundDelay = 3.25f;
    public float MaxSoundDelay = 5.38f;

    private AudioSource AudioComponent;
    private SphereCollider ColliderComponent;
    private float DeltaSoundTime;

	void Awake () {
        AudioComponent = GetComponentInChildren<AudioSource>();
        ColliderComponent = GetComponent<SphereCollider>();

        Stat.Health *= Level;
        Stat.MaxHealth *= Level;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Stat.IsDead = (Stat.Health <= 0);
        if (Stat.IsDead)
        {
            Destroy(gameObject);
        }

        DeltaSoundTime += Time.deltaTime;
        float RandomTime = Random.Range(MinSoundDelay, MaxSoundDelay); 
        if(DeltaSoundTime > RandomTime)
        {
            PlayRandomSoundFromList();
            DeltaSoundTime = 0.0f;
        }
	}

    void PlayRandomSoundFromList()
    {
        int RandomIndex = Random.Range(0, SoundsList.Length-1);
        AudioClip RandomClip = SoundsList[RandomIndex];
        AudioComponent.clip = RandomClip;
        AudioComponent.Play();
    }

}
