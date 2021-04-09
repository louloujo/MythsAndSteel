using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Campagne : MonoBehaviour
{
    [SerializeField] int SceneToLoad; //La scène que l'on veut charger (se référer au build settings dans l'onglet file)
    public Scenario _Scenario;
    public Text nomDeLaCampagne;
    public Sprite[] SpriteMap;

    public Image ImageScenario;


    public enum Scenario
    {
        Rethel, Shanghai, Stalingrad, Husky, Guadalcanal, ElAlamein, Elsenborn
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene((int)_Scenario+1);
    }

    public void Decrease()
    {
        if((int)_Scenario == 0)
        {
            _Scenario = Scenario.Elsenborn;
        }
        else
        {
            _Scenario--;
        }
        
    }

    public void Increase()
    {
        if ((int)_Scenario == 6)
        {
            _Scenario = Scenario.Rethel;
        }
        else
        {
            _Scenario++;
        }
    }

    private void Update()
    {
        nomDeLaCampagne.text = _Scenario.ToString();
        ImageScenario.sprite = SpriteMap[(int)_Scenario];
    }

}
