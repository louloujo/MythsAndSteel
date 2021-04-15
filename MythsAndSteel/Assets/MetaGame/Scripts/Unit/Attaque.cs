using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attaque : MonoSingleton<Attaque>
{
    #region Variables
    [SerializeField] private int[] neighbourValue; // +1 +9 +10...

    public List<int> _newNeighbourId => newNeighbourId;
    [SerializeField] private List<int> newNeighbourId = new List<int>(); // Voisins atteignables avec le range de l'unité.

    public List<int> _selectedTileId => selectedTileId;
    [SerializeField] private List<int> selectedTileId = new List<int>(); // Cases selectionnées par le joueur.

    //Est ce que l'unité a commencé à choisir son déplacement ?
    [SerializeField] private bool _isInAttack;
    public bool IsInAttack
    {
        get
        {
            return _isInAttack;
        }
        set
        {
            _isInAttack = value;
        }
    }

    //Est ce qu'une unité est sélectionnée ?
    [SerializeField] private bool _selected;
    public bool Selected
    {
        get
        {
            return _selected;
        }
        set
        {
            _selected = value;
        }
    }

    // Vie de l'ennemi ciblé 
    [SerializeField] int _EnnemyLife;
    public int Life => _EnnemyLife;

    //Portée d'attaque
    [SerializeField] int _attackRange;
    public int AttackRange => _attackRange;

    //Dégats minimum infligés 
    [SerializeField] int _damageMinimum;
    public int DamageMinimum => _damageMinimum;

    // Range d'attaque (-) 
    [SerializeField] Vector2 _numberRangeMin;
    public Vector2 NumberRangeMin => _numberRangeMin;

    // Dégats maximum infligés
    [SerializeField] int _damageMaximum;
    public int DamageMaximum => _damageMaximum;

    // Range d'attaque (+) 
    [SerializeField] Vector2 _numberRangeMax;
    public Vector2 NumberRangeMax => _numberRangeMax;

    // Range d'attaque (+) 
    [SerializeField] bool _isActionDone;
    public bool IsActionDone => _isActionDone;


    float firstDiceFloat, secondDiceFloat;
    int firstDiceInt, secondDiceInt, DiceResult;

    GameObject selectedUnit;
    GameObject selectedUnitEnnemy;

    [Header("SPRITES POUR LES CASES")]
    [SerializeField] private Sprite _selectedSprite = null;

    public Sprite selectedSprite
    {
        get
        {
            return _selectedSprite;
        }
    }

    #endregion Variables

    [EasyButtons.Button]
    void Randomdice()
    {
        firstDiceFloat = Random.Range(1f, 7f);
        secondDiceFloat = Random.Range(1f, 7f);
        firstDiceInt = (int)firstDiceFloat;
        secondDiceInt = (int)secondDiceFloat;
        //DiceResult = firstDiceInt + secondDiceInt + selectedUnit.GetComponent<UnitScript>().DiceBonus;
        DiceResult = firstDiceInt + secondDiceInt;

        RandomMore();

        Debug.Log("Dice Result : " + DiceResult);
    }

    void UnitAttackOneRange(Vector2 _numberRangeMin, int _damageMinimum, int DiceResult)
    {
        if (DiceResult >= _numberRangeMin.x && DiceResult <= _numberRangeMin.y)
        {
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMinimum);
            Debug.Log("Damage : " + _damageMinimum);
        }
        if (DiceResult < _numberRangeMin.x)
        {
            Debug.Log("Damage : " + null);
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(0);
        }
        AnimationUpdate();
    }
    void UnitAttackTwoRanges(Vector2 _numberRangeMin, int _damageMinimum, Vector2 _numberRangeMax, int _damageMaximum, int DiceResult)
    {
        if (DiceResult >= _numberRangeMin.x && DiceResult <= _numberRangeMin.y)
        {
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMinimum);
            Debug.Log("Damage : " + _damageMinimum);
        }
        if (DiceResult >= _numberRangeMax.x && DiceResult <= _numberRangeMax.y)
        {
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMaximum);
            Debug.Log("Damage : " + _damageMaximum);
        }
        if (DiceResult < _numberRangeMin.x)
        {
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(0);
            Debug.Log("Damage : " + null);
        }
        AnimationUpdate();
    }
    void AnimationUpdate()
    {
        GameObject ActualUnit = RaycastManager.Instance.ActualUnitSelected;
        GameObject ActualEnemy = selectedUnitEnnemy;

        float X = ActualEnemy.transform.position.x - ActualUnit.transform.position.x; 
        float Y = ActualEnemy.transform.position.y - ActualUnit.transform.position.y;

        if (X >= 0)
        {
            if (Mathf.Abs(X) > Mathf.Abs(Y))
            {
                ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 1); //right
                ActualUnit.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (Mathf.Abs(X) <= Mathf.Abs(Y))
            {
                if (Y > 0)
                {
                    ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 2); // up
                }
                else if (Y < 0)
                {
                    ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 3); // down
                }
            }
        }
        if (X < 0)
        {
            if (Mathf.Abs(X) > Mathf.Abs(Y))
            {
                ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 1); // left
                ActualUnit.GetComponent<SpriteRenderer>().flipX = false; 
            }
            else if (Mathf.Abs(X) <= Mathf.Abs(Y))
            {
                if (Y > 0)
                {
                    ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 2); // up
                }
                else if (Y < 0)
                {
                    ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 3); // down
                }
            }
        }
        ActualUnit.GetComponent<UnitScript>().Animation.SetBool("Attack", true);
        StartCoroutine(AnimationWait(ActualUnit.GetComponent<UnitScript>().Animation, "Attack"));
    }

    public IEnumerator AnimationWait(Animator AnimToWait, string BoolName)
    {
        if(AnimToWait.runtimeAnimatorController != null)
        {
            yield return new WaitForSeconds(AnimToWait.runtimeAnimatorController.animationClips[0].length);
            AnimToWait.SetBool(BoolName, false);
        }
    }

    void ChooseAttackType(Vector2 _numberRangeMin, int _damageMinimum, Vector2 _numberRangeMax, int _damageMaximum, int DiceResult)
    {
        if (_numberRangeMax.x == 0 && _numberRangeMax.y == 0)
        {
            UnitAttackOneRange(_numberRangeMin, _damageMinimum, DiceResult);
        }

        else
        {
            UnitAttackTwoRanges(_numberRangeMin, _damageMinimum, _numberRangeMax, _damageMaximum, DiceResult);
        }
    }

    /// <summary>
    /// Highlight des cases dans la range d'attaque de l'unité
    /// </summary>
    /// <param name="tileId"></param>
    /// <param name="Range"></param>
    public void Highlight(int tileId, int Range)
    {
        if (Range > 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;

                if (TileSc.Unit != null)
                {
                    if (GameManager.Instance.IsPlayerRedTurn == TileSc.Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                    {
                        i = true;
                    }
                }

                if (!i)
                {
                    TileSc.ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect, _selectedSprite);
                    if (!newNeighbourId.Contains(ID))
                    {
                        newNeighbourId.Add(ID);
                    }
                    Highlight(ID, Range - 1);
                }
            }
        }
    }

    /// <summary>
    /// Ajoute plus d'aléatoire aux lancés de dé
    /// </summary>
    private void RandomMore()
    {
        if ((GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.dontTouchThis) ||
        (!GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.dontTouchThis))
        {
            DiceResult += 3;
            if (DiceResult > 12)
            {
                DiceResult = 12;
            }
        }
    }

    /// <summary>
    /// Vérifie si l'unité selectionné peut attaqué + récupère la portée de l'unité
    /// </summary>
    public void StartAttackSelectionUnit()
    {
        GameObject tileSelected = RaycastManager.Instance.ActualTileSelected;

        if (tileSelected != null)
        {
            selectedUnit = tileSelected.GetComponent<TileScript>().Unit;
            if (!selectedUnit.GetComponent<UnitScript>()._isActionDone)
            {
                Debug.Log(selectedUnit);
                _selected = true;
                GetStats();
                StartAttack(TilesManager.Instance.TileList.IndexOf(tileSelected), selectedUnit.GetComponent<UnitScript>().AttackRange + selectedUnit.GetComponent<UnitScript>().AttackRangeBonus);
            }
            else
            {
                _selected = false;
            }
        }
        else
        {
            _selected = false;
        }
    }

    /// <summary>
    /// Prépare l'Highlight des tiles ciblables & passe le statut de l'unité en -> _isInAttack
    /// </summary>
    /// <param name="tileId"></param>
    /// <param name="Range"></param>
    public void StartAttack(int tileId, int Range)
    {
        if (!_isInAttack)
        {
            _isInAttack = true;
            selectedTileId.Add(tileId);
            List<int> ID = new List<int>();
            ID.Add(tileId);

            // Lance l'highlight des cases dans la range de l'unité.
            Highlight(tileId, Range);
        }
    }

    /// <summary>
    /// Arrête l'attaque de l'unité select (UI + possibilité d'attaquer
    /// </summary>
    public void StopAttack()
    {
        foreach (int Neighbour in newNeighbourId) // Supprime toutes les tiles.
        {
            if (TilesManager.Instance.TileList[Neighbour] != null)
            {
                TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect);
            }
        }

        // Clear de toutes les listes et stats
        selectedTileId.Clear();
        newNeighbourId.Clear();

        _isInAttack = false;
        _selected = false;

        DiceResult = 0;
        firstDiceFloat = 0f;
        secondDiceFloat = 0f;
        firstDiceInt = 0;
        secondDiceInt = 0;
        _attackRange = 0;
        _damageMinimum = 0;
        _damageMaximum = 0;
        _numberRangeMin.x = 0;
        _numberRangeMin.y = 0;
        _numberRangeMax.x = 0;
        _numberRangeMax.y = 0;

        RaycastManager.Instance.ActualTileSelected = null;
    }

    public void Attack(int tileId)
    {
        GameObject TileSelectedForAttack = TilesManager.Instance.TileList[tileId];
        if (_isInAttack)
        {
            if (TileSelectedForAttack != null && newNeighbourId.Contains(tileId))
            {
                selectedUnitEnnemy = TileSelectedForAttack.GetComponent<TileScript>().Unit;
                if (selectedUnitEnnemy != null)
                {
                    _EnnemyLife = selectedUnitEnnemy.GetComponent<UnitScript>().Life;
                    ApplyAttack();
                }
                else
                {
                    StopAttack();
                }
            }
            else
            {
                StopAttack();
            }
        }
        else
        {
            StopAttack();
        }
    }

    public void GetStats()
    {
        _attackRange = selectedUnit.GetComponent<UnitScript>().AttackRange; // Récupération de la Portée
        _damageMinimum = selectedUnit.GetComponent<UnitScript>().DamageMinimum; // Récupération des Dégats Maximum
        _damageMaximum = selectedUnit.GetComponent<UnitScript>().DamageMaximum; // Dégats Minimums
        _numberRangeMin.x = selectedUnit.GetComponent<UnitScript>().NumberRangeMin.x; // Récupération de la Range min - x
        _numberRangeMin.y = selectedUnit.GetComponent<UnitScript>().NumberRangeMin.y; // Récupération de la Range min - y 
        _numberRangeMax.x = selectedUnit.GetComponent<UnitScript>().NumberRangeMax.x; // Récupération de la Range min - x
        _numberRangeMax.y = selectedUnit.GetComponent<UnitScript>().NumberRangeMax.y; // Récupération de la Range min - y

        // Applique les bonus/malus de terrains
        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Bosquet, selectedUnit.GetComponent<UnitScript>().ActualTiledId))
        {
            _numberRangeMin.x += 1;
            _numberRangeMin.y += 1;
            _numberRangeMax.x += 1;
            Debug.Log("Error");
        }

        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Colline, selectedUnit.GetComponent<UnitScript>().ActualTiledId))
        {
            selectedUnit.GetComponent<UnitScript>().AttackRangeBonus = 1;
            Debug.Log("Error");
        }

        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Plage, selectedUnit.GetComponent<UnitScript>().ActualTiledId) && selectedUnit.GetComponent<Unit_SO>().typeUnite == MYthsAndSteel_Enum.TypeUnite.Infanterie)
        {
            _numberRangeMin.x += -2;
            _numberRangeMin.y += -1;
            _numberRangeMax.x += -1;
            Debug.Log("Error");
        }

        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, selectedUnit.GetComponent<UnitScript>().ActualTiledId))
        {
            selectedUnit.GetComponent<UnitScript>().AttackRangeBonus = 1;
            Debug.Log("Error");
        }
    }

    public void ApplyAttack()
    {
        Randomdice();
        ChooseAttackType(_numberRangeMin, _damageMinimum, _numberRangeMax, _damageMaximum, DiceResult);
        StopAttack();
        IsInAttack = false;

        selectedUnit.GetComponent<UnitScript>()._isActionDone = true;
        selectedUnit.GetComponent<UnitScript>().checkActivation();
    }
}