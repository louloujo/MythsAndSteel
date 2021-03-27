using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MYthsAndSteel_Enum : MonoBehaviour
{
    /// <summary>
    /// Enum pour le type des unités
    /// </summary>
    public enum TypeUnite{ 
        Infanterie, Vehicule, Artillerie, Mythe, Leader, Mecha
    }

    /// <summary>
    /// Enum pour els phases du jeu
    /// </summary>
    public enum PhaseDeJeu{
        Debut, Activation, OrgoneJ1, ActionJ1, OrgoneJ2, ActionJ2, Strategie
    }

    /// <summary>
    /// Enum pour les cartes events
    /// </summary>
    public enum EventCard{
        J1Faction1, J1Faction2, J1Faction3, J2Faction1, J2Faction2, J2Faction3
    }

    /// <summary>
    /// Les différentes stats modifiables
    /// </summary>
    public enum Stat{
        Attaque, vie, portee
    }
}
