using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInstance : MonoSingleton<UIInstance>{
    [Header("UI Manager")]
    //Avoir en référence l'UI Manager pour appeler des fonctions à l'intérieur
    [SerializeField] private UIManager _uiManager = null;
    public UIManager UiManager => _uiManager;

    //Le canvas en jeu pour afficher des menus
    [SerializeField] private GameObject _canvasTurnPhase = null;
    public GameObject CanvasTurnPhase => _canvasTurnPhase;

    [Header("Phase De Jeu")]
    //Le panneau à afficher lorsque l'on change de phase
    [SerializeField] private GameObject _switchPhaseObject = null;
    public GameObject SwitchPhaseObject => _switchPhaseObject;

    [Header("Cartes Événements")]
    //L'objet d'event à afficher lorsqu'une nouvelle carte event est piochée
    [SerializeField] private GameObject _eventCardObject = null;
    public GameObject EventCardObject => _eventCardObject;


}
