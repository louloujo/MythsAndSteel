using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeOrgone3Blue : ChargeOrgoneClass
{


    public void SetupVariable()
    {
        IsPowerForRedPlayer = false;



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
        Debug.Log("Je suis la charge3 du joueur bleu");
    }
}
