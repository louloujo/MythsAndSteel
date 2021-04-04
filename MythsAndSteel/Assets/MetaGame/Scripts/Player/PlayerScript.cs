using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoSingleton<PlayerScript>
{
    //Liste des Unités
    public List<GameObject> _unitList = new List<GameObject>();

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

        Debug.Log(newCard);

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
            EventCardList._eventCardRedPlayer.Add(card);
        }
        else if(player == 2){
            EventCardList._eventCardBluePlayer.Add(card);
        }
        else{
            Debug.LogError("vous essayez d'ajouter une carte event a un joueur qui n'existe pas");
        }

        CreateEventCard(player, card);
    }

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

        if(player == 1){
            EventCardList._eventGamRedPlayer.Add(newCard);
            _eventCardList._eventSO.UpdateVisualUI(EventCardList._eventGamRedPlayer, 1);
        }
        else if(player == 2){
            EventCardList._eventGamBluePlayer.Add(newCard);
            _eventCardList._eventSO.UpdateVisualUI(EventCardList._eventGamBluePlayer, 2);
        }
        else{
            Debug.LogError("vous essayez d'ajouter une carte event a un joueur qui n'existe pas");
        }
    }


    #endregion CarteEvent

    /// <summary>
    /// Est ce qu'il reste des unités dans l'armée du joueur
    /// </summary>
    /// <param name="Joueur"></param>
    /// <returns></returns>
    public bool CheckArmy(int Joueur)
    {
        switch (Joueur)
        {
            case 1:
                foreach (GameObject us in _unitList)
                {
                    if (us.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                    {
                        return true;
                    }
                }
                break;
            case 2:
                foreach (GameObject us in _unitList)
                {
                    if (!us.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                    {
                        return true;
                    }
                }
                break;
        }
        return false;
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
