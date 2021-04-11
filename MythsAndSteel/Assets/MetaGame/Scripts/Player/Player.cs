using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    [Header("ARMY INFO")]
    //nom de l'armée
    public string ArmyName;

    [Header("ACTIVATION")]
    //Nombre d'activation restante
    public int ActivationLeft; 
    //Valeur de la carte activation posée
    public int ActivationCardValue; 

    [Header("ORGONE")]
    //Nombre de charges d'orgone actuel
    public int OrgoneValue; 
    //Nombre de pouvoirs d'orgone encore activable
    public int OrgonePowerLeft; 
    //Permet de se souvenir de la dernière valeur d'orgone avant Update
    public int LastKnownOrgoneValue; 
    //Tile qui correspond au centre de la zone d'Orgone
    public GameObject TileCentreZoneOrgone; 

    [Header("RESSOURCE")]
    //Nombre de Ressources actuel
    public int Ressource;

    [Header("OBJECTIF")]
    //Nombre d'objectif actuellement capturé
    public int GoalCapturePointsNumber; 

    public bool HasCreateUnit; //est ce que le joueur a créer une unité durant sont tour

    /// <summary>
    /// Check si l'orgone va exploser
    /// </summary>
    /// <returns></returns>
    public bool OrgoneExplose(){
        return OrgoneValue > 5 ? true : false;
    }

    /// <summary>
    /// Change la valeur (pos/neg) de la jauge d'orgone.
    /// </summary>
    /// <param name="Value">Valeur positive ou négative.</param>
    public void ChangeOrgone(int Value, int player){
        OrgoneValue += Value;
        UpdateOrgoneUI(player);
    }

    /// <summary>
    /// Update l'UI de la jauge d'orgone en fonction du nombre de charge
    /// </summary>
    public void UpdateOrgoneUI(int player){
        if(player == 1){
            foreach(Image img in OrgoneManager.Instance.RedPlayerCharge){
                img.enabled = false;
            }

            for(int i = 0; i < OrgoneValue; i++){
                OrgoneManager.Instance.RedPlayerCharge[i].enabled = true;
            }
        }
        else{
            foreach(Image img in OrgoneManager.Instance.BluePlayerCharge)
            {
                img.enabled = false;
            }

            for(int i = 0; i < OrgoneValue; i++)
            {
                OrgoneManager.Instance.BluePlayerCharge[i].enabled = true;
            }
        }
    }

    /// <summary>
    /// Appelle l'explosion d'orgone
    /// </summary>
    public void MakeOrgoneExplosion(){
        // oskour Paul !
    } 
}
