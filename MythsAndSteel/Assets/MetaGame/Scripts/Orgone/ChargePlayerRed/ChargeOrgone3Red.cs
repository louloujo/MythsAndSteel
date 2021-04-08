using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeOrgone3Red : ChargeOrgoneClass
{
   
    
    public void SetupVariable()
    {
        IsPowerForRedPlayer = true;

        cout = -3;
}
    public override void UtilisationChargeOrgone()
    {
        SetupVariable();
        base.UtilisationChargeOrgone();
   
    }
    public override void RaiseEvent()
    {
       
        base.RaiseEvent();
        Debug.Log("Je suis la charge3 du joueur rouge");
    }
}
