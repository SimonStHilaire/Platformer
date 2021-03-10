using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : SceneSingleton<TransitionManager>
{
    const string LOADING_SCENE_NAME = "loading";

    string CurrentLevelName = "";

    public Action<string> OnLevelLoaded;

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(LOADING_SCENE_NAME, LoadSceneMode.Additive);

        if (SceneManager.GetSceneByName(CurrentLevelName).isLoaded)
            SceneManager.UnloadSceneAsync(CurrentLevelName);

        CurrentLevelName = levelName;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(CurrentLevelName, LoadSceneMode.Additive);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != LOADING_SCENE_NAME)//Do not handle loading scene
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            SceneManager.UnloadSceneAsync(LOADING_SCENE_NAME);

            SceneManager.SetActiveScene(scene);
            OnLevelLoaded?.Invoke(scene.name);
        }
    }
}
