using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBluePlayer : ChargeOrgone
{


    public override void ChargeOrgone1(int cost)
    {
        Debug.Log("B1");
    }
    public override void ChargeOrgone3(int cost)
    {
        Debug.Log("B3");
    }

    #region Charge 5 D'orgone
    public override void ChargeOrgone5(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(4, 2))
        {
            OrgoneManager.Instance.ischarge5Blue = true;
            Debug.Log("B5");
            GameManager.Instance._eventCall += UseCharge5BluePlayer;
            GameManager.Instance.StartEventModeUnit(1, false, PlayerScript.Instance.UnitRef.UnitListBluePlayer, "Charge 3 d'orgone Bleu", "Êtes vous sûr de vouloir d'utiliser la charge 3 d'orgone ?");
        }
    }

    void UseCharge5BluePlayer()
    {
        Debug.Log("UseCharge");
        List<GameObject> SelectTileList = new List<GameObject>();
        foreach (GameObject gam in TilesManager.Instance.TileList)
        {
            TileScript tilescript = gam.GetComponent<TileScript>();
            Debug.Log(tilescript.TileId);
            if (tilescript.Unit == null)
            {
                Debug.Log("Unit est null");
                foreach (MYthsAndSteel_Enum.TerrainType i in tilescript.TerrainEffectList)
                {
                    if (i != MYthsAndSteel_Enum.TerrainType.Point_de_ressource && i != MYthsAndSteel_Enum.TerrainType.Point_Objectif && i != MYthsAndSteel_Enum.TerrainType.UsineBleu && i != MYthsAndSteel_Enum.TerrainType.UsineRouge)
                    {
                        SelectTileList.Add(gam);
                        Debug.Log("break");
                        break;
                    }
                }
            }
        }
        GameManager.Instance._eventCall -= UseCharge5BluePlayer;
        //if (GameManager.Instance._eventCall == null) Debug.Log("Call null"); else Debug.Log("Call non null");
        GameManager.Instance.StartEventModeTiles(1, false, SelectTileList, "Tile de tp", "Etes vous sur de validé cette case de tp?");
        //if (GameManager.Instance._eventCall == null) Debug.Log("Call null"); else Debug.Log("Call non null");
        GameManager.Instance._eventCall += DoneCharge5Blueplayer;
    }
    void DoneCharge5Blueplayer()
    {
        Debug.Log("Tu as atin le bonheur");
        GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().ActualTiledId = GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().TileId;
        GameManager.Instance.StopEventModeTile();

    }
    #endregion

}
