using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Manager")]
public class GameManagerSO : ScriptableObject
{
    //Event pour quand le joueur clique sur un bouton pour passer à la phase en question
    public delegate void ClickButtonSwitchPhase();
    public event ClickButtonSwitchPhase GoToDebutPhase;
    public event ClickButtonSwitchPhase GoToActivationPhase;
    public event ClickButtonSwitchPhase GoToOrgoneJ1Phase;
    public event ClickButtonSwitchPhase GoToActionJ1Phase;
    public event ClickButtonSwitchPhase GoToOrgoneJ2Phase;
    public event ClickButtonSwitchPhase GoToActionJ2Phase;
    public event ClickButtonSwitchPhase GoToStrategyPhase;

    /// <summary>
    /// Aller à la phase de jeu renseigner en paramètre
    /// </summary>
    /// <param name="phaseToGoTo"></param>
    public void GoToPhase()
    {
        int phaseSuivante = ((int)GameManager.Instance.ActualTurnPhase) + 1;

        if(phaseSuivante > 6)
        {
            if(GameManager.Instance.ActualTurnNumber > 11)
            {
                GameManager.Instance.VictoryForArmy(1);
                return;
            }
            else
            {
                phaseSuivante = 0;
                GameManager.Instance.ActualTurnNumber++;
            }
        }

        MYthsAndSteel_Enum.PhaseDeJeu nextPhase = (MYthsAndSteel_Enum.PhaseDeJeu) phaseSuivante;

        //Selon la phase effectue certaines actions
        switch(nextPhase)
        {
            case MYthsAndSteel_Enum.PhaseDeJeu.Debut:
                GoToDebutPhase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.Activation:
                GoToActivationPhase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1:
                GoToOrgoneJ1Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1:
                foreach(GameObject unit in GameManager.Instance.IsPlayerRedTurn ? PlayerScript.Instance._unitListRedPlayer : PlayerScript.Instance._unitListBluePlayer){
                    unit.GetComponent<UnitScript>().ResetTurn();
                }

                GoToActionJ1Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2:
                GoToOrgoneJ2Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2:
                foreach(GameObject unit in GameManager.Instance.IsPlayerRedTurn ? PlayerScript.Instance._unitListRedPlayer : PlayerScript.Instance._unitListBluePlayer){
                    unit.GetComponent<UnitScript>().ResetTurn();
                }

                GoToActionJ2Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.Strategie:
                GoToStrategyPhase();
                break;
        }
    }
}
