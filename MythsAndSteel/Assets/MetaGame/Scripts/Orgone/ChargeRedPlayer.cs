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
        Debug.Log("R1");
    }
    public override void ChargeOrgone3(int cost)
    {
        Debug.Log("R3");

    }
    public override void ChargeOrgone5(int cost)
    {
        Debug.Log("R5");
        if (GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1 && PlayerScript.Instance.RedPlayerInfos.OrgoneValue == 5)
        {
            OrgoneManager.Instance.DoingOrgoneCharge = true;
            List<GameObject> TempSelectablelist = PlayerInstance.GetComponent<UnitReference>().UnitClassCreableListRedPlayer;
            PlayerInstance.GetComponent<UnitReference>().UnitClassCreableListRedPlayer = UnitListForOrgone;
            RenfortPhase.Instance.CreateRenfort(true);
            mouseCommand.GetComponent<MouseCommand>().MenuRenfortUI(true);

            while (GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1)
            {
                if (!DoingOrgoneCharge)
                {
                    PlayerInstance.GetComponent<UnitReference>().UnitClassCreableListRedPlayer = TempSelectablelist;
                    break;
                }
            }
        }
        
    }
}
