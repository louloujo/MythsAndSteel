using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    List<GameObject> ArmyUnitsList; //Liste de toutes les unités de l'armée

    List<GameObject> CreatorPointsUnits; //Liste des unités qui permettent de créer des unités autour d'elle

    List<Unit_SO> CreableUnits; //Liste des unités créables grâce au menu renfort

    List<EventCard> EventCardInHand; //liste des cartes event dans la main du joueur

    public int EventsCardNumberLeft; //Nombre de cartes événements restantes

    public int OrgoneValue; //Nombre de charges d'orgone actuel

    public int Ressource; //Nombre de Ressources actuel

    public int ActivationLeft; //Nombre d'activation restante

    public int ActivationCardValue; //Valeur de la carte activation posée

    public int GoalCapturePointsNumber; //Nombre d'objectif actuellement capturé

    public int OrgonePowerLeft; //Nombre de pouvoirs d'orgone encore activable

    public int LastKnownOrgoneValue; //Permet de se souvenir de la dernière valeur d'orgone avant Update

    public GameObject TileCentreZoneOrgone; //Tile qui correspond au centre de la zone d'Orgone

    public string ArmyName; //nom de l'armée

    public bool HasCreateUnit; //est ce que le joueur a créer une unité durant sont tour

    public bool ActivationCardChoose;//Est ce que le joueur a choisit une carte activation

    public bool IsMovingOrgoneArea; //Est ce que le joueur est en train de bouger la zone d'orgone

    public bool HasMoveOrgoneArea; //Est que le joueur a déjà bougé la zone d'orgone

    public bool OrgoneExplose(){
        return OrgoneValue > 5 ? true : false;
    }

    /// <summary>
    /// Change la valeur (pos/neg) de la jauge d'orgone.
    /// </summary>
    /// <param name="Value">Valeur positive ou négative.</param>
    public void ChangeOrgone(int Value){
        OrgoneValue += Value;
        Debug.Log(OrgoneValue);
    }

    /// <summary>
    /// Update l'UI de la jauge d'orgone en fonction du nombre de charge
    /// </summary>
    public void UpdateOrgoneUI(){
        // oskour Paul !
    }

    /// <summary>
    /// Appelle l'explosion d'orgone
    /// </summary>
    public void MakeOrgoneExplosion(){
        // oskour Paul !
    } 
}
