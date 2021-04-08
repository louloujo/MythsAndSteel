using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Orgone/Basic Orgone Power")]
public class ChargeOrgone : ScriptableObject
{
    //Est ce que les charges d'orgone appartient au joueur rouge
    [SerializeField] private bool _isRedOrgonePower = false;

    /// <summary>
    /// Fonction qui permet de créer une charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void ChargeOrgone1(int cost){
        if(!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de créer une deuxième charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void ChargeOrgone2(int cost){
        if(!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de créer une troisième charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void ChargeOrgone3(int cost){
        if(!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de créer une quatrième charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void ChargeOrgone4(int cost){
        if(!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de créer une cinquième charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void ChargeOrgone5(int cost){
        if(!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }
}
