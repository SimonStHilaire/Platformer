using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class UpdateSoundSettings
{
    [MenuItem("Utils/Update Sound Settings")]
    public static void Update()
    {
        SoundSettings newSettings = ScriptableObject.CreateInstance<SoundSettings>();

        newSettings.Clips = new List<AudioClip>();

        string[] soundFiles = Directory.GetFiles("Assets/Audio/");
        
        foreach(string sound in soundFiles)
        {
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(sound);

            if(clip)
            {
                newSettings.Clips.Add(clip);
            }
        }

        AssetDatabase.CreateAsset(newSettings, "Assets/Resources/SoundSettings.asset");
    }
}
