using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeRedPlayer : ChargeOrgone
{
    [SerializeField] GameObject mouseCommand;
    [SerializeField] GameObject PlayerInstance;
    public List<GameObject> UnitListForOrgone = new List<GameObject>();
    public override void ChargeOrgone1(int cost)
    {
        Debug.Log("R1");
    }
    public override void ChargeOrgone3(int cost)
    {
        Debug.Log("R3");

    }
    public override void ChargeOrgone5(int cost)
    {
        Debug.Log("R5");
        if (GameManager.Instance.IsPlayerRedTurn)
        {
            OrgoneManager.Instance.DoingOrgoneCharge = true;
            List<GameObject> TempSelectablelist = PlayerInstance.GetComponent<UnitReference>().UnitClassCreableListRedPlayer;
            PlayerInstance.GetComponent<UnitReference>().UnitClassCreableListRedPlayer = UnitListForOrgone;
            mouseCommand.GetComponent<MouseCommand>().MenuRenfortUI(true);
        }
        
    }
}
