using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeOrgone5Blue : ChargeOrgoneClass
{


    public void SetupVariable()
    {
        IsPowerForRedPlayer = false;

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
        Debug.Log("Je suis la charge5 du joueur bleu");
    }
}
