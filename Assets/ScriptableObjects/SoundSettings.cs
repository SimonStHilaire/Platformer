using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSettings", menuName = "ScriptableObjects/New SoundSettings", order = 2)]
public class SoundSettings : ScriptableObject
{
    public List<AudioClip> Clips;
}
