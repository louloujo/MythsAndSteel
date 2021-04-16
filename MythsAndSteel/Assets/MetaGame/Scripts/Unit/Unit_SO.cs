using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Basic Unit")]
public class Unit_SO : ScriptableObject{
    [Header("Nom et Description")]
    public string UnitName; //Nom de l'unité
    public string Description; //Decritpion de l'unité

    [Header("Type de l'unité")]
    public MYthsAndSteel_Enum.TypeUnite typeUnite; //Type de l'unité

    [Header("Stat de base de l'unité")]
    public int LifeMax; //Vie de base de l'unité
    public int ShieldMax; //Bouclier Max de l'unité
    public int AttackRange; //Range d'attaque de l'unité
    public int MoveSpeed; //Vitesse de déplacement de l'unité
    public int CreationCost; //Cout de création de l'unité

    [Header("Attaque Minimum")]
    public int DamageMinimum;//Dégats minimum infligé
    public Vector2 NumberRangeMin;

    [Header("Attaque Maximum")]
    public int DamageMaximum;//Dégats maximum infligé
    public Vector2 NumberRangeMax;

    [Header("Son de l'unité")]
    public AudioClip Son1; //Son 1 de l'unité
    public AudioClip Son2; //Son 2 de l'unité

    [Header("Sprite de l'unité")]
    public Sprite Sprite; //Sprite de base de l'unité

    [Header("Infos Supplémentaire")]
    [Tooltip("Est ce que l'unité peut attaquer des cases vide?")]
    public bool CanAttackEmptyTile = false;
    [Tooltip("Est ce que si cette unité est tuée elle permet à l'adversaire de gagner?")]
    public bool UsefullToWin = false;
    [Tooltip("Est ce que cette unité fait parti de l'armée 1?")]
    public bool IsInRedArmy = true;
    public MYthsAndSteel_Enum.Attributs[] UnitAttributs = new MYthsAndSteel_Enum.Attributs[3];


    

}
