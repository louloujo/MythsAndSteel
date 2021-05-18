using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeRedPlayer : ChargeOrgone
{
    [SerializeField] GameObject mouseCommand;
    [SerializeField] GameObject PlayerInstance;
    public List<GameObject> UnitListForOrgone = new List<GameObject>();

    #region Charge 1 d'Orgone
    public override void ChargeOrgone1(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(1, 1))
        {
            List<GameObject> unitList = new List<GameObject>();

            unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListRedPlayer);

            GameManager.Instance.StartEventModeUnit(1, true, unitList, "Bombardement Aérien", "Êtes-vous sur de vouloir infliger des dégâts à cette unité?");
            GameManager.Instance._eventCall += UseChargeOrgone1;

        }
    }

    void ChooseUnitCharge1()
    {
    }
    void UseChargeOrgone1()
    {
        List<GameObject> tileList = new List<GameObject>();

        List<int> unitNeigh = PlayerStatic.GetNeighbourDiag(GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false);
        foreach (int i in unitNeigh)
        {
            tileList.Add(TilesManager.Instance.TileList[i]);
        }

        GameManager.Instance.StartEventModeTiles(1, true, tileList, "Bombardement Aérien", "Êtes-vous sur de vouloir déplacer l'unité attaquée sur cette case?");
        GameManager.Instance._eventCall += MoveUnitChageOgone1;
        tileList.Clear();
    }

    void MoveUnitChageOgone1()
    {
        Debug.Log("Call");
        GameManager.Instance._eventCall = null;

        while (GameManager.Instance.UnitChooseList[0].transform.position != GameManager.Instance.TileChooseList[0].transform.position)
        {
            GameManager.Instance.UnitChooseList[0].transform.position = Vector3.MoveTowards(GameManager.Instance.UnitChooseList[0].transform.position, GameManager.Instance.TileChooseList[0].transform.position, .7f);
            GameManager.Instance._waitEvent -= MoveUnitChageOgone1;
            GameManager.Instance._waitEvent += MoveUnitChageOgone1;
            GameManager.Instance.WaitToMove(.025f);
            return;
        }

        GameManager.Instance._waitEvent -= MoveUnitChageOgone1;
        TilesManager.Instance.TileList[GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().RemoveUnitFromTile();
        GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[0]);

        GameManager.Instance.TileChooseList.Clear();
        GameManager.Instance.UnitChooseList.Clear();
    }
    #endregion

    #region Charge 3 d'Orgone
    public override void ChargeOrgone3(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(3, 1))
        {
            Debug.Log("R3Laucnhed");
            GameManager.Instance._eventCall += UseCharge3RedPlayer;
            UIInstance.Instance.ShowValidationPanel("Charge 3 D'orgone", "Êtes vous sûr de vouloir d'utiliser la charge 3 d'orgone ?");
        }

        void UseCharge3RedPlayer()
        {
            Debug.Log("R3 USE");
            PlayerScript.Instance.RedPlayerInfos.EventUseLeft += 1;
            PlayerScript.Instance.RedPlayerInfos.OrgoneValue -= 3;
            Debug.Log("R3 finish");
            PlayerScript.Instance.RedPlayerInfos.UpdateOrgoneUI(1);
        }

    }
    #endregion

    #region Charge 5 d'Orgone
    public override void ChargeOrgone5(int cost)
    {
        Debug.Log("R5");
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(4, 1))
        {
            Debug.Log("R5Laucnhed");
            GameManager.Instance._eventCall += UseCharge5RedPlayer;
            GameManager.Instance._eventCall();
        }

    }

    void UseCharge5RedPlayer()
    {
        OrgoneManager.Instance.DoingOrgoneCharge = true;
        List<GameObject> TempSelectablelist = PlayerInstance.GetComponent<UnitReference>().UnitClassCreableListRedPlayer;
        PlayerInstance.GetComponent<UnitReference>().UnitClassCreableListRedPlayer = UnitListForOrgone;
        RenfortPhase.Instance.CreateRenfort(true);
        mouseCommand.GetComponent<MouseCommand>().MenuRenfortUI(true);

        while (GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1)
        {
            if (!DoingOrgoneCharge)
            {
                PlayerInstance.GetComponent<UnitReference>().UnitClassCreableListRedPlayer = TempSelectablelist;
                break;
            }
        }
        PlayerScript.Instance.RedPlayerInfos.OrgoneValue = 0;
        PlayerScript.Instance.RedPlayerInfos.UpdateOrgoneUI(1);
        GameManager.Instance._eventCall -= UseCharge5RedPlayer;
    }
    #endregion
}
