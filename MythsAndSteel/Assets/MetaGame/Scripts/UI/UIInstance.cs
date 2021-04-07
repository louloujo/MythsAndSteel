using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInstance : MonoSingleton<UIInstance>
{
    #region AppelDeScript
    MouseCommand mouseCommand;
    #endregion

    [Header("PHASE DE JEU")]
    //Le panneau à afficher lorsque l'on change de phase
    [SerializeField] private GameObject _switchPhaseObject = null;
    public GameObject SwitchPhaseObject => _switchPhaseObject;

    //Le canvas en jeu pour la phase d'activation
    [SerializeField] private GameObject _canvasActivation = null;
    public GameObject CanvasActivation => _canvasActivation;

    //Le canvas en jeu pour afficher des menus
    [SerializeField] private GameObject _canvasTurnPhase = null;
    public GameObject CanvasTurnPhase => _canvasTurnPhase;

    [SerializeField] private GameObject _buttonNextPhase = null;
    public GameObject ButtonNextPhase => _buttonNextPhase;


    [Header("PANNEAU D'ACTIVATION D'UNE UNITE")]
    [SerializeField] private MenuActionUnite _activationUnitPanel = null;
    public MenuActionUnite ActivationUnitPanel => _activationUnitPanel;


    [Header("CARTES EVENEMENTS")]
    //L'objet d'event à afficher lorsqu'une nouvelle carte event est piochée pour le joueur rouge
    [SerializeField] private GameObject _eventCardObjectRed = null;
    public GameObject EventCardObjectRed => _eventCardObjectRed;

    //L'objet d'event à afficher lorsqu'une nouvelle carte event est piochée.
    [SerializeField] private GameObject _eventCardObjectBlue = null;
    public GameObject EventCardObjectBlue => _eventCardObjectBlue;

    //Sprite du bouton activé
    [SerializeField] private Sprite _activateButtonSprite = null;
    public Sprite ActivateButtonSprite => _activateButtonSprite;

    //Sprite du bouton désactivé
    [SerializeField] private Sprite _desactivateButtonSprite = null;
    public Sprite DesactivateButtonSprite => _desactivateButtonSprite;

    //Boutons des cartes events du joueur rouge
    [SerializeField] private ButtonEvent _buttonEventRedPlayer = null;
    public ButtonEvent ButtonEventRedPlayer => _buttonEventRedPlayer;

    //Boutons des cartes events du joueur bleu
    [SerializeField] private ButtonEvent _buttonEventBluePlayer = null;
    public ButtonEvent ButtonEventBluePlayer => _buttonEventBluePlayer;

    //Transform des cartes events du joueur rouge
    [SerializeField] private Transform _redPlayerEventtransf = null;
    public Transform RedPlayerEventtransf => _redPlayerEventtransf;

    //Transform des cartes events du joueur bleu
    [SerializeField] private Transform _bluePlayerEventtransf = null;
    public Transform BluePlayerEventtransf => _bluePlayerEventtransf;

    //Transform de la carte event du joueur rouge le plus en bas
    [SerializeField] private Transform _redEventDowntrans = null;
    public Transform RedEventDowntrans => _redEventDowntrans;

    //Transform de la carte event du joueur bleu le plus en bas
    [SerializeField] private Transform _blueEventDowntrans = null;
    public Transform BlueEventDowntrans => _blueEventDowntrans;

    [Header("LISTES DES BOUTONS D ACTIONS")]
    [Tooltip("0 et  1 sont pour les boutons quitter, 2 et 3 sont pour switch entre la Page 1 et la Page 2")]
    //Les premiers chiffres (pour le moment 0 et 1 déterminent les bouttons à quitter les menus), les derniers déterminent les buttons des switch de menu.
    [SerializeField] private StatMenuButton _pageButton;
    public StatMenuButton PageButton => _pageButton;
    [Space]
    [Header("LISTES DES ELEMENTS UI POUR LE MOUSEOVER")]
    [Tooltip("Tous les éléments qui composent l'UI dans le MOUSEOVER")]
    //Texte qui indique le nom de l'unité.
    [SerializeField] private GameObject _titlePanelMouseOver;
    public GameObject TitlePanelMouseOver => _titlePanelMouseOver;
    //Ordre : 0 => Texte Vie, 1 => Texte Portée et 2 => Texte Déplacement
    [SerializeField] private TextStatUnit _mouseOverStats;
    public TextStatUnit MouseOverStats => _mouseOverStats;
    
    [Space]
    
    [Header("LISTES DES ELEMENTS UI POUR LE SHIFT CLIC DE LA PAGE 1")]
    [Tooltip("Tous les éléments qui composent l'UI pour le Shift Clic de la Page 1")]
    
    //Texte qui indique le nom de l'unité.
    [SerializeField] private GameObject _titlePanelShiftClicPage1;
    public GameObject TitlePanelShiftClicPage1 => _titlePanelShiftClicPage1;
    
    //Ordre : 0 => Texte Vie, 1 => Texte Portée et 2 => Texte Déplacement
    [SerializeField] private TextStatUnit _pageUnitStat;
    public TextStatUnit PageUnitStat => _pageUnitStat;

    //Ordre : 0 => Valeur Min entre deux valeurs, 1 => Valeur Max entre deux valeurs, 2 => Degats Min et 3 Degats Max
    [SerializeField] private AttackStat _attackStat;
    public AttackStat AttackStat => _attackStat;

    

    [Header("LISTES DES ELEMENTS UI POUR LE SHIFT CLIC DE LA PAGE 2")]
    [Tooltip("Tous les éléments qui composent l'UI pour le Shift Clic de la Page 2")]
    //Texte qui indique le nom de l'unité.
    [SerializeField] private GameObject _titlePanelShiftClicPage2;
    public GameObject TitlePanelShiftClicPage2 => _titlePanelShiftClicPage2;    
    
    [SerializeField] private List<GameObject> _middleImageTerrain;
    public List<GameObject> MiddleImageTerrain => _middleImageTerrain;

    [SerializeField] private List<GameObject> _middleTextTerrain;
    public List<GameObject> MiddleTextTerrain => _middleTextTerrain;

    [Header("ENFANTS CASE DU PLATEAU")]
    [SerializeField] private GameObject _mouvementTilePrefab;
    public GameObject MouvementTilePrefab => _mouvementTilePrefab;

    [Header("STAT DU JEU")]
    [SerializeField] private TextMeshProUGUI _fpsText = null;
    public TextMeshProUGUI FpsText => _fpsText;

    public void DesactivateNextPhaseButton(){
        _buttonNextPhase.SetActive(false);
    }

    public void ActivateNextPhaseButton(){
        _buttonNextPhase.SetActive(true);
    }

    #region UITile
    [Header("Tile's infos update")]
    [SerializeField] private TileTypeClass _typeTileList;

    //Variable du Scriptable.
    public TerrainTypeClass Terrain;

    public void CallUpdateUI(GameObject Tile)
    {
        if (Tile == null)
        {
            _typeTileList.UiTiles.SetBool("open", false); return;
        }
        else if (Tile.TryGetComponent(out TileScript T))
        {
            Terrain.Synch(Tile.GetComponent<TileScript>(), _typeTileList.Tile, _typeTileList.Desc, _typeTileList.Ressources, _typeTileList.Rendu); // Affiche les nouvelles informations à propos de la tile séléctionnée.  
            _typeTileList.UiTiles.SetBool("open", true);
        }
    }
    #endregion
}

#region ClassToRangeList
/// <summary>
/// Class pour les boutons pour les cartes events
/// </summary>
[System.Serializable]
public class ButtonEvent
{
    public GameObject _upButton = null;
    public GameObject _downButton = null;
}

/// <summary>
/// Boutons quitter et changer de page des menus de stats
/// </summary>
[System.Serializable]
public class StatMenuButton
{
    public Button _quitMenuPage1 = null;
    public Button _quitMenuPage2 = null;

    public Button _rightArrowPage1 = null;
    public Button _leftArrowPage2 = null;
}

/// <summary>
/// GameObject qui comprend la vie, la portée et la vitesse de déplacement
/// </summary>
[System.Serializable]
public class TextStatUnit
{
    public GameObject _lifeGam = null;
    public GameObject _rangeGam = null;
    public GameObject _moveGam = null;
}

/// <summary>
/// Liste des gameObjects pour l'attaque
/// </summary>
[System.Serializable]
public class AttackStat
{
    public GameObject _rangeMinDamageGam = null;
    public GameObject _rangeMaxDamageGam = null;

    public GameObject _minDamageValueGam = null;
    public GameObject _maxDamageValueGam = null;
}

/// <summary>
/// Liste pour tous les objets pour l'affichage du type de terrain sur chaque case
/// </summary>
[System.Serializable]
public class TileTypeClass
{
    [SerializeField] private TextMeshProUGUI _title;
    public TextMeshProUGUI Tile => _title;
    [SerializeField] private TextMeshProUGUI _desc;
    public TextMeshProUGUI Desc => _desc;
    [SerializeField] private Image _rendu;
    public Image Rendu => _rendu;
    [SerializeField] private TextMeshProUGUI _ressources;
    public TextMeshProUGUI Ressources => _ressources;
    [SerializeField] private Animator _uiTiles;
    public Animator UiTiles => _uiTiles;
}
#endregion ClassToRangeList