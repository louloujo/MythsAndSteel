using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

/*
    Ce script est le Game Manager. Il va gérer toutes les phases du jeu, les différents tours de jeu, ...
    Il sera aussi la pour changer de phase, changer de tour, ...
    Il est indispensable au jeu!!!

    NE LE SUPPRIMEZ PAS DE VOTRE SCENE!!!
*/

public class GameManager : MonoSingleton<GameManager>{

    #region Variables
    [Header("INFO TOUR ACTUEL")]
    //Correspond à la valeur du tour actuel
    [SerializeField] private int _actualTurnNumber = 0;
    public int ActualTurnNumber
    {
        get
        {
            return _actualTurnNumber;
        }
        set
        {
            _actualTurnNumber = value;
        }
    }

    //Permet de savoir si c'est le joueur 1 (TRUE) ou le joueur 2 (FALSE) qui commence durant ce tour
    [SerializeField] private bool _isPlayerRedStarting = false;
    public bool IsPlayerRedStarting => _isPlayerRedStarting;

    //Permet de savoir si c'est le joueur 1 (TRUE) ou le joueur 2 (FALSE) qui joue actuellement
    [SerializeField] private bool _isPlayerRedTurn = false;
    public bool IsPlayerRedTurn => _isPlayerRedTurn;

    //Est ce que les joueurs sont actuellement dans un tour de jeu?
    [SerializeField] private bool _isInTurn = false;
    public bool IsInTurn => _isInTurn;


    [Header("INFO PHASE DE JEU ACTUEL")]
    //Correspond à la phase actuelle durant le tour
    [SerializeField] private MYthsAndSteel_Enum.PhaseDeJeu _actualTurnPhase = MYthsAndSteel_Enum.PhaseDeJeu.Debut;
    public MYthsAndSteel_Enum.PhaseDeJeu ActualTurnPhase => _actualTurnPhase;

    //Le panneau qui est affiché sur l'écran du joueur
    GameObject actualSwitchPhasePanel = null;

    [Header("REFERENCES DES SCRIPTABLE")]
    //Event Manager
    [SerializeField] private EventCardClass _eventCardSO = null;
    public EventCardClass EventCardSO => _eventCardSO;
    //Game Manager avec tous les event
    [SerializeField] private GameManagerSO _managerSO = null;
    public GameManagerSO ManagerSO => _managerSO;

    [Header("COUPURE DE PHASE")]
    //Est ce que le joueur est en train de choisir des unités
    [SerializeField] private bool _chooseUnitForEvent = false;
    public bool ChooseUnitForEvent => _chooseUnitForEvent;

    [SerializeField] bool _chooseOponnentUnit = false;
    public bool ChooseOponentUnit => _chooseOponnentUnit;
    [SerializeField] bool _chooseArmyUnit = false;
    public bool ChooseArmyUnit => _chooseArmyUnit;

    [SerializeField] bool _redPlayerUseEvent = false;
    public bool RedPlayerUseEvent => _redPlayerUseEvent;

    //Liste des unités choisies
    [SerializeField] private List<GameObject> _unitChooseList = new List<GameObject>();
    public List<GameObject> UnitChooseList => _unitChooseList;

    public int _numberOfUnitToChoose = 0;

    //Fonctions à appeler après que le joueur ait choisit les unités
    public delegate void EventToCallAfterChoose();
    public EventToCallAfterChoose _eventCardCall;

    #endregion Variables

    /// <summary>
    /// Permet d'initialiser le script
    /// </summary>
    private void Start(){
        _managerSO.GoToDebutPhase += OnclickedEvent;
        _managerSO.GoToActivationPhase += OnclickedEvent;
        _managerSO.GoToOrgoneJ1Phase += OnclickedEvent;
        _managerSO.GoToActionJ1Phase += OnclickedEvent;
        _managerSO.GoToOrgoneJ2Phase += OnclickedEvent;
        _managerSO.GoToActionJ2Phase += OnclickedEvent;
        _managerSO.GoToStrategyPhase += OnclickedEvent;

        _managerSO.GoToOrgoneJ1Phase += DetermineWhichPlayerplay;
        _managerSO.GoToOrgoneJ2Phase += DetermineWhichPlayerplay;
        _isInTurn = true;
    }

    /// <summary>
    /// Aller à la phase de jeu renseigner en paramètre
    /// </summary>
    /// <param name="phaseToGoTo"></param>
    public void ChangePhase()
    {
        //Affiche le panneau de transition d'UI
        SwitchPhaseObjectUI(false);
    }

    /// <summary>
    /// Donne la victoire à une armée
    /// </summary>
    /// <param name="armeeGagnante"></param>
    public void VictoryForArmy(int armeeGagnante){
        Debug.Log($"Armée {armeeGagnante} a gagné");
    }

    /// <summary>
    /// Determine quel joueur est actuellement en train de jouer
    /// </summary>
    public void DetermineWhichPlayerplay()
    {
        if(_actualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1)
        {
            if(_isPlayerRedStarting)
            {
                _isPlayerRedTurn = true;
            }
            else
            {
                _isPlayerRedTurn = false;
            }
        }
        else if(_actualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2)
        {
            if(_isPlayerRedStarting)
            {
                _isPlayerRedTurn = false;
            }
            else
            {
                _isPlayerRedTurn = true;
            }
        }
    }

    /// <summary>
    /// Fonction qui est appellée lorsque l'event est appellé (event lors du clic sur le bouton pour passer à la phase suivante)
    /// </summary>
    public void OnclickedEvent(){
        //Les joueurs peuvent à nouveau jouer
        _isInTurn = true;

        //Détruit le panneau de changement de phase
        SwitchPhaseObjectUI(true);

        int newPhase = (int)_actualTurnPhase + 1;

        if(newPhase > 6) newPhase = 0;

        _actualTurnPhase = (MYthsAndSteel_Enum.PhaseDeJeu)newPhase;
    }

    #region UIFunction
    /// <summary>
    /// Affiche le panneau d'indication de changement de phase. Les joueurs doivent cliquer sur un bouton pour passer la phase
    /// </summary>
    public void SwitchPhaseObjectUI(bool destroy){
        //Ajoute le menu où il faut cliquer
        if(!destroy) {
            //Instantie le panneau de transition entre deux phases et le garde en mémoire
            GameObject phaseObj = Instantiate(UIInstance.Instance.SwitchPhaseObject, UIInstance.Instance.CanvasTurnPhase.transform.position, 
                                              Quaternion.identity, UIInstance.Instance.CanvasTurnPhase.transform);
            actualSwitchPhasePanel = phaseObj;

            //Variable qui permet d'avoir le texte à afficher au début de la phase
            string textForSwitch = "";
            MYthsAndSteel_Enum.PhaseDeJeu nextPhase = (MYthsAndSteel_Enum.PhaseDeJeu)((int)_actualTurnPhase + 1) > (MYthsAndSteel_Enum.PhaseDeJeu)6 ? 0 : (MYthsAndSteel_Enum.PhaseDeJeu)((int)_actualTurnPhase + 1);

            if(ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.Debut) textForSwitch = "Tour suivant : Vous passez à la phase" + " " + nextPhase.ToString();
            else textForSwitch = "Vous passez à la phase" + " " + nextPhase.ToString();

            //Change le texte en jeu
            phaseObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = textForSwitch;

            EventSystem.current.SetSelectedGameObject(phaseObj);
        }

        //Supprime le menu où il faut cliquer
        else {
            if(actualSwitchPhasePanel != null){
                Destroy(actualSwitchPhasePanel);
            }
        }
    }
    #endregion UIFunction

    /// <summary>
    /// Permet d'avoir en référence si c'est le joueur 1 qui commence ou le joueur 2
    /// </summary>
    /// <param name="player1"></param>
    public void SetPlayerStart(bool player1){
        _isPlayerRedStarting = player1;
    }

    #region EventMode
    /// <summary>
    /// Commence le mode event
    /// </summary>
    /// <param name="numberUnit"></param>
    /// <param name="opponentUnit"></param>
    /// <param name="armyUnit"></param>
    public void StartEventMode(int numberUnit, bool opponentUnit, bool armyUnit, bool redPlayer){
        _numberOfUnitToChoose = numberUnit;
        _chooseUnitForEvent = true;
        _chooseOponnentUnit = opponentUnit;
        _chooseArmyUnit = armyUnit;
        _redPlayerUseEvent = redPlayer;

        UIInstance.Instance.RedPlayerEventtransf.gameObject.SetActive(false);
        UIInstance.Instance.BluePlayerEventtransf.gameObject.SetActive(false);
        UIInstance.Instance.ButtonNextPhase.SetActive(false);
        UIInstance.Instance.ButtonEventRedPlayer._upButton.SetActive(false);
        UIInstance.Instance.ButtonEventRedPlayer._downButton.SetActive(false);
        UIInstance.Instance.ButtonEventBluePlayer._upButton.SetActive(false);
        UIInstance.Instance.ButtonEventBluePlayer._downButton.SetActive(false);

        _eventCardCall += StopEventMode;
    }

    void StopEventMode(){
        _numberOfUnitToChoose = 0;
        _chooseUnitForEvent = false;
        _chooseOponnentUnit = false;
        _chooseArmyUnit = false;
        _redPlayerUseEvent = false;

        UIInstance.Instance.RedPlayerEventtransf.gameObject.SetActive(true);
        UIInstance.Instance.BluePlayerEventtransf.gameObject.SetActive(true);
        UIInstance.Instance.ButtonNextPhase.SetActive(true);
        UIInstance.Instance.ButtonEventRedPlayer._upButton.SetActive(true);
        UIInstance.Instance.ButtonEventRedPlayer._downButton.SetActive(true);
        UIInstance.Instance.ButtonEventBluePlayer._upButton.SetActive(true);
        UIInstance.Instance.ButtonEventBluePlayer._downButton.SetActive(true);

        _eventCardCall = null;
    }

    public void AddUnitToList(GameObject unit){
        _unitChooseList.Add(unit);

        if(_unitChooseList.Count == _numberOfUnitToChoose){
            _eventCardCall();
        }
    }
    #endregion EventMode
}
