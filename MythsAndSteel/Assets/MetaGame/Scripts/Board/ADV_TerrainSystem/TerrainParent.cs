using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainParent : MonoBehaviour
{

    public virtual void OnUnityAdd(UnitScript Unit)
    {

    }
    public virtual void OnUnityDown(UnitScript Unit)
    {

    }
    public virtual void ComingFromUp(UnitScript Unit)
    {

    }
    public virtual void ComingFromDown(UnitScript Unit)
    {

    }
    public virtual void ComingFromLeft(UnitScript Unit)
    {

    }
    public virtual void ComingFromRight(UnitScript Unit)
    {

    }
    public virtual void QuitToRight(UnitScript Unit)
    {

    }
    public virtual void QuitToLeft(UnitScript Unit)
    {

    }
    public virtual void QuitToUp(UnitScript Unit)
    {

    }
    public virtual void QuitToDown(UnitScript Unit)
    {

    }
    public virtual void UnithasBeenAttacked(UnitScript Unit)
    {

    }
    public virtual void UnithasBeenSelectedForAttack(UnitScript Unit)
    {

    }
    /// <summary>
    /// Effet appliqué à la case à la fin de chaque tour de jeu.
    /// Cette fonction est appelée même si il n'y a pas d'unité sur la case.
    /// </summary>
    /// <param name="Unit"></param>
    public virtual void EndTurnEffect(TileScript ts, UnitScript Unit = null)
    {

    }
}
