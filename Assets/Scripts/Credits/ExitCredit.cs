using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitCredit : MonoBehaviour
{
    private static CreditsList creditsList;

    public Text label;

    public void getCredits()
    {
        if(creditsList == null)
            creditsList = JsonUtility.FromJson<CreditsList>(File.ReadAllText("credits.json"));

        string displayString = "";
        foreach (Metier m in creditsList.metiers)
        {
            displayString += m.nomMetier;
            foreach (string n in m.noms)
            {
                displayString += "\n" + n;
            }
            displayString += "\n\n";
        }
        label.text = displayString;
    }

    private void Start()
    {
        getCredits();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitCredit();
        }
    }

    public void exitCredit()
    {
        SceneManager.UnloadSceneAsync("credits");
    }
}

[Serializable]
public class CreditsList
{
    public List<Metier> metiers;
}

[Serializable]
public class Metier
{
    public string nomMetier;
    public List<string> noms;
}