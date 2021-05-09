using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : TerrainParent
{
    [SerializeField] private int _Turnleft;
    public FireGestion FireG;

    public int TurnLeft
    {
        get
        {
            return _Turnleft;
        }
        set
        {
            _Turnleft = value;
            Check();
        }
    }

    private void Start()
    {
        Check();
    }
    public void Check()
    {
        if(TurnLeft == 2)
        {
            GetComponentInChildren<Animator>().SetBool("Feu", false);
            GetComponentInChildren<Animator>().SetBool("Brasier", true);

            if (GetComponentInParent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Feu))
            {
                GetComponentInParent<TileScript>().TerrainEffectList.Remove(MYthsAndSteel_Enum.TerrainType.Feu);
            }
            if (!GetComponentInParent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Brasier))
            {
                GetComponentInParent<TileScript>().TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Brasier);
            }
            GetComponentInParent<ChildEffect>().Type = MYthsAndSteel_Enum.TerrainType.Brasier;
        }
        else if(TurnLeft == 1)
        {
            GetComponentInChildren<Animator>().SetTrigger("Out");
            GetComponentInChildren<Animator>().SetBool("Brasier", false);
            GetComponentInChildren<Animator>().SetBool("Feu", true);

            if (GetComponentInParent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Brasier))
            {
                GetComponentInParent<TileScript>().TerrainEffectList.Remove(MYthsAndSteel_Enum.TerrainType.Brasier);
            }
            if (!GetComponentInParent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Feu))
            {
                GetComponentInParent<TileScript>().TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Feu);
            }
            GetComponent<ChildEffect>().Type = MYthsAndSteel_Enum.TerrainType.Feu;
        }
        else if(TurnLeft <= 0)
        {
            GetComponentInChildren<Animator>().SetTrigger("Out");
        }
    }

    public void CheckingEffector()
    {
        if(TurnLeft <= 0)
        {
            FireG.FireActive.Remove(this);
            GetComponentInParent<TileScript>().RemoveEffect(MYthsAndSteel_Enum.TerrainType.Feu);
            GetComponentInParent<TileScript>().RemoveEffect(MYthsAndSteel_Enum.TerrainType.Brasier);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (FireG.FireActive.Contains(this))
        {
            FireG.FireActive.Remove(this);
        }
    }

    public override void OnUnityAdd(UnitScript Unit)
    {
        if(GetComponent<ChildEffect>().Type == MYthsAndSteel_Enum.TerrainType.Brasier)
        {
            Unit.TakeDamage(5);
        }
        if (GetComponent<ChildEffect>().Type == MYthsAndSteel_Enum.TerrainType.Feu)
        {
            Unit.TakeDamage(1);
        }
        base.OnUnityAdd(Unit);
    }

    public override void ComingFromUp(UnitScript Unit)
    {
        base.ComingFromUp(Unit);
        Debug.Log(this.name + " Coming from up.");
    }

    public override int AttackRangeValue(int i = 0)
    {
        i = 3;
        return base.AttackRangeValue(i);
    }
}
