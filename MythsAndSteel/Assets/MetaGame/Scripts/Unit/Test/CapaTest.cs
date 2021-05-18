using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapaTest : Capacity
{
    [SerializeField] private FireGestion fr;



    public override void StartCpty()
    {
        List<GameObject> tile = new List<GameObject>();
        foreach(GameObject T in TilesManager.Instance.TileList)
        {
            if(T.GetComponent<TileScript>().Unit != RaycastManager.Instance.ActualUnitSelected)
            {
                tile.Add(T);
            }
        }


        GameManager.Instance._eventCall += EndCpty;
        GameManager.Instance._eventCallCancel += StopCpty;
        GameManager.Instance.StartEventModeTiles(1, GetComponent<UnitScript>().UnitSO.IsInRedArmy, tile, "Embrasement!", "Voulez-vous vraiment embraser cette case ?");
        
        base.StartCpty();
    }

    public override void StopCpty()
    {
        GameManager.Instance.StopEventModeTile();
        GameManager.Instance.TileChooseList.Clear();
        GetComponent<UnitScript>().StopCapacity(true);
        base.StopCpty();
    }

    public override void EndCpty()
    {
        GameManager.Instance._eventCall -= EndCpty;
        GetComponent<UnitScript>().EndCapacity();
        if(GameManager.Instance.TileChooseList.Count > 0)
        {
            fr.CreateFire(GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().TileId);
        }
        base.EndCpty();        
        GameManager.Instance.TileChooseList.Clear();
    }
}
