using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
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
            GetComponent<Animator>().SetBool("Feu", false);
            GetComponent<Animator>().SetBool("Brasier", true);
        }
        else if(TurnLeft == 1)
        {
            GetComponent<Animator>().SetTrigger("Out");
            GetComponent<Animator>().SetBool("Brasier", false);
            GetComponent<Animator>().SetBool("Feu", true);
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
}
