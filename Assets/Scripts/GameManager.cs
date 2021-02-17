using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameSettings Settings;
    [Header("UI")]
    public GameObject MainMenu;

    public PlayerController PlayerRef;
    public EnemyController EnemyRef;

    public List<Transform> EnemySpawnPoints;
    public Transform PlayerSpawnPosition;

    public int MaxEnemiesCount;

    public int LevelCount;

    const string LEVEL_NAME = "Level";

    PlayerController Player = null;
    List<EnemyController> Enemies = new List<EnemyController>();

    float EnemySpawnTimer;

    bool IsPlaying = false;

    int CurrentLevelIndex = 0;//0 = invalid since we are 1 based here
    string CurrentLevel = "";

    void Start()
    {
        MainMenu.SetActive(true);
    }

    public void StartGame()
    {
        LoadNextLevel();

        /*if (Player == null)
        {
            Player = Instantiate(PlayerRef, PlayerSpawnPosition.position, Quaternion.identity, null);
        }
        else
        {
            Player.transform.position = PlayerSpawnPosition.position;
        }

        Camera.main.GetComponent<CameraController>().Player = Player.transform;

        Player.OnEnemyCollison += OnPlayerTouchEnemy;

        Enemies = new List<EnemyController>();

        EnemySpawnTimer = Settings.EnemySpawnInterval;

        MainMenu.SetActive(false);
        IsPlaying = true;*/
    }

    void LoadNextLevel(int levelIndex = -1)
    {
        if (CurrentLevelIndex > 0 && SceneManager.GetSceneByName(CurrentLevel).isLoaded)
            SceneManager.UnloadSceneAsync(CurrentLevel);

        CurrentLevelIndex++;

        if(CurrentLevelIndex > LevelCount)//TODO Win the game
            CurrentLevelIndex = 1;

        CurrentLevel = LEVEL_NAME + CurrentLevelIndex.ToString();

        SceneManager.LoadScene(CurrentLevel, LoadSceneMode.Additive);
    }

    void OnPlayerTouchEnemy()
    {
        IsPlaying = false;
        MainMenu.SetActive(true);

        for (int i = 0; i < Enemies.Count; ++i)
        {
            Destroy(Enemies[i].gameObject);
        }

        Destroy(Player.gameObject);
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
                int spawnIndex = Random.Range(0, EnemySpawnPoints.Count);

                EnemyController newEnemy = Instantiate(EnemyRef, EnemySpawnPoints[spawnIndex].position, Quaternion.identity, transform);

                Enemies.Add(newEnemy);

                if (Enemies.Count < MaxEnemiesCount)
                    EnemySpawnTimer = Settings.EnemySpawnInterval;
            }
        }
    }
}
