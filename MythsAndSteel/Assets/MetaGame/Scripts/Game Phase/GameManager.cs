using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

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

    [Header("REFERENCES DES SCRIPTABLE")]
    //Event Manager
    [SerializeField] private EventCardClass _eventCardSO = null;
    public EventCardClass EventCardSO => _eventCardSO;
    //Game Manager avec tous les event
    [SerializeField] private GameManagerSO _managerSO = null;
    public GameManagerSO ManagerSO => _managerSO;

    [Header("SELECTION UNITE")]
    [Header("MODE EVENEMENT")]
    [Space]
    //Est ce que le joueur est en train de choisir des unités
    [SerializeField] private bool _chooseUnitForEvent = false;
    public bool ChooseUnitForEvent => _chooseUnitForEvent;

    //Liste des unités sélectionnables par le joueur
    [SerializeField] private List<GameObject> _selectableUnit = new List<GameObject>();
    public List<GameObject> SelectableUnit => _selectableUnit;

    //Liste des unités choisies
    [SerializeField] private List<GameObject> _unitChooseList = new List<GameObject>();
    public List<GameObject> UnitChooseList => _unitChooseList;

    //Nombre d'unité à choisir
    [SerializeField] int _numberOfUnitToChoose = 0;

    [Header("SELECTION CASE")]
    //Est ce que le joueur est en train de choisir des unités
    [SerializeField] private bool _chooseTileForEvent = false;
    public bool ChooseTileForEvent => _chooseTileForEvent;

    //Liste des cases sélectionnables
    public List<GameObject> _selectableTiles = new List<GameObject>();

    //Nombre d'unité à choisir
    [SerializeField] int _numberOfTilesToChoose = 0;

    //Liste des unités choisies
    [SerializeField] private List<GameObject> _tileChooseList = new List<GameObject>();
    public List<GameObject> TileChooseList => _tileChooseList;


    [Space]
    //Est ce que c'est le joueur rouge qui a utilisé les cartes events
    [SerializeField] bool _redPlayerUseEvent = false;
    public bool RedPlayerUseEvent => _redPlayerUseEvent;

    [HideInInspector] public bool IllusionStratégique = false;

    string _titleValidation = "";
    string _descriptionValidation = "";

    //Fonctions à appeler après que le joueur ait choisit les unités
    public delegate void EventToCallAfterChoose();
    public EventToCallAfterChoose _eventCall;
    public EventToCallAfterChoose _eventCallCancel;
    public EventToCallAfterChoose _waitEvent;

    [Header("ACTIVATION")]
    //Est ce que le joueur est en train de choisir des unités
    [SerializeField] private PhaseActivation _activationPhase = null;
    public PhaseActivation ActivationPhase => _activationPhase;

    float deltaTimeX = 0f;

    #region CheckOrgone
    //Check l'orgone pour éviter l'override
    public bool IsCheckingOrgone = false;

    //Event qui permet d'attendre pour donner de l'orgone à un joueur
    public delegate void Checkorgone();
    public Checkorgone _waitToCheckOrgone;

    //Quel joueur attend de recevoir son orgone
    private int _playerOrgone = 0;
    public int PlayerOrgone => _playerOrgone;

    //Quelle est la valeur a donner au joueur
    private int _valueOrgone= 0;
    public int ValueOrgone => _valueOrgone;
    #endregion CheckOrgone

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

    private void Update(){
        #region FPSCounter
        deltaTimeX += Time.deltaTime;
        deltaTimeX /= 2;
        UIInstance.Instance.FpsText.text = "FPS : " +((int) (1 / deltaTimeX)).ToString();
        #endregion FPSCounter
    }

    /// <summary>
    /// Quand le joueur clic pour passer à la phase suivante
    /// </summary>
    public void CliCToChangePhase(){
        UIInstance.Instance.ShowValidationPanel("Passer à la phase suivante", "Êtes-vous sur de vouloir passer à la phase suivante? En passant la phase vous n'aurez pas la possibilité de revenir en arrière.");
        _eventCall += ChangePhase;
        _eventCallCancel += CancelSkipPhase;
    }

    /// <summary>
    /// Quand le joueur annule le fait de passer une phase
    /// </summary>
    void CancelSkipPhase()
    {
        _eventCallCancel -= CancelSkipPhase;
        UIInstance.Instance.ActivateNextPhaseButton();
    }

    /// <summary>
    /// Aller à la phase de jeu renseigner en paramètre
    /// </summary>
    /// <param name="phaseToGoTo"></param>
    public void ChangePhase()
    {
        //Affiche le panneau de transition d'UI
        _isInTurn = false;
        SwitchPhaseObjectUI();
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
    }

    public void GoPhase(MYthsAndSteel_Enum.PhaseDeJeu phase)
    {
        _actualTurnPhase = phase;
    }

    #region UIFunction
    /// <summary>
    /// Affiche le panneau d'indication de changement de phase. Les joueurs doivent cliquer sur un bouton pour passer la phase
    /// </summary>
    public void SwitchPhaseObjectUI()
    {
        //Ajoute le menu où il faut cliquer
        //Instantie le panneau de transition entre deux phases et le garde en mémoire
        GameObject phaseObj = Instantiate(UIInstance.Instance.SwitchPhaseObject, UIInstance.Instance.CanvasTurnPhase.transform.position,
                                          Quaternion.identity, UIInstance.Instance.CanvasTurnPhase.transform);

        //Variable qui permet d'avoir le texte à afficher au début de la phase
        string textForSwitch = "";
        int nextPhase = (int)ActualTurnPhase + 1 > 6 ? 0 : (int) ActualTurnPhase + 1;
        textForSwitch = "Phase " + ((MYthsAndSteel_Enum.PhaseDeJeu) nextPhase).ToString();

        phaseObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = textForSwitch;
        Destroy(phaseObj, 1.25f);      
        if(ActualTurnPhase + 1 == MYthsAndSteel_Enum.PhaseDeJeu.Activation){
            StartCoroutine(waitToChange());
        }
        else{
            ManagerSO.GoToPhase();
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
    /// Commence le mode event pour choisir une unité
    /// </summary>
    /// <param name="numberUnit"></param>
    /// <param name="opponentUnit"></param>
    /// <param name="armyUnit"></param>
    public void StartEventModeUnit(int numberUnit, bool redPlayer, List<GameObject> _unitSelectable, string title, string description){
        UIInstance.Instance.DesactivateNextPhaseButton();

        _titleValidation = title;
        _descriptionValidation = description;

        _numberOfUnitToChoose = numberUnit;
        _chooseUnitForEvent = true;
        _selectableUnit.AddRange(_unitSelectable);
        _redPlayerUseEvent = redPlayer;

        foreach(GameObject gam in _selectableUnit){
            TilesManager.Instance.TileList[gam.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.EventSelect);
        }

        _eventCall += StopEventModeUnit;
    }

    /// <summary>
    /// Arrete le choix d'unité
    /// </summary>
    public void StopEventModeUnit(){
        _numberOfUnitToChoose = 0;
        _chooseUnitForEvent = false;

        foreach(GameObject gam in _selectableUnit){
            //Détruit l'enfant avec le tag selectable tile
            GameObject tile = TilesManager.Instance.TileList[gam.GetComponent<UnitScript>().ActualTiledId];
            tile.GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.EventSelect);
        }

        _selectableUnit.Clear();

        _redPlayerUseEvent = false;

        UIInstance.Instance.RedPlayerEventtransf.gameObject.SetActive(true);
        UIInstance.Instance.BluePlayerEventtransf.gameObject.SetActive(true);
        UIInstance.Instance.ButtonNextPhase.SetActive(true);
        UIInstance.Instance.ButtonEventRedPlayer._upButton.SetActive(true);
        UIInstance.Instance.ButtonEventRedPlayer._downButton.SetActive(true);
        UIInstance.Instance.ButtonEventBluePlayer._upButton.SetActive(true);
        UIInstance.Instance.ButtonEventBluePlayer._downButton.SetActive(true);

        _eventCall = null;
        IllusionStratégique = false;
    }

    /// <summary>
    /// Ajoute une unité à la liste des unités sélectionnées
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnitToList(GameObject unit){
        if(unit != null){
            _unitChooseList.Add(unit);

            //Pour la carte événement illusion stratégique
            if(IllusionStratégique)
            {
                foreach(GameObject gam in _selectableUnit)
                {
                    if(gam.GetComponent<UnitScript>().UnitSO.IsInRedArmy != unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                    {
                        GameObject tile = TilesManager.Instance.TileList[gam.GetComponent<UnitScript>().ActualTiledId].gameObject;
                        tile.GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.EventSelect);
                    }
                }
            }
            else { }

            if(_unitChooseList.Count == _numberOfUnitToChoose)
            {
                _chooseUnitForEvent = false;
                UIInstance.Instance.ShowValidationPanel(_titleValidation, _descriptionValidation);
            }
        }
    }

    /// <summary>
    /// Enleve une unité de la liste
    /// </summary>
    /// <param name="unit"></param>
    public void RemoveUnitToList(GameObject unit){
        _unitChooseList.Remove(unit);

        if(IllusionStratégique){
            foreach(GameObject gam in _selectableUnit){
                GameObject tile = TilesManager.Instance.TileList[gam.GetComponent<UnitScript>().ActualTiledId].gameObject;
                tile.GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.EventSelect);
                tile.GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.EventSelect);
            }
        }
    }

    /// <summary>
    /// Commence le mode event pour choisir une case du plateau
    /// </summary>
    /// <param name="numberOfTile"></param>
    /// <param name="redPlayer"></param>
    /// <param name="_tileSelectable"></param>
    public void StartEventModeTiles(int numberOfTile, bool redPlayer, List<GameObject> _tileSelectable, string title, string description){
        UIInstance.Instance.DesactivateNextPhaseButton();

        _titleValidation = title;
        _descriptionValidation = description;

        _chooseTileForEvent = true;
        _redPlayerUseEvent = redPlayer;
        _numberOfTilesToChoose = numberOfTile;
        _selectableTiles.AddRange(_tileSelectable);

        foreach(GameObject gam in _selectableTiles){
            gam.GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.EventSelect);
        }

        _eventCall += StopEventModeTile;
    }

    /// <summary>
    /// Arrete le choix de case
    /// </summary>
    public void StopEventModeTile(){
        _chooseTileForEvent = false;

        foreach(GameObject gam in _selectableTiles){
            gam.GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.EventSelect);
        }

        UIInstance.Instance.RedPlayerEventtransf.gameObject.SetActive(true);
        UIInstance.Instance.BluePlayerEventtransf.gameObject.SetActive(true);
        UIInstance.Instance.ButtonNextPhase.SetActive(true);
        UIInstance.Instance.ButtonEventRedPlayer._upButton.SetActive(true);
        UIInstance.Instance.ButtonEventRedPlayer._downButton.SetActive(true);
        UIInstance.Instance.ButtonEventBluePlayer._upButton.SetActive(true);
        UIInstance.Instance.ButtonEventBluePlayer._downButton.SetActive(true);

        _selectableTiles.Clear();
        _redPlayerUseEvent = false;
        _eventCall = null;
        IllusionStratégique = false;
    }

    /// <summary>
    /// Ajoute la case à la liste
    /// </summary>
    /// <param name="tile"></param>
    public void AddTileToList(GameObject tile){
        if(tile != null)
        {
            if(_selectableTiles.Contains(tile))
            {
                _tileChooseList.Add(tile);

                if(_tileChooseList.Count == _numberOfTilesToChoose)
                {
                    _chooseTileForEvent = false;
                    UIInstance.Instance.ShowValidationPanel(_titleValidation, _descriptionValidation);
                }
            }
        }
    }
    
    /// <summary>
    /// Fonction qui permet d'attendre avant de relancer une autre fonction
    /// </summary>
    /// <param name="t"></param>
    public void WaitToMove(float t){
        StartCoroutine(waitToCall(t));
    }

    IEnumerator waitToCall(float t){
        yield return new WaitForSeconds(t);
        if(_waitEvent != null)
        {
            _waitEvent();
        }
    }

    /// <summary>
    /// Call the event of validation panel
    /// </summary>
    public void CallEvent(){
        _eventCall();
    }

    /// <summary>
    /// Call the event cancel on the validation panel
    /// </summary>
    public void CancelEvent(){
        StopEventModeTile();
        StopEventModeUnit();
        if(_eventCallCancel != null) _eventCallCancel();
    }
    #endregion EventMode

    #region WaitOrgone
    /// <summary>
    /// Permet de lancer la fonction d'orgone
    /// </summary>
    /// <param name="player"></param>
    /// <param name="value"></param>
    public void LaunchOrgone(int player, int value){
        _playerOrgone = player;
        _valueOrgone = value;
    }

    /// <summary>
    /// Quand l'orgone a été effectuée
    /// </summary>
    public void StopOrgone(){
        _playerOrgone = 0;
        _valueOrgone = 0;
    }
    #endregion WaitOrgone

    IEnumerator waitToChange(){
        yield return new WaitForSeconds(1.35f);
        ManagerSO.GoToPhase();
        _isInTurn = true;
    }
}
