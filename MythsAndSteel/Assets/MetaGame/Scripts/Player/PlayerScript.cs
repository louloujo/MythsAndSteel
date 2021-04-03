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
    //Carte Event du Joueur 1
    [SerializeField] private List<MYthsAndSteel_Enum.EventCard> _eventCardRedPlayer = new List<MYthsAndSteel_Enum.EventCard>();
    public List<MYthsAndSteel_Enum.EventCard> EventCardRedPlayer => _eventCardRedPlayer;

    //Carte Event du Joueur 2
    [SerializeField] private List<MYthsAndSteel_Enum.EventCard> _eventCardBluePlayer = new List<MYthsAndSteel_Enum.EventCard>();
    public List<MYthsAndSteel_Enum.EventCard> EventCardBluePlayer => _eventCardBluePlayer;

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
    public void GiveEventCard(int player)
    {
        List<MYthsAndSteel_Enum.EventCard> combineEventCards = new List<MYthsAndSteel_Enum.EventCard>();
        combineEventCards.AddRange(_eventCardRedPlayer);
        combineEventCards.AddRange(_eventCardBluePlayer);

        int randomCard = UnityEngine.Random.Range(0, GameManager.Instance.EventCardSO.NumberOfEventCard + 1);
        Debug.Log((MYthsAndSteel_Enum.EventCard)randomCard);

        if(combineEventCards.Contains((MYthsAndSteel_Enum.EventCard)randomCard))
        {
            GiveEventCard(player);
            return;
        }

        AddEventCard(player, (MYthsAndSteel_Enum.EventCard)randomCard);
    }

    /// <summary>
    /// Ajoute une carte spécifique au joueur
    /// </summary>
    /// <param name="player"></param>
    /// <param name="card"></param>
    public void AddEventCard(int player, MYthsAndSteel_Enum.EventCard card)
    {
        if(player == 1)
        {
            _eventCardRedPlayer.Add(card);
        }
        else
        {
            _eventCardBluePlayer.Add(card);
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
