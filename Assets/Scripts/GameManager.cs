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

    public bool SoundOn;

    PlayerController Player = null;

    bool IsPlaying = false;

    int CurrentLevelIndex = 0;//0 = invalid since we are 1 based here

    Level CurrentLevel;

    void Start()
    {
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

    void LoadNextLevel(int levelIndex = -1)
    {
        CurrentLevelIndex++;

        if(CurrentLevelIndex > LevelCount)//TODO Win the game
            CurrentLevelIndex = 1;

        TransitionManager.Instance.OnLevelLoaded += OnSceneLoaded;
        TransitionManager.Instance.LoadLevel(LEVEL_NAME + CurrentLevelIndex.ToString());
    }

    void OnSceneLoaded()
    {
        TransitionManager.Instance.OnLevelLoaded -= OnSceneLoaded;

        //CurrentLevel = (Level)FindObjectOfType(typeof(Level));
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
