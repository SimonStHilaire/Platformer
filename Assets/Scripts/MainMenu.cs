using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StarGame()
    {
        GameManager.Instance.StartGame();
    }

    public void StarCredits()
    {
        GameManager.Instance.StartCredits();
    }
}
