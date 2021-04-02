using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoSingleton<PlayerScript>
{
    //List<MYthsAndSteel_Enum.EventCard> EventCardUse; Déjà présente normalement
    public List<UnitScript> Unites;//Liste des Unités
    public List<MYthsAndSteel_Enum.TypeUnite> DisactivateUnitType = new List<MYthsAndSteel_Enum.TypeUnite>();
    
    [SerializeField] bool _Army1WinAtTheEnd;
    public bool ArmyRedWinAtTheEnd => _Army1WinAtTheEnd;

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
                foreach (UnitScript us in Unites)
                {
                    if (us.UnitSO.IsInRedArmy)
                    {
                        return true;
                    }
                }
                break;
            case 2:
                foreach (UnitScript us in Unites)
                {
                    if (!us.UnitSO.IsInRedArmy)
                    {
                        return true;
                    }
                }
                break;
        }
        return false;
    }
    
}
