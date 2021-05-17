using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBluePlayer : ChargeOrgone
{
    
    
    public override void ChargeOrgone1(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, 2))
        {
            Debug.Log("B1");

        }
    }
    public override void ChargeOrgone3(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, 2))
        {

            Debug.Log("B3");
        }
    }
    public override void ChargeOrgone5(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, 2))
        {
            Debug.Log("B5");
      

        }
    }
}
