using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyButtons;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitScript : MonoBehaviour{
    #region Variables
    [Header("Stats de base de l'unité")]
    //Scriptable qui contient les stats de base de l'unité
    [SerializeField] Unit_SO _unitSO;
    public Unit_SO UnitSO => _unitSO;


    [Header("Stats en jeu de l'unité")]
    //Vie actuelle
    [SerializeField] int _life; 
    public int Life => _life;

    // Bouclier actuelle
    [SerializeField] int _shield; 
    public int Shield => _shield;
    
    //Portée
    [SerializeField] int _attackRange; 
    public int AttackRange => _attackRange;
    
    //Vitesse de déplacement
    [SerializeField] int _moveSpeed; 
    public int MoveSpeed => _moveSpeed;

    // Coût de création
    [SerializeField] int _creationCost; 
    public int CreationCost => _creationCost;

    //Dégats minimum infligé
    [SerializeField] int _DamageMinimum;
    public int DamageMinimum => _DamageMinimum;

    [SerializeField] Vector2 _NumberRangeMin;
    public Vector2 NumberRangeMin => _NumberRangeMin;

    //Dégats maximum infligé
    [SerializeField] int _DamageMaximum;
    public int DamageMaximum => _DamageMaximum;

    [SerializeField] Vector2 _NumberRangeMax;
    public Vector2 NumberRangeMax => _NumberRangeMax;



    [Header("Stats non nécéssaire")]
    // Déplacement réstant de l'unité durant cette activation
    [SerializeField] int _moveLeft;
    public int MoveLeft => _moveLeft;

    //Valeur (id) de la case sur laquelle se trouve l'unité
    [SerializeField] int _actualTileld;
    public int ActualTiledId => _actualTileld;

    //déplacement actuel de l'unité pour la fonction "MoveWithPath"
    int _i;
    public int i => _i;

    //lorsque le joueur a fini d'utiliser tous ses points de déplacement
    [SerializeField] bool _isMoveDone;
    public bool IsMoveDone => _isMoveDone;

    //lorsque le joueur a effectué soit une attaque soit un pouvoir actif
    [SerializeField] bool _isActionDone;
    public bool IsActionDone => _isActionDone;

    //lorsque l'activation a totalement été finie
    [SerializeField] bool _isActivationDone;
    public bool IsActivationDone => _isActivationDone;

    //est ce que cette unité est utilisable par l'adversaire
    bool _usefullForOpponent;
    public bool UsefullForOpponent => _usefullForOpponent;

    //est ce que l'unité peut prendre des dégâts (carte event "Cessez le feu")
    bool _canTakeDamage;
    public bool CanTakeDamage => _canTakeDamage;

    //est ce que l'unité peut attaquer (carte event "Cessez le feu")
    bool _canFight;
    public bool CanFight => _canFight;

    //est ce que l'unité peut prendre des objectifs (carte event "Cessez le feu")
    bool _canTakeGoal;
    public bool CanTakeGoal => _canTakeGoal;

    //Est-ce que l'unité est en vie ?
    bool _isLiving;
    public bool isLiving => _isLiving;

    //Chemin que l'unité va emprunter
    [SerializeField] List<int> _pathtomake;
    public List<int> Pathtomake => _pathtomake;

    //A CHANGER AU BON ENDROIT QUAND CE SERA FAIT
    //list qui va chercher les text enfant dans la hiérarchie pour l'UI
    Text[] allchildren;

    #endregion Variables

/*    void Start()
    {
        //------------ Assign Les Stat du scriptable a l'unité et aux text de l'UI ------------------
        allchildren = this.transform.GetComponentsInChildren<Text>();

        UpdateUnitStat();

        allchildren[0].text = "Vie : " + _life.ToString() + " / " + _unitSO.LifeMax.ToString() ;
        allchildren[1].text = "Bouclier : " + _shield.ToString() + " / " + _unitSO.ShieldMax.ToString();
        allchildren[2].text = "Portée : " + _attackRange.ToString();
        allchildren[3].text = "Déplacement : " + _moveSpeed.ToString();
        allchildren[5].text = _unitSO.Description.ToString();

        //------------------------------------------------------------------------------------------------

    }*/

    private void Update(){
       /* //-------------Actualise l'affichage des valeurs dans L'UI------------------------------------------------------
        allchildren[0].text = "Vie : " + _life.ToString() + " / " + _unitSO.LifeMax.ToString();
        allchildren[1].text = "Bouclier : " + _shield.ToString() + " / " + _unitSO.ShieldMax.ToString();
        allchildren[2].text = "Portée : " + _attackRange.ToString();
        allchildren[3].text = "Déplacement : " + _moveSpeed.ToString();
        allchildren[5].text = _unitSO.Description.ToString();
        //-------------------------------------------------------------------------------------------------*/

        //Test--------------------------------------
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GiveLife(1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TakeDamage(1);
        }
        //----------------------------------------
    }

    #region LifeMethods
    /// <summary>
    /// Rajoute de la vie au joueur
    /// </summary>
    /// <param name="Lifeadd"></param>
    public virtual void GiveLife(int Lifeadd){
        _life += Lifeadd;
    }

    /// <summary>
    /// Fait perdre de la vie au joueur
    /// </summary>
    /// <param name="Damage"></param>
    public virtual void TakeDamage(int Damage){
        _life -= Damage;
        CheckLife();
    }

    /// <summary>
    /// Check la vie du joueur
    /// </summary>
    void CheckLife(){
        if (_life <= 0){
            _isLiving = false; //A voir si c'est nécéssaire
            Death();
        }
    }
    #endregion LifeMethods

    /// <summary>
    /// Tue l'unité
    /// </summary>
    public virtual void Death(){
        Destroy(gameObject);
        Debug.Log("Unité Détruite");
    }

    [Button]
    /// <summary>
    /// Update les stats de l'unité avec les stats de base
    /// </summary>
    public virtual void UpdateUnitStat(){
        //Si il n'y a pas de scriptable object alors ca arrete la fonction
        if(_unitSO == null) return;

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
    }

#if UNITY_EDITOR
    /// <summary>
    /// Lorsqu'une valeur est modifié dans l'inspecteur ca update le UnitScript a partir du scriptable
    /// </summary>
    private void OnValidate(){
        UpdateUnitStat();
    }
#endif
}

