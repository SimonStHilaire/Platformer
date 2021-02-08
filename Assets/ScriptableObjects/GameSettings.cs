using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/NewGameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public float PlayerProjectilesSpeed;
    public bool DebugDraw;
}
