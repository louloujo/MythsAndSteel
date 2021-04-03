using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Event Scriptable")]
public class EventCardClass : ScriptableObject{
    //Nombre de cartes events
    [SerializeField] private int _numberOfEventCard = 0;
    public int NumberOfEventCard => _numberOfEventCard;

    //Liste des cartes events
    [SerializeField] private List<EventCard> _eventCardList = new List<EventCard>();
    public List<EventCard> EventCardList => _eventCardList;

    /// <summary>
    /// Carte event du pointeur optimisé
    /// </summary>
    public void PointeursLaserOptimisés(){

    }

    /// <summary>
    /// Call an event
    /// </summary>
    /// <param name="id"></param>
    public void CallEvent(int id)
    {
        switch(id)
        {
            case 0:
                PointeursLaserOptimisés();
                break;
        }
    }

    /// <summary>
    /// get a random event card and add it to the player
    /// </summary>
    /// <param name="PlayerId"></param>
    public void AddRandomEvent(int PlayerId)
    {
        int randomEvent = Random.Range(0, NumberOfEventCard + 1);
    }
}

/// <summary>
/// Class qui regroupe toutes les variables pour une carte event
/// </summary>
[System.Serializable]
public class EventCard {
    public string _eventName = "";
    [TextArea] public string _description = "";
    public MYthsAndSteel_Enum.EventCard _eventType = MYthsAndSteel_Enum.EventCard.Activation_de_nodus;
    public int _eventCost = 0;
    public bool _isEventInFinalGame = true;
}
