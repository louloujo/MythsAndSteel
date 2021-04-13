using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Campagne : MonoBehaviour
{
    public MYthsAndSteel_Enum.Scenario _Scenario; //Scénario Séléctionné et affiché
    [SerializeField] int ScenarioVal = 0;
    [SerializeField] int spaceBetweenScenario = 0;
        
    [SerializeField] int Unlocked;//Nombre actuelle de niveau débloqué

    [Header("Assignations")]
    public Slider Unlockslider; // Slider qui lontre la progression de dévérouillage

    [SerializeField] private GameObject _buttonLeft = null;
    [SerializeField] private GameObject _buttonRight = null;
    [SerializeField] private float _mapSpeed = 0f;
    [SerializeField] private GameObject _mapTransform = null;

    private void Start()
    {
        //Assigne les valeur de dévérouillage des scénario
        Unlockslider.value = Unlocked;
    }

    private void Update(){
        _mapTransform.GetComponent<RectTransform>().localPosition = Vector2.MoveTowards(_mapTransform.GetComponent<RectTransform>().localPosition, new Vector2(-spaceBetweenScenario * (Screen.width / 1920) * ScenarioVal, _mapTransform.GetComponent<RectTransform>().localPosition.y), Time.deltaTime * _mapSpeed);
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
        int targetValue = ScenarioVal - 1;

        if(targetValue > 0 && targetValue < 6)
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
            ScenarioVal++;
        }
        else if(targetValue == 6)
        {
            _buttonRight.GetComponent<Button>().interactable = false;
            _buttonLeft.GetComponent<Button>().interactable = true;
            _Scenario++;
            ScenarioVal++;
        }
        else if(targetValue > 6){}

        if(ScenarioVal == targetValue){
            if(ScenarioVal == Unlocked){
                _buttonRight.GetComponent<Button>().interactable = false;
            }
        }
    }

    /// <summary>
    /// Update the position of the slider
    /// </summary>
    [EasyButtons.Button]
    public void UpdateSliderValue(){
        Unlockslider.value = Unlocked;
    }
}
