using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Ce Script fait partie de la liste de script qui détermine les caractéristiques et le pouvoir d'une charge de la jauge d'orgone. 
 */
public class ChargeOrgone1Blue : ChargeOrgoneClass
{

    /// <summary>
    /// Permet de spécifier pour cette charge les variables décrites dans le script ChargeOrgoneClass
    /// </summary>
    public void SetupVariable()
    {
        IsPowerForRedPlayer = false;



        cout = -1;
    }
    public override void UtilisationChargeOrgone()
    {
        SetupVariable();
        base.UtilisationChargeOrgone();

    }
    public override void RaiseEvent()
    {

        base.RaiseEvent();
        Debug.Log("Je suis la charge1 du joueur bleu");
    }
}
