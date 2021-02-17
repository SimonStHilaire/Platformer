using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public List<Transform> EnemySpawnPoints;
    public Transform PlayerSpawnPosition;
    public string ConfigFilename;

    public int MaxEnemiesCount;

    Config LevelConfig;

    // Start is called before the first frame update
    void Start()
    {
        ReadConfig();
        //GenerateConfig();
    }

    void ReadConfig()
    {
        if (!string.IsNullOrEmpty(ConfigFilename) && File.Exists(ConfigFilename))
        {
            string jsonConfig = File.ReadAllText(ConfigFilename);

            LevelConfig = JsonUtility.FromJson<Config>(jsonConfig);

            Physics2D.gravity *= LevelConfig.GravityModifier;
        }
        else
        {
            Physics2D.gravity = new Vector2(0f, -9.8f);
        }
    }

    void GenerateConfig()
    {
        Config levelConfig = new Config();

        levelConfig.GravityModifier = 0.5f;
        levelConfig.EnemyRespawnInterval = 1f;

        levelConfig.Enemies = new List<EnemyConfig>(10);

        for (int i = 0; i < 10; ++i)
        {
            EnemyConfig enemy = new EnemyConfig();
            enemy.CanDrop = false;
            enemy.Speed = Random.Range(10f, 15f);
            enemy.SpawnPosition = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f);

            levelConfig.Enemies.Add(enemy);
        }

        string jsonConfig = JsonUtility.ToJson(levelConfig);

        File.WriteAllText("config.json", jsonConfig);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
