using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundmanager : MonoBehaviour
{
    public AudioClip[] PunchSound;
    public AudioClip JumpStartSound;
    public AudioClip JumpEndSound;
    public AudioClip DieSound;
    public AudioClip _randomSound;

    private AudioSource audioSource;

    void Start()
    {
        // Získání audio zdroje komponenty
        audioSource = GetComponent<AudioSource>();
    }

    public void Punch()
    {
        int randomSound = Random.Range(0, PunchSound.Length);
        audioSource.PlayOneShot(PunchSound[randomSound]);
        Debug.Log("soundPlay");
    }
    public void JumpStart()
    {
        audioSource.PlayOneShot(JumpStartSound);

    }
    public void JumpEnd()
    {
        audioSource.PlayOneShot(JumpEndSound);

    }
    public void DiePlay()
    {
        audioSource.PlayOneShot(DieSound);

    }
    public void RandomSoundPlay()
    {
        audioSource.PlayOneShot(_randomSound);

    }
}
