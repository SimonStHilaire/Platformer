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
    List<EnemyController> Enemies = new List<EnemyController>();

    float EnemySpawnTimer;

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

        //Remove remaining ennemies
        for (int i = 0; i < Enemies.Count; ++i)
        {
            Destroy(Enemies[i].gameObject);
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

        Enemies = new List<EnemyController>();

        EnemySpawnTimer = Settings.EnemySpawnInterval;

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

        if(EnemySpawnTimer > 0)
        {
            EnemySpawnTimer -= Time.deltaTime;

            if (EnemySpawnTimer <= 0)
            {
                int spawnIndex = Random.Range(0, CurrentLevel.EnemySpawnPoints.Count);

                EnemyController newEnemy = Instantiate(EnemyRef, CurrentLevel.EnemySpawnPoints[spawnIndex].position, Quaternion.identity, transform);

                Enemies.Add(newEnemy);

                if (Enemies.Count < CurrentLevel.MaxEnemiesCount || CurrentLevel.MaxEnemiesCount == 0)
                    EnemySpawnTimer = Settings.EnemySpawnInterval;
            }
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            SoundOn = !SoundOn;
            UpdateVolume();
        }
        
    }
}
