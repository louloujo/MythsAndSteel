using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Campagne : MonoBehaviour
{
    int SceneToLoad; //La scène que l'on veut charger (se référer au build settings dans l'onglet file)
    public Scenario _Scenario; //Scénario Séléctionné et affiché
    
    public Sprite[] SpriteMap; //Sprite des map de chaques plateau
        
    [SerializeField] int Unlocked;//Nombre actuelle de niveau débloqué

    [Header("Assignations")]
    public Image ImageScenario; //Affiche La map du scénario actuelle
    public Text nomDeLaCampagne; //Affiche le nom de la campagne sur le bouton
    public Slider Unlockslider; // Slider qui lontre la progression de dévérouillage
    public GameObject LockCanvas; //Canvas affiché lorsqu'un niveau n'est pas débloqué

    private void Start()
    {
        //Assigne les valeur de dévérouillage des scénario
        Unlockslider.value = Unlocked;
    }



    public enum Scenario
    {
        Rethel, Shanghai, Stalingrad, Husky, Guadalcanal, ElAlamein, Elsenborn
    }

    public void ChangeScene()
    {
        if ((int)_Scenario <= Unlocked) //Check si le scénario est débloqué
        {
            SceneManager.LoadScene((int)_Scenario + 1);
        }
        
    }

    public void Decrease() //Fonction boutton pour montrer le scénario précédent
    {
        if((int)_Scenario == 0)
        {
            _Scenario = Scenario.Elsenborn;
        }
        else
        {
            _Scenario--;
        }

        Actualise();
        
    }

    public void Increase() //Fonction boutton pour montrer le scénario suivant
    {
        if ((int)_Scenario == 6)
        {
            _Scenario = Scenario.Rethel;
        }
        else
        {
            _Scenario++;
        }

        Actualise();
    }

    void Actualise() //Anti-voidUpdate
    {
        if ((int)_Scenario <= Unlocked) //Active et désactive le Canvas pour les scénario bloqué
        {
            LockCanvas.SetActive(false);
        }
        else
        {
            LockCanvas.SetActive(true);
        }
        nomDeLaCampagne.text = _Scenario.ToString(); //Actualise le nom du scénario
        ImageScenario.sprite = SpriteMap[(int)_Scenario];//Actualsie l'image de scénario
    }


}
