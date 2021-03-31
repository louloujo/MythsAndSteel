using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class AttackUnit : MonoBehaviour
{
    int i = 5, j = 12, k = 2, l = 0, m = 0, n = 0; //  Information récupérés 
    float o, p, x; // 

    void Start() // 
    {
        o = Random.Range(1f, 6f);
        p = Random.Range(1f, 6f);
        x = o + p;
        Debug.Log(x);
    }

    private void Update()
    {
        ChooseAttackType(i, j, k, l, m, n, x);
    }

    void UnitAttack(int i, int j, int k, int l, int m, int n, float x)
    {
        if (x >= i && x <= j)
        {
            Debug.Log("Damage : " + k);
        }
        if (x >= l && x <= m)
        {
            Debug.Log("Damage : " + n);
        }
        if (x < i)
        {
            Debug.Log("Damage : " + 0);
        }
    }

    void UnitAttackOne(int i, int j, int k, float x)
    {
        if (x >= i && x <= j)
        {
            Debug.Log("Damage : " + k);
        }
        if (x < i)
        {
            Debug.Log("Damage : " + 0);
        }
    }

    void ChooseAttackType(int i, int j, int k, int l, int m, int n, float x)
    {
        if (l == 0 && m == 0 && l == 0)
        {
            UnitAttackOne(i, j, k, x);
            Debug.Log("Une range");
        }
        else
        {
            UnitAttack(i, j, k, l, m, n, x);
            Debug.Log("Deux range");
        }
    }
}
