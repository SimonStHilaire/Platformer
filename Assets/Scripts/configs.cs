using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class EnemyConfig
{
    public Vector3 SpawnPosition;
    public float Speed;
    public bool CanDrop;
}

[Serializable]
public class Config
{
    public float GravityModifier;
    public float EnemyRespawnInterval;
    public int MaxEnemiesCount;

    public List<EnemyConfig> Enemies;
}
