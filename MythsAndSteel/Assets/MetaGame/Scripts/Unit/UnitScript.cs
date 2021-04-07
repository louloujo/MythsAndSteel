using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyButtons;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitScript : MonoBehaviour
{
    #region Variables
    [Header("--------------- STATS DE BASE DE L'UNITE ---------------")]
    //Scriptable qui contient les stats de base de l'unité
    [SerializeField] Unit_SO _unitSO;
    public Unit_SO UnitSO => _unitSO;

    [Header("------------------- VIE -------------------")]
    [Header("------------------- STAT EN JEU -------------------")]
    //Vie actuelle
    [SerializeField] int _life;
    public int Life => _life;

    // Bouclier actuelle
    [SerializeField] int _shield;
    public int Shield => _shield;

    [Header("-------------------- ATTAQUE -------------------")]
    //Portée
    [SerializeField] int _attackRange;
    public int AttackRange => _attackRange;
    public int AttackRangeBonus = 0;

    [Space]
    //Dégats minimum infligé
    [SerializeField] Vector2 _NumberRangeMin;
    public Vector2 NumberRangeMin => _NumberRangeMin;
    [SerializeField] int _DamageMinimum;
    public int DamageMinimum => _DamageMinimum;

    [Space]
    //Dégats maximum infligé
    [SerializeField] Vector2 _NumberRangeMax;
    public Vector2 NumberRangeMax => _NumberRangeMax;
    [SerializeField] int _DamageMaximum;
    public int DamageMaximum => _DamageMaximum;

    [Header("------------------- DEPLACEMENT -------------------")]
    //Vitesse de déplacement
    [SerializeField] int _moveSpeed;
    public int MoveSpeed => _moveSpeed;
    public int MoveSpeedBonus = 0;

    [Header("------------------- COUT DE CREATION -------------------" )]
    // Coût de création
    [SerializeField] int _creationCost;
    public int CreationCost => _creationCost;



    public int DiceBonus = 0;


    [Header("------------------- DEPLACEMENT RESTANT -------------------")]
    // Déplacement réstant de l'unité durant cette activation
    [SerializeField] int _moveLeft;
    public int MoveLeft
    {
        get
        {
            return _moveLeft;
        }
        set
        {
            _moveLeft = value;
        }
    }

    [Header("------------------- CASE DE L'UNITE -------------------")]
    //Valeur (id) de la case sur laquelle se trouve l'unité
    [SerializeField] int _actualTileld;
    public int ActualTiledId
    {
        get
        {
            return _actualTileld;
        }
        set
        {
            _actualTileld = value;
        }
    }

    //déplacement actuel de l'unité pour la fonction "MoveWithPath"
    int _i;
    public int i => _i;

    [Header("------------------- ACTIVATION UNITE -------------------")]
    //lorsque le joueur a fini d'utiliser tous ses points de déplacement
    [SerializeField] bool _isMoveDone;
    public bool IsMoveDone => _isMoveDone;

    //lorsque le joueur a effectué soit une attaque soit un pouvoir actif
    [SerializeField] bool _isActionDone;
    public bool IsActionDone => _isActionDone;

    //lorsque l'activation a totalement été finie
    [SerializeField] bool _isActivationDone;
    public bool IsActivationDone => _isActivationDone;

    [Header("------------------- CHEMIN DE DEPLACEMENT -------------------")]
    //Chemin que l'unité va emprunter
    [SerializeField] List<int> _pathtomake;
    public List<int> Pathtomake => _pathtomake;

    //A CHANGER AU BON ENDROIT QUAND CE SERA FAIT
    //list qui va chercher les text enfant dans la hiérarchie pour l'UI
    Text[] allchildren;

    [Header("------------------- STAUT DE L'UNITE -------------------")]
    //Statut que possède l'unité
    [SerializeField] private List<MYthsAndSteel_Enum.Statut> _unitStatus = new List<MYthsAndSteel_Enum.Statut>();
    public List<MYthsAndSteel_Enum.Statut> UnitStatus => _unitStatus;

    #endregion Variables

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GiveLife(1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TakeDamage(1);
        }
    }

    #region LifeMethods
    /// <summary>
    /// Rajoute de la vie au joueur
    /// </summary>
    /// <param name="Lifeadd"></param>
    public virtual void GiveLife(int Lifeadd)
    {
        _life += Lifeadd;
        if(_life > UnitSO.LifeMax){
            int shieldPlus = _life - UnitSO.LifeMax;
            _life = UnitSO.LifeMax;
            _shield += shieldPlus;
        }
    }

    /// <summary>
    /// Fait perdre de la vie au joueur
    /// </summary>
    /// <param name="Damage"></param>
    public virtual void TakeDamage(int Damage)
    {
        if(_shield > 0){
            _shield -= Damage;
            _life += _shield;
            CheckLife();
        }
        else
        {
            _life -= Damage;
            CheckLife();
        }
    }

    /// <summary>
    /// Check la vie du joueur
    /// </summary>
    void CheckLife()
    {
        if (_life <= 0)
        {
            Death();
        }
    }
    #endregion LifeMethods


    /// <summary>
    /// Tue l'unité
    /// </summary>
    public virtual void Death()
    {
        Destroy(gameObject);
        Debug.Log("Unité Détruite");
    }

    #region Statut
    public void AddStatutToUnit(MYthsAndSteel_Enum.Statut stat){
        _unitStatus.Add(stat);
    }

    #endregion Statut


    [Button]
    /// <summary>
    /// Update les stats de l'unité avec les stats de base
    /// </summary>
    public virtual void UpdateUnitStat()
    {
        //Si il n'y a pas de scriptable object alors ca arrete la fonction
        if (_unitSO == null) return;

        //Assigne les stats
        _life = _unitSO.LifeMax;
        _shield = _unitSO.ShieldMax;
        _attackRange = _unitSO.AttackRange;
        _moveSpeed = _unitSO.MoveSpeed;
        _creationCost = _unitSO.CreationCost;
        _DamageMinimum = _unitSO.DamageMinimum;
        _DamageMaximum = _unitSO.DamageMaximum;
        _NumberRangeMax = _unitSO.NumberRangeMax;
        _NumberRangeMin = _unitSO.NumberRangeMin;

        //Assigne le sprite de l'unité
        GetComponent<SpriteRenderer>().sprite = _unitSO.Sprite;

        ResetTurn();
    }

    /// <summary>
    /// Reset les valeurs nécéssaires pour un nouveau tour
    /// </summary>
    public virtual void ResetTurn(){
        _isActivationDone = false;
        _isMoveDone = false;
        _isActionDone = false;

        MoveSpeedBonus = 0;
        AttackRangeBonus = 0;

        _moveLeft = _unitSO.MoveSpeed;
    }

    public void checkMovementLeft()
    {
        if (_moveLeft == 0)
        {
            _isMoveDone = true;
        }
    }
}

