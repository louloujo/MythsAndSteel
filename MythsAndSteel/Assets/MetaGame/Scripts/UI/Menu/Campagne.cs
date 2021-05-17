using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Campagne : MonoBehaviour
{
    [SerializeField] private SaveData saveData;
    public MYthsAndSteel_Enum.Scenario _Scenario; //Scénario Séléctionné et affiché
    [SerializeField] int ScenarioVal = 0;
    [SerializeField] int spaceBetweenScenario = 0;
    [SerializeField] int Unlocked;//Nombre actuelle de niveau débloqué

    [Header("Assignations")]
    [SerializeField] private GameObject Jauge0 = null;
    [SerializeField] private GameObject Jauge1 = null;
    [SerializeField] private GameObject Jauge2 = null;
    [SerializeField] private GameObject Jauge3 = null;
    [SerializeField] private GameObject Jauge4 = null;
    [SerializeField] private GameObject Jauge5 = null;
    [SerializeField] private GameObject Jauge6 = null;
    [SerializeField] private GameObject Jauge7 = null;

    [SerializeField] private GameObject _buttonLeft = null;
    [SerializeField] private GameObject _buttonRight = null;
    [SerializeField] private float _mapSpeed = 0f;
    [SerializeField] private GameObject _mapTransform = null;
    [SerializeField] private TextMeshProUGUI RedPlayerVictories;
    [SerializeField] private TextMeshProUGUI BluePlayerVictories;
    [SerializeField] int redPlayerVictories;
    [SerializeField] int bluePlayerVictories;

    private void Awake()
    {
        Time.timeScale = 1;
        Unlocked = PlayerPrefs.GetInt("UnlockCampaign");

        if (Unlocked == 0)
        {
            Jauge0.SetActive(true);
            Jauge1.SetActive(false);
            Jauge2.SetActive(false);
            Jauge3.SetActive(false);
            Jauge4.SetActive(false);
            Jauge5.SetActive(false);
            Jauge6.SetActive(false);
            Jauge7.SetActive(false);
        }
        else if (Unlocked == 1)
        {
            Jauge0.SetActive(false);
            Jauge1.SetActive(true);
            Jauge2.SetActive(false);
            Jauge3.SetActive(false);
            Jauge4.SetActive(false);
            Jauge5.SetActive(false);
            Jauge6.SetActive(false);
            Jauge7.SetActive(false);
        }
        else if (Unlocked == 2)
        {
            Jauge0.SetActive(false);
            Jauge1.SetActive(false);
            Jauge2.SetActive(true);
            Jauge3.SetActive(false);
            Jauge4.SetActive(false);
            Jauge5.SetActive(false);
            Jauge6.SetActive(false);
            Jauge7.SetActive(false);
        }
        else if (Unlocked == 3)
        {
            Jauge0.SetActive(false);
            Jauge1.SetActive(false);
            Jauge2.SetActive(false);
            Jauge3.SetActive(true);
            Jauge4.SetActive(false);
            Jauge5.SetActive(false);
            Jauge6.SetActive(false);
            Jauge7.SetActive(false);
        }
        else if (Unlocked == 4)
        {
            Jauge0.SetActive(false);
            Jauge1.SetActive(false);
            Jauge2.SetActive(false);
            Jauge3.SetActive(false);
            Jauge4.SetActive(true);
            Jauge5.SetActive(false);
            Jauge6.SetActive(false);
            Jauge7.SetActive(false);
        }
        else if (Unlocked == 5)
        {
            Jauge0.SetActive(false);
            Jauge1.SetActive(false);
            Jauge2.SetActive(false);
            Jauge3.SetActive(false);
            Jauge4.SetActive(false);
            Jauge5.SetActive(true);
            Jauge6.SetActive(false);
            Jauge7.SetActive(false);
        }
        else if (Unlocked == 6)
        {
            Jauge0.SetActive(false);
            Jauge1.SetActive(false);
            Jauge2.SetActive(false);
            Jauge3.SetActive(false);
            Jauge4.SetActive(false);
            Jauge5.SetActive(false);
            Jauge6.SetActive(true);
            Jauge7.SetActive(false);
        }
        else if (Unlocked >= 7)
        {
            Jauge0.SetActive(false);
            Jauge1.SetActive(false);
            Jauge2.SetActive(false);
            Jauge3.SetActive(false);
            Jauge4.SetActive(false);
            Jauge5.SetActive(false);
            Jauge6.SetActive(false);
            Jauge7.SetActive(true);
        }
        if (Unlocked == 0)
        {
            _buttonRight.GetComponent<Button>().interactable = false;
            _buttonLeft.GetComponent<Button>().interactable = false;
        }
    }


    private void Update(){
        _mapTransform.GetComponent<RectTransform>().localPosition = Vector2.MoveTowards(_mapTransform.GetComponent<RectTransform>().localPosition, new Vector2(-spaceBetweenScenario * (Screen.width / 1920f) * ScenarioVal, _mapTransform.GetComponent<RectTransform>().localPosition.y), Time.deltaTime * _mapSpeed);
        redPlayerVictories = saveData.redPlayerVictories;
        bluePlayerVictories = saveData.bluePlayerVictories;
        RedPlayerVictories.text = redPlayerVictories.ToString();
        BluePlayerVictories.text = bluePlayerVictories.ToString();
    }

    /// <summary>
    /// Permet d'aller à une scène quand on clique sur un bouton
    /// </summary>
    public void ChangeScene(int sceneID)
    {
            SceneManager.LoadScene(sceneID);
    }

    /// <summary>
    /// Fonction boutton pour montrer le scénario précédent
    /// </summary>
    public void Decrease() 
    {
        int targetValue = ScenarioVal-1;

        if(targetValue > 0 && targetValue < 7)
        {
            _buttonRight.GetComponent<Button>().interactable = true;
            _buttonLeft.GetComponent<Button>().interactable = true;
            _Scenario--;
            ScenarioVal--;
        }
        else if(targetValue == 0)
        {
            _buttonRight.GetComponent<Button>().interactable = true;
            _buttonLeft.GetComponent<Button>().interactable = false;
            _Scenario--;
            ScenarioVal--;
        }
        else if(targetValue < 0) { }
    }

    /// <summary>
    /// Fonction boutton pour montrer le scénario suivant
    /// </summary>
    public void Increase()
    {
        int targetValue = ScenarioVal+1;

        if(targetValue > 0 && targetValue < 6)
        {
            _buttonRight.GetComponent<Button>().interactable = true;
            _buttonLeft.GetComponent<Button>().interactable = true;
            _Scenario++;
            ScenarioVal++;;
        }
        else if(targetValue == 6)
        {
            _buttonRight.GetComponent<Button>().interactable = false;
            _buttonLeft.GetComponent<Button>().interactable = true;
            _Scenario++;
            ScenarioVal++;
        }
        else if(targetValue > 6){}

   
        if(ScenarioVal == Unlocked){
            _buttonRight.GetComponent<Button>().interactable = false;
        }
        
    }
}
