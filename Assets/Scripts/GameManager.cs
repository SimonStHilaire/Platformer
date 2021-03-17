using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : SceneSingleton<GameManager>
{
    public GameSettings Settings;

    public PlayerController PlayerRef;
    public EnemyController EnemyRef;
    public AudioMixerGroup SFXMixer;

    public int LevelCount;

    const string LEVEL_NAME = "Level";
    const string MAINMENU_LEVEL_NAME = "mainmenu";
    const string DYNAMIC_LEVEL_NAME = "DynamicLevel";

    public bool SoundOn;

    PlayerController Player = null;

    bool IsPlaying = false;

    int CurrentLevelIndex = 0;//0 = invalid since we are 1 based here

    Level CurrentLevel;

    void Start()
    {
        AssetBundleManager.Instance.Initialize();

        TransitionManager.Instance.LoadLevel(MAINMENU_LEVEL_NAME);

        UpdateVolume();
    }

    void UpdateVolume()
    {
        if (SoundOn)
        {
            SFXMixer.audioMixer.SetFloat("Volume", 0f);
        }
        else
        {
            SFXMixer.audioMixer.SetFloat("Volume", -80f);
        }
    }

    public void StartGame()
    {
        LoadNextLevel();
    }

    public void StartCredits()
    {
        SceneManager.LoadScene("credits", LoadSceneMode.Additive);
    }

    void LoadNextLevel(int levelIndex = -1)
    {
        CurrentLevelIndex++;

        string nextSceneName = LEVEL_NAME + CurrentLevelIndex.ToString();

        if (CurrentLevelIndex > LevelCount)//TODO Win the game
        {
            if (AssetBundleManager.Instance.Exists(LEVEL_NAME + CurrentLevelIndex.ToString()))
            {
                nextSceneName = DYNAMIC_LEVEL_NAME;
            }
            else
            {
                CurrentLevelIndex = 1;
                nextSceneName = LEVEL_NAME + CurrentLevelIndex.ToString();
            }
        }

        TransitionManager.Instance.OnLevelLoaded += OnSceneLoaded;
        TransitionManager.Instance.LoadLevel(nextSceneName);
    }

    void OnSceneLoaded(string sceneName)
    {
        TransitionManager.Instance.OnLevelLoaded -= OnSceneLoaded;

        if(sceneName == DYNAMIC_LEVEL_NAME)
        {
            AssetBundleManager.Instance.LoadDynamicScene(LEVEL_NAME + CurrentLevelIndex.ToString());
        }
       
        CurrentLevel = FindObjectOfType<Level>();

        if (!CurrentLevel)
        {
            Debug.LogError("No level found");
        }

        if (Player == null)
        {
            Player = Instantiate(PlayerRef, CurrentLevel.PlayerSpawnPosition.position, Quaternion.identity, null);
        }
        else
        {
            Player.transform.position = CurrentLevel.PlayerSpawnPosition.position;
        }

        CameraController cameraController = Camera.main.GetComponent<CameraController>();

        if(cameraController)
            cameraController.Player = Player.transform;

        Player.OnEnemyCollison += OnPlayerTouchEnemy;

        IsPlaying = true;
    }

    void OnPlayerTouchEnemy()
    {
        IsPlaying = false;

        Destroy(Player.gameObject);

        CurrentLevelIndex -= 1;

        SoundController.Instance.playAudio("Death", GetComponent<AudioSource>());

        LoadNextLevel();
    }

    public void OnGameExitTouched()
    {
        LoadNextLevel();
    }

    void Update()
    {
        if (!IsPlaying)
            return;

        if(Input.GetKeyDown(KeyCode.P))
        {
            SoundOn = !SoundOn;
            UpdateVolume();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNextLevel();
        }

    }
}
