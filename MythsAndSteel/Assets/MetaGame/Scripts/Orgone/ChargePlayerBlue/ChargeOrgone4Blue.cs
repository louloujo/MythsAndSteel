using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeOrgone4Blue : ChargeOrgoneClass
{


    public void SetupVariable()
    {
        IsPowerForRedPlayer = false;


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
        Debug.Log("Je suis la charge4 du joueur bleu");
    }
}
