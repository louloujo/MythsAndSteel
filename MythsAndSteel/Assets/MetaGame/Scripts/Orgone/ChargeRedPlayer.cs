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

    #region Charge 3 d'Orgone
    public override void ChargeOrgone3(int cost)
    {
        if(MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(3, 1))
        {
            Debug.Log("R3Laucnhed");
            GameManager.Instance._eventCall += UseCharge3RedPlayer;
            UIInstance.Instance.ShowValidationPanel("Charge 3 D'orgone", "Êtes vous sûr de vouloir d'utiliser la charge 3 d'orgone ?");
        }

    void UseCharge3RedPlayer()
        {
            Debug.Log("R3 USE");
            PlayerScript.Instance.RedPlayerInfos.EventUseLeft += 1;
            PlayerScript.Instance.RedPlayerInfos.OrgoneValue -= 3;
            Debug.Log("R3 finish");
            PlayerScript.Instance.RedPlayerInfos.UpdateOrgoneUI(1);
        }

    }
    #endregion

    #region Charge 5 d'Orgone
    public override void ChargeOrgone5(int cost)
    {
        Debug.Log("R5");
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(4, 1))
        {
            Debug.Log("R5Laucnhed");
            GameManager.Instance._eventCall += UseCharge5RedPlayer;
            GameManager.Instance._eventCall();
        }
        
    }
    
    void UseCharge5RedPlayer()
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
        PlayerScript.Instance.RedPlayerInfos.OrgoneValue = 0;
        PlayerScript.Instance.RedPlayerInfos.UpdateOrgoneUI(1);
        GameManager.Instance._eventCall -= UseCharge5RedPlayer;
    }
    #endregion
}
