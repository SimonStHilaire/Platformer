﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
{
    private static T m_Instance = null;
    public static T Instance {
        get {
            if (m_Instance == null) {
                m_Instance = FindObjectOfType<T>();
                if (m_Instance == null) {
                    m_Instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }

#if !UNITY_EDITOR
                DontDestroyOnLoad(m_Instance.gameObject);
#endif
            }
            return m_Instance;
        }
    }
}