using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public AudioClip Jump;
    public AudioClip Shoot;
    public AudioClip Death;

    private AudioClip getAudio(string name)
    {
        switch (name)
        {
            case "Jump":
                return Jump;
            case "Shoot":
                return Shoot;
            case "Death":
                return Death;
        }
        return null;
    }

    public void playAudio(string audioName, AudioSource audioSource)
    {
        audioSource.clip = getAudio(audioName);
        audioSource.Play();
    }

}
