using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeRedPlayer : ChargeOrgone
{
    [SerializeField] GameObject mouseCommand;
    [SerializeField] GameObject PlayerInstance;
    public List<GameObject> UnitListForOrgone = new List<GameObject>();
    public override void ChargeOrgone1(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, 1))
        {

            Debug.Log("R1");
        }
    }
    public override void ChargeOrgone3(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, 1))
        {

            Debug.Log("R3");
        }

    }
    public override void ChargeOrgone5(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, 1))
        {
        Debug.Log("R5");
            OrgoneManager.Instance.DoingOrgoneCharge = true;
            List<GameObject> TempSelectablelist = PlayerInstance.GetComponent<UnitReference>().UnitClassCreableListRedPlayer;
            PlayerInstance.GetComponent<UnitReference>().UnitClassCreableListRedPlayer = UnitListForOrgone;
            RenfortPhase.Instance.CreateRenfort(true);
            mouseCommand.GetComponent<MouseCommand>().MenuRenfortUI(true);

            while (GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1)
            {
                if (OrgoneManager.Instance.DoingOrgoneCharge)
                {
                    PlayerInstance.GetComponent<UnitReference>().UnitClassCreableListRedPlayer = TempSelectablelist;
                    break;
                }
            }
           
        }
        
    }
}
