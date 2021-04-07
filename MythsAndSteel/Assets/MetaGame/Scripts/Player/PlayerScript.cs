using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoSingleton<PlayerScript>
{
    [SerializeField] private Player _redPlayerInfos = new Player();
    [SerializeField] private Player _bluePlayerInfos = new Player();
    [Space]
    //Liste des Unités
    public List<GameObject> _unitListRedPlayer = new List<GameObject>();
    public List<GameObject> _unitListBluePlayer = new List<GameObject>();

    //Liste des unités désactivées
    public List<MYthsAndSteel_Enum.TypeUnite> DisactivateUnitType = new List<MYthsAndSteel_Enum.TypeUnite>();
    
    [SerializeField] bool _ArmyRedWinAtTheEnd;
    public bool ArmyRedWinAtTheEnd => _ArmyRedWinAtTheEnd;

    [Header("Cartes events")]
    [SerializeField] private EventCardList _eventCardList = null;
    public EventCardList EventCardList => _eventCardList;

    private void Start(){
        EventCardList._eventSO.UpdateVisualUI(_eventCardList._eventGamBluePlayer, 2);
        EventCardList._eventSO.UpdateVisualUI(_eventCardList._eventGamRedPlayer, 1);
    }

    #region DesactivationUnitType
    /// <summary>
    /// Désactive un type d'unité
    /// </summary>
    /// <param name="DesactiveUnit"></param>
    public void DesactivateUnitType(MYthsAndSteel_Enum.TypeUnite DesactiveUnit)
    {
        DisactivateUnitType.Add(DesactiveUnit);
    }


    /// <summary>
    /// active tous les types d'unités
    /// </summary>
    public void ActivateAllUnitType()
    {
        DisactivateUnitType.Clear();
    }
    #endregion DesactivationUnitType

    #region CarteEvent
    /// <summary>
    /// Ajoute une carte event random au joueur
    /// </summary>
    /// <param name="player"></param>
    [EasyButtons.Button]
    public void GiveEventCard(int player)
    {
        List<MYthsAndSteel_Enum.EventCard> combineEventCards = new List<MYthsAndSteel_Enum.EventCard>();
        combineEventCards.AddRange(EventCardList._eventCardRedPlayer);
        combineEventCards.AddRange(EventCardList._eventCardBluePlayer);

        int randomCard = UnityEngine.Random.Range(0, EventCardList._eventSO.NumberOfEventCard);

        MYthsAndSteel_Enum.EventCard newCard = EventCardList._eventSO.EventCardList[randomCard]._eventType;

        if(combineEventCards.Contains(newCard)){
            GiveEventCard(player);
            return;
        }

        AddEventCard(player, newCard);
    }

    /// <summary>
    /// Ajoute une carte spécifique au joueur
    /// </summary>
    /// <param name="player"></param>
    /// <param name="card"></param>
    void AddEventCard(int player, MYthsAndSteel_Enum.EventCard card)
    {
        if(player == 1){
            EventCardList._eventCardRedPlayer.Insert(1, card);
        }
        else if(player == 2){
            EventCardList._eventCardBluePlayer.Insert(1, card);
        }
        else{
            Debug.LogError("vous essayez d'ajouter une carte event a un joueur qui n'existe pas");
        }

        CreateEventCard(player, card);
    }

    /// <summary>
    /// Ajoute la carte event au canvas
    /// </summary>
    /// <param name="player"></param>
    /// <param name="card"></param>
    void CreateEventCard(int player, MYthsAndSteel_Enum.EventCard card){
        GameObject newCard = Instantiate(player == 1? UIInstance.Instance.EventCardObjectRed : UIInstance.Instance.EventCardObjectBlue,
                                         player == 1 ? UIInstance.Instance.RedPlayerEventtransf.GetChild(0).transform.position : UIInstance.Instance.BluePlayerEventtransf.GetChild(0).transform.position,
                                         Quaternion.identity,
                                         player == 1 ? UIInstance.Instance.RedPlayerEventtransf.GetChild(0) : UIInstance.Instance.BluePlayerEventtransf.GetChild(0));

        EventCard newEventCard = new EventCard();
        foreach(EventCard evC in EventCardList._eventSO.EventCardList){
            if(evC._eventType == card){
                newEventCard = evC;
            }
        }
        newCard.GetComponent<EventCardContainer>().AddEvent(newEventCard);

        AddEventToButton(card, newCard);

        if(player == 1){
            EventCardList._eventGamRedPlayer.Insert(1, newCard);
            _eventCardList._eventSO.UpdateVisualUI(EventCardList._eventGamRedPlayer, 1);
        }
        else if(player == 2){
            EventCardList._eventGamBluePlayer.Insert(1, newCard);
            _eventCardList._eventSO.UpdateVisualUI(EventCardList._eventGamBluePlayer, 2);
        }
        else{
            Debug.LogError("vous essayez d'ajouter une carte event a un joueur qui n'existe pas");
        }
    }

    public void AddEventToButton(MYthsAndSteel_Enum.EventCard card, GameObject cardGam){
        switch(card)
        {
            case MYthsAndSteel_Enum.EventCard.Activation_de_nodus:
                break;

            case MYthsAndSteel_Enum.EventCard.Armes_perforantes:
                break;

            case MYthsAndSteel_Enum.EventCard.Bombardement_aérien:
                break;

            case MYthsAndSteel_Enum.EventCard.Cessez_le_feu:
                break;

            case MYthsAndSteel_Enum.EventCard.Déploiement_accéléré:
                break;

            case MYthsAndSteel_Enum.EventCard.Détonation_d_orgone:
                break;

            case MYthsAndSteel_Enum.EventCard.Entraînement_rigoureux:
                break;

            case MYthsAndSteel_Enum.EventCard.Fil_barbelé:
                break;

            case MYthsAndSteel_Enum.EventCard.Illusion_stratégique:
                break;

            case MYthsAndSteel_Enum.EventCard.Manoeuvre_stratégique:
                break;

            case MYthsAndSteel_Enum.EventCard.Optimisation_de_l_orgone:
                break;

            case MYthsAndSteel_Enum.EventCard.Paralysie:
                break;

            case MYthsAndSteel_Enum.EventCard.Pillage_orgone:
                break;

            case MYthsAndSteel_Enum.EventCard.Pointeurs_laser_optimisés:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchPointeursLaserOptimisés);
                break;

            case MYthsAndSteel_Enum.EventCard.Reprogrammation:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchReproggramation);
                break;

            case MYthsAndSteel_Enum.EventCard.Réapprovisionnement:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchReapprovisionnement);
                break;

            case MYthsAndSteel_Enum.EventCard.Sabotage:
                break;

            case MYthsAndSteel_Enum.EventCard.Sérum_expérimental:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchSerumExperimental);
                break;

            case MYthsAndSteel_Enum.EventCard.Transfusion_d_orgone:
                break;
        }
    }

    #endregion CarteEvent

    /// <summary>
    /// Est ce qu'il reste des unités dans l'armée du joueur
    /// </summary>
    /// <param name="Joueur"></param>
    /// <returns></returns>
    public bool CheckArmy(UnitScript unit, int Joueur){
        if(Joueur == 1){
            if(unit.UnitSO.IsInRedArmy){
                return true;
            }
            return false;
        }
        else{
            if(unit.UnitSO.IsInRedArmy){
                return false;
            }
            return true;
        }
    }
}

/// <summary>
/// Toutes les infos liées aux cartes events
/// </summary>
[System.Serializable]
public class EventCardList
{
    public EventCardClass _eventSO = null;

    //Carte Event du Joueur 1
    public List<MYthsAndSteel_Enum.EventCard> _eventCardRedPlayer = new List<MYthsAndSteel_Enum.EventCard>();

    //Carte Event du Joueur 2
    public List<MYthsAndSteel_Enum.EventCard> _eventCardBluePlayer = new List<MYthsAndSteel_Enum.EventCard>();

    //Carte Gam du Joueur 1
    public List<GameObject> _eventGamRedPlayer = null;

    //Carte Gam du Joueur 2
    public List<GameObject> _eventGamBluePlayer = null;
}
