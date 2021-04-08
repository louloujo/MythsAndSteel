using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeOrgone5Red : ChargeOrgoneClass
{
   
    
    public void SetupVariable()
    {
        IsPowerForRedPlayer = true;

        cout = -5;
}
    public override void UtilisationChargeOrgone()
    {
        SetupVariable();
        base.UtilisationChargeOrgone();
   
    }
    public override void RaiseEvent()
    {
       
        base.RaiseEvent();
        Debug.Log("Je suis la charge5 du joueur rouge");
    }
}
