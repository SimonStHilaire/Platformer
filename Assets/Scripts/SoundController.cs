using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : SceneSingleton<SoundController>
{
    Dictionary<string, AudioClip> Sounds = new Dictionary<string, AudioClip>();

    SoundSettings Settings;

    private void Start()
    {
        foreach(AudioClip clip in Settings.Clips)
        {
            Sounds[clip.name] = clip;
        }
    }

    private AudioClip getAudio(string name)
    {
        if(Sounds.ContainsKey(name))
            return Sounds[name];

        return null;
    }

    public void playAudio(string audioName, AudioSource audioSource)
    {
        AudioClip clip = getAudio(audioName);

        if (clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

}
