using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapaTest : Capacity
{
    [SerializeField] private FireGestion fr;



    public override void StartCpty()
    {
        int ressourcePlayer = GetComponent<UnitScript>().UnitSO.IsInRedArmy ? PlayerScript.Instance.RedPlayerInfos.Ressource : PlayerScript.Instance.BluePlayerInfos.Ressource;
        if (ressourcePlayer >= Capacity1Cost)
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

        }
    
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
        if (GetComponent<UnitScript>().UnitSO.IsInRedArmy)
        {
            PlayerScript.Instance.RedPlayerInfos.Ressource -= Capacity1Cost;
        }
        else
        {
            PlayerScript.Instance.BluePlayerInfos.Ressource -= Capacity1Cost;
        }
        
        GameManager.Instance._eventCall -= EndCpty;
        GetComponent<UnitScript>().EndCapacity();
        if(GameManager.Instance.TileChooseList.Count > 0)
        {
            foreach(int id in PlayerStatic.GetNeighbourDiag(GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().TileId, GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().Line, false))
            {
                fr.CreateFire(id);
            }
            fr.CreateFire(GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().TileId);
            
        }
        
        base.EndCpty();        
        GameManager.Instance.TileChooseList.Clear();
    }
}
