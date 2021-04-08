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

    public bool IllusionStratégique = false;

    //Fonctions à appeler après que le joueur ait choisit les unités
    public delegate void EventToCallAfterChoose();
    public EventToCallAfterChoose _eventCardCall;
    public EventToCallAfterChoose _waitEvent;

    float deltaTimeX = 0f;
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
    /// Commence le mode event pour choisir une unité
    /// </summary>
    /// <param name="numberUnit"></param>
    /// <param name="opponentUnit"></param>
    /// <param name="armyUnit"></param>
    public void StartEventModeUnit(int numberUnit, bool redPlayer, List<GameObject> _unitSelectable){
        UIInstance.Instance.DesactivateNextPhaseButton();

        _numberOfUnitToChoose = numberUnit;
        _chooseUnitForEvent = true;
        _selectableUnit.AddRange(_unitSelectable);
        _redPlayerUseEvent = redPlayer;

        foreach(GameObject gam in _selectableUnit){
            GameObject child = TilesManager.Instance.TileList[gam.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().AddChildRender(Mouvement.Instance.selectedSprite);
            child.tag = "SelectableTile";
        }

        UIInstance.Instance.RedPlayerEventtransf.gameObject.SetActive(false);
        UIInstance.Instance.BluePlayerEventtransf.gameObject.SetActive(false);
        UIInstance.Instance.ButtonNextPhase.SetActive(false);
        UIInstance.Instance.ButtonEventRedPlayer._upButton.SetActive(false);
        UIInstance.Instance.ButtonEventRedPlayer._downButton.SetActive(false);
        UIInstance.Instance.ButtonEventBluePlayer._upButton.SetActive(false);
        UIInstance.Instance.ButtonEventBluePlayer._downButton.SetActive(false);

        _eventCardCall += StopEventModeUnit;
    }

    /// <summary>
    /// Arrete le choix d'unité
    /// </summary>
    public void StopEventModeUnit(){
        UIInstance.Instance.ActivateNextPhaseButton();

        _numberOfUnitToChoose = 0;
        _chooseUnitForEvent = false;

        foreach(GameObject gam in _selectableUnit){
            //Détruit l'enfant avec le tag selectable tile
            GameObject tile = TilesManager.Instance.TileList[gam.GetComponent<UnitScript>().ActualTiledId];
            for(int i = 0; i < tile.transform.childCount; i++){
                if(tile.transform.GetChild(i).tag == "SelectableTile")
                {
                    Destroy(tile.transform.GetChild(i).gameObject);
                }
            }

            tile.GetComponent<TileScript>().RemoveChild();
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

        _eventCardCall = null;
        IllusionStratégique = false;
    }

    /// <summary>
    /// Ajoute une unité à la liste des unités sélectionnées
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnitToList(GameObject unit){
        _unitChooseList.Add(unit);

        //Pour la carte événement illusion stratégique
        if(IllusionStratégique){
            foreach(GameObject gam in _selectableUnit){
                if(gam.GetComponent<UnitScript>().UnitSO.IsInRedArmy != unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy){
                    GameObject tile = TilesManager.Instance.TileList[gam.GetComponent<UnitScript>().ActualTiledId].gameObject;

                    for(int i = 0; i < tile.transform.childCount; i++){
                        if(tile.transform.GetChild(i).tag == "SelectableTile"){
                            Destroy(tile.transform.GetChild(i).gameObject);
                        }
                    }

                    tile.GetComponent<TileScript>().RemoveChild();
                }
            }
        }

        if(_unitChooseList.Count == _numberOfUnitToChoose){
            _eventCardCall();
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
                for(int i = 0; i < tile.transform.childCount; i++)
                {
                    if(tile.transform.GetChild(i).tag == "SelectableTile")
                    {
                        Destroy(tile.transform.GetChild(i).gameObject);
                    }
                }
                tile.GetComponent<TileScript>().RemoveChild();

                GameObject child = TilesManager.Instance.TileList[gam.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().AddChildRender(Mouvement.Instance.selectedSprite);
                child.tag = "SelectableTile";
            }
        }
    }

    /// <summary>
    /// Commence le mode event pour choisir une case du plateau
    /// </summary>
    /// <param name="numberOfTile"></param>
    /// <param name="redPlayer"></param>
    /// <param name="_tileSelectable"></param>
    public void StartEventModeTiles(int numberOfTile, bool redPlayer, List<GameObject> _tileSelectable){
        UIInstance.Instance.DesactivateNextPhaseButton();

        _chooseTileForEvent = true;
        _redPlayerUseEvent = redPlayer;
        _numberOfTilesToChoose = numberOfTile;
        _selectableTiles.AddRange(_tileSelectable);

        foreach(GameObject gam in _selectableTiles){
            GameObject child = gam.GetComponent<TileScript>().AddChildRender(Mouvement.Instance.selectedSprite);
            child.tag = "SelectableTile";
        }

        UIInstance.Instance.RedPlayerEventtransf.gameObject.SetActive(false);
        UIInstance.Instance.BluePlayerEventtransf.gameObject.SetActive(false);
        UIInstance.Instance.ButtonNextPhase.SetActive(false);
        UIInstance.Instance.ButtonEventRedPlayer._upButton.SetActive(false);
        UIInstance.Instance.ButtonEventRedPlayer._downButton.SetActive(false);
        UIInstance.Instance.ButtonEventBluePlayer._upButton.SetActive(false);
        UIInstance.Instance.ButtonEventBluePlayer._downButton.SetActive(false);

        _eventCardCall += StopEventModeTile;
    }

    /// <summary>
    /// Arrete le choix de case
    /// </summary>
    public void StopEventModeTile(){
        UIInstance.Instance.ActivateNextPhaseButton();

        _chooseTileForEvent = false;

        foreach(GameObject gam in _selectableTiles){
            //Détruit l'enfant avec le tag selectable tile
            for(int i = 0; i < gam.transform.childCount; i++){
                if(gam.transform.GetChild(i).tag == "SelectableTile"){
                    Destroy(gam.transform.GetChild(i).gameObject);
                }
            }

            gam.GetComponent<TileScript>().RemoveChild();
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
        _eventCardCall = null;
        IllusionStratégique = false;
    }

    /// <summary>
    /// Ajoute la case à la liste
    /// </summary>
    /// <param name="tile"></param>
    public void AddTileToList(GameObject tile){
        if(_selectableTiles.Contains(tile)){
            _tileChooseList.Add(tile);

            if(_tileChooseList.Count == _numberOfTilesToChoose){
                _eventCardCall();
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
    #endregion EventMode
}
