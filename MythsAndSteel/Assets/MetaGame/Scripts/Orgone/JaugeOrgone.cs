using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Tout ce qui est dans ce script est à transférer le Player Class ! 
 */
public class JaugeOrgone : MonoSingleton<JaugeOrgone>
{
//Variable à mettre dans la Class Player
  public int OrgonePowerLeftRedPlayer = 1;
    public int OrgonePowerLeftBluePlayer = 1;
    public int ValueOrgoneRedPlayer = 3;
    public int ValueOrgoneBluePlayer = 4;
    public bool showPanelValidate = true;

    /// <summary>
    /// Permet de connaitre la nouvelle valeur de la jauge d'orgone en fonction d'un variation positif ou négatif
    /// </summary>
    public int ChangeOrgone(int currentValue, int fluctuation)
    {
        int newValue = currentValue + fluctuation;
        return newValue;
    }
    /// <summary>
    /// Détermine et applique l'animation de l'UI de la jauge d'orgone. A DÉTERMINER !
    /// </summary>
    public void UpdateOrgoneUI()
    {
        Debug.Log(OrgonePowerLeftRedPlayer);
        Debug.Log(OrgonePowerLeftBluePlayer);
        /* Pour les animations voici ma proposition
         il faudrait faire une liste de boutons avec les boutons d'une jauge d'orgone
         il faudrait la valeur actuelle de l'orgone 
         et la variation
         on applique ensuite un for qui dépend qui s'arrete quand on a appliquer tout la fluctuation de l'orgone et 
        qui a chaque fois applique l'animation pour chaque boutons concernés
        on aurait du coup un valeur spécifique temporaire qui corresponderait à la valeur actuelle qui se baisse au fur et à mesure pour comptabiliser le nombre d'animation qui reste à faire
        
         */ 
    }

}
