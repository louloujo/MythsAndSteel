using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeOrgone2Red : ChargeOrgoneClass
{
   
    
    public void SetupVariable()
    {
        IsPowerForRedPlayer = true;

        cout = -2;
}
    public override void UtilisationChargeOrgone()
    {
        SetupVariable();
        base.UtilisationChargeOrgone();
   
    }
    public override void RaiseEvent()
    {
       
        base.RaiseEvent();
        Debug.Log("Je suis la charge2 du joueur rouge");
    }
}
