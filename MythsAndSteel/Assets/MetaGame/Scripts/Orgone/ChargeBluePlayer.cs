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
    public override void ChargeOrgone5(int cost)
    {
        Debug.Log("B5");
        DoingOrgoneCharge = true;
    }
}
