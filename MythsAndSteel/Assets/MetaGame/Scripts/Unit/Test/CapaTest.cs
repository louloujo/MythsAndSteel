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
        GameManager.Instance.StartEventModeTiles(2, GetComponent<UnitScript>().UnitSO.IsInRedArmy, tile, "Embrasement!", "Voulez-vous vraiment embraser cette case ?", true);
        base.StartCpty();
    }

    public override void StopCpty()
    {
        GameManager.Instance.TileChooseList.Clear();
        GetComponent<UnitScript>().StopCapacity(true);
        base.StopCpty();
    }

    public override void EndCpty()
    {
        GameManager.Instance._eventCall -= EndCpty;
        GetComponent<UnitScript>().EndCapacity();
        fr.CreateFire(GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().TileId);
        fr.CreateFire(GameManager.Instance.TileChooseList[1].GetComponent<TileScript>().TileId);

        base.EndCpty();        
        GameManager.Instance.TileChooseList.Clear();

    }
}
