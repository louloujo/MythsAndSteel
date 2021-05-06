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
        Test();
    }
    public void Check()
    {
        if(TurnLeft == 2)
        {
            GetComponent<Animator>().SetBool("Feu", false);
            GetComponent<Animator>().SetBool("Brasier", true);

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
            GetComponent<Animator>().SetTrigger("Out");
            GetComponent<Animator>().SetBool("Brasier", false);
            GetComponent<Animator>().SetBool("Feu", true);

            if (GetComponentInParent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Brasier))
            {
                GetComponentInParent<TileScript>().TerrainEffectList.Remove(MYthsAndSteel_Enum.TerrainType.Brasier);
            }
            if (!GetComponentInParent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Feu))
            {
                GetComponentInParent<TileScript>().TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Feu);
            }
            GetComponentInParent<ChildEffect>().Type = MYthsAndSteel_Enum.TerrainType.Feu;
        }
        else if(TurnLeft <= 0)
        {
            GetComponent<Animator>().SetTrigger("Out");
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


    public void Test()
    {
        TerrainGestion.Instance.UnitModification(this);
    }

    public override void OnUnityAdd()
    {
        base.OnUnityAdd();
        Debug.Log("Test" + this.name);
    }
}
