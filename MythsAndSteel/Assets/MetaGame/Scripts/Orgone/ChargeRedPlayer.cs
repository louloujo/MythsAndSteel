using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeRedPlayer : ChargeOrgone
{
    MouseCommand mouseCommand;
    UnitReference unitReference;
    public List<GameObject> UnitListForOrgone = new List<GameObject>();
    public override void ChargeOrgone1(int cost)
    {

    }
    public override void ChargeOrgone3(int cost)
    {

    }
    public override void ChargeOrgone5(int cost)
    {
        if (GameManager.Instance.IsPlayerRedTurn)
        {
            DoingOrgoneCharge = true;
            List<GameObject> TempSelectablelist = unitReference.UnitClassCreableListRedPlayer;
            unitReference.UnitClassCreableListRedPlayer = UnitListForOrgone;
            mouseCommand.MenuRenfortUI(true);
        }
        
    }
}
