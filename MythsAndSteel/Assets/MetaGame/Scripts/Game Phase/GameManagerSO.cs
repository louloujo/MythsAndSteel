using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "META/Game Manager")]
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
    public void GoToPhase(MYthsAndSteel_Enum.PhaseDeJeu phase = MYthsAndSteel_Enum.PhaseDeJeu.Debut, bool randomPhase = false)
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        int phaseSuivante = ((int)GameManager.Instance.ActualTurnPhase) + 1;
        MYthsAndSteel_Enum.PhaseDeJeu nextPhase = MYthsAndSteel_Enum.PhaseDeJeu.Debut;

        if (randomPhase)
        {
            nextPhase = phase;
        }
        else
        {
            phaseSuivante = ((int)GameManager.Instance.ActualTurnPhase) + 1;

            if (phaseSuivante > 6)
            {
                if (GameManager.Instance.ActualTurnNumber > 11)
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

            nextPhase = (MYthsAndSteel_Enum.PhaseDeJeu)phaseSuivante;
        }

        gophase(nextPhase);
    }

    /// <summary>
    /// Aller à la phase de jeu renseigner en paramètre POUR LE BOUTON
    /// </summary>
    /// <param name="phaseToGoTo"></param>
    public void GoToPhase()
    {
       

        int phaseSuivante = ((int)GameManager.Instance.ActualTurnPhase) + 1;


        if (phaseSuivante > 6)
        {
            if (GameManager.Instance.ActualTurnNumber > 11)
            {
                GameManager.Instance.VictoryForArmy(1);
                return;
            }
            else
            {
                phaseSuivante = 0;
                GameManager.Instance.ActualTurnNumber++;
                GameManager.Instance.UpdateTurn();
            }
        }

        MYthsAndSteel_Enum.PhaseDeJeu nextPhase = (MYthsAndSteel_Enum.PhaseDeJeu)phaseSuivante;

        gophase(nextPhase);
    }

    /// <summary>
    /// Va a la phase
    /// </summary>
    /// <param name="nextPhase"></param>
    void gophase(MYthsAndSteel_Enum.PhaseDeJeu nextPhase)
    {
        //Selon la phase effectue certaines actions
        switch (nextPhase)
        {
            case MYthsAndSteel_Enum.PhaseDeJeu.Debut:
                if (GoToDebutPhase != null)
                {
                    GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.Debut);
                    GoToDebutPhase();
                }
                else
                {
                    GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.Activation);
                    GoToActivationPhase();
                }
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.Activation:
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.Activation);
                GoToActivationPhase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1:
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1);

                GoToOrgoneJ1Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1:
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1);

                foreach (GameObject unit in GameManager.Instance.IsPlayerRedTurn ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer)
                {
                    unit.GetComponent<UnitScript>().ResetTurn();
                }

                GoToActionJ1Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2:
                foreach (GameObject TS in TilesManager.Instance.TileList)
                {
                    foreach (MYthsAndSteel_Enum.TerrainType T1 in TS.GetComponent<TileScript>().TerrainEffectList)
                    {
                        foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                        {
                            foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                            {
                                if (T1 == T2)
                                {
                                    if (Type.Child != null)
                                    {
                                        if (Type.MustBeInstantiate)
                                        {
                                            foreach (GameObject G in TS.GetComponent<TileScript>()._Child)
                                            {
                                                if (G.TryGetComponent<ChildEffect>(out ChildEffect Try2))
                                                {
                                                    if (Try2.Type == T1)
                                                    {
                                                        if (Try2.TryGetComponent<TerrainParent>(out TerrainParent Try3))
                                                        {
                                                            Debug.Log("bon");
                                                            Try3.EndPlayerTurnEffect(GameManager.Instance.IsPlayerRedTurn);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                            {
                                                Debug.Log("bon1");
                                                Try.EndPlayerTurnEffect(GameManager.Instance.IsPlayerRedTurn);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2);

                GoToOrgoneJ2Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2:
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2);

                foreach (GameObject unit in GameManager.Instance.IsPlayerRedTurn ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer)
                {
                    unit.GetComponent<UnitScript>().ResetTurn();
                }

                GoToActionJ2Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.Strategie:
                Debug.Log("End");
                foreach (GameObject TS in TilesManager.Instance.TileList)
                {
                    foreach (MYthsAndSteel_Enum.TerrainType T1 in TS.GetComponent<TileScript>().TerrainEffectList)
                    {
                        foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                        {
                            foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                            {
                                if (T1 == T2)
                                {
                                    if (Type.Child != null)
                                    {
                                        if (Type.MustBeInstantiate)
                                        {
                                            foreach (GameObject G in TS.GetComponent<TileScript>()._Child)
                                            {
                                                if (G.TryGetComponent<ChildEffect>(out ChildEffect Try2))
                                                {
                                                    if (Try2.Type == T1)
                                                    {
                                                        if (Try2.TryGetComponent<TerrainParent>(out TerrainParent Try3))
                                                        {
                                                            Debug.Log("bon");
                                                            Try3.EndPlayerTurnEffect(GameManager.Instance.IsPlayerRedTurn);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                            {
                                                Debug.Log("bon1");
                                                Try.EndPlayerTurnEffect(GameManager.Instance.IsPlayerRedTurn);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.Strategie);

                if (GoToStrategyPhase != null) GoToStrategyPhase();
                break;
        }
    }

    /// <summary>
    /// Est ce que l'event de début possède des fonctions à appeler
    /// </summary>
    /// <returns></returns>
    public bool GetDebutFunction()
    {
        if (GoToDebutPhase != null) return true;
        else return false;
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        GoToDebutPhase = null;
        GoToActivationPhase = null;
        GoToOrgoneJ1Phase = null;
        GoToActionJ1Phase = null;
        GoToOrgoneJ2Phase = null;
        GoToActionJ2Phase = null;
        GoToStrategyPhase = null;
        GameManager.Instance.isGamePaused = false;
        SceneManager.LoadScene(1);
    }
}