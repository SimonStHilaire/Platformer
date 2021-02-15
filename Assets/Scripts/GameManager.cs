using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    PlayerController Player = null;
    List<EnemyController> Enemies = new List<EnemyController>();

    float EnemySpawnTimer;

    bool IsPlaying = false;

    void Start()
    {
        MainMenu.SetActive(true);
    }

    public void StartGame()
    {
        if (Player == null)
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
        IsPlaying = true;
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
