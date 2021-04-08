using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeOrgone4Red : ChargeOrgoneClass
{
   
    
    public void SetupVariable()
    {
        IsPowerForRedPlayer = true;

        cout = -4;
}
    public override void UtilisationChargeOrgone()
    {
        SetupVariable();
        base.UtilisationChargeOrgone();
   
    }
    public override void RaiseEvent()
    {
       
        base.RaiseEvent();
        Debug.Log("Je suis la charge4 du joueur rouge");
    }
}
