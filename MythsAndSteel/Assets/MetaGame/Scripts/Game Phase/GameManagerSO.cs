using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "META/Game Manager")]
public class GameManagerSO : ScriptableObject
{
    public bool azer = false;
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
                foreach (GameObject unit in GameManager.Instance.IsPlayerRedTurn ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer)
                {
                    unit.GetComponent<UnitScript>().ResetTurn();
                }
                UIInstance.Instance.ActiveOrgoneChargeButton();
                if(GameManager.Instance.IsPlayerRedStarting)
                {
                    PlayerScript.Instance.RedPlayerInfos.OrgonePowerLeft = 1;
                }
                else
                {
                    PlayerScript.Instance.BluePlayerInfos.OrgonePowerLeft = 1;
                }
                GoToOrgoneJ1Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1:
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1);
                UIInstance.Instance.DesactiveOrgoneChargeButton();
                UIInstance.Instance.ButtonRenfortJ1.GetComponent<Button>().interactable = true;
                if (GameManager.Instance.SabotageStat == 1 && !GameManager.Instance.IsPlayerRedTurn)
                {
                    PlayerScript.Instance.BluePlayerInfos.ActivationLeft--;
                    GameManager.Instance.SabotageStat = 3;

                }
                else if (GameManager.Instance.SabotageStat == 2 && GameManager.Instance.IsPlayerRedTurn)
                {
                    PlayerScript.Instance.RedPlayerInfos.ActivationLeft--;
                    GameManager.Instance.SabotageStat = 3;
                }
                UIInstance.Instance.UpdateActivationLeft();

                GoToActionJ1Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2:

                UIInstance.Instance.ButtonRenfortJ1.GetComponent<Button>().interactable = false;
                foreach (GameObject unit in GameManager.Instance.IsPlayerRedTurn ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer)
                {
                    unit.GetComponent<UnitScript>().ResetTurn();
                }
                if (GameManager.Instance.possesion)
                {

                    foreach (GameObject unit in GameManager.Instance.IsPlayerRedTurn ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer)
                    {
                        unit.GetComponent<UnitScript>().ResetStatutPossesion();
                        GameManager.Instance.possesion = false;



                    }
                }
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2);
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

                                                Try.EndPlayerTurnEffect(GameManager.Instance.IsPlayerRedTurn);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }



                if (GameManager.Instance.armeEpidemelogiqueStat != 0)
                {
                    List<GameObject> refunit = new List<GameObject>();
                    if (GameManager.Instance.IsPlayerRedTurn && GameManager.Instance.armeEpidemelogiqueStat == 2)
                    {
                        refunit = PlayerScript.Instance.UnitRef.UnitListRedPlayer;
                    }
                    else if (!GameManager.Instance.IsPlayerRedTurn && GameManager.Instance.armeEpidemelogiqueStat == 1)
                    {
                        refunit = PlayerScript.Instance.UnitRef.UnitListBluePlayer;
                    }
                    foreach (GameObject unit in refunit)
                    {

                        if (unit.GetComponent<UnitScript>().UnitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.ArmeEpidemiologique))
                        {

                            unit.GetComponent<UnitScript>().TakeDamage(1);
                            unit.GetComponent<UnitScript>().RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut.ArmeEpidemiologique);
                            foreach (int i in PlayerStatic.GetNeighbourDiag(unit.GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[unit.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false))
                            {

                                if (TilesManager.Instance.TileList[i].GetComponent<TileScript>().Unit != null)
                                {
                                    TilesManager.Instance.TileList[i].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().TakeDamage(1);
                                }
                            }
                            
                            GameManager.Instance.armeEpidemelogiqueStat = 0;
                            refunit = null;

                        }
                    }

                }
                if (GameManager.Instance.ParalysieStat != 3)
                {
                    List<GameObject> refunit = new List<GameObject>();
                    refunit.AddRange(PlayerScript.Instance.UnitRef.UnitListRedPlayer);
                    refunit.AddRange(PlayerScript.Instance.UnitRef.UnitListBluePlayer);
                    if (GameManager.Instance.IsPlayerRedTurn && GameManager.Instance.ParalysieStat == 2 || !GameManager.Instance.IsPlayerRedTurn && GameManager.Instance.ParalysieStat == 1)
                    {

                        foreach (GameObject unit in refunit)
                        {

                            if (unit.GetComponent<UnitScript>().UnitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Paralysie))
                            {
                                unit.GetComponent<UnitScript>().RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut.Paralysie);
                            }

                        }
                     
                    }
                    GameManager.Instance.ParalysieStat = 3;
                    refunit.Clear();
                }
                if (!GameManager.Instance.IsPlayerRedStarting)
                {
                    PlayerScript.Instance.RedPlayerInfos.OrgonePowerLeft = 1;
                }
                else
                {
                    PlayerScript.Instance.BluePlayerInfos.OrgonePowerLeft = 1;
                }


                GoToOrgoneJ2Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2:
                UIInstance.Instance.ButtonRenfortJ2.GetComponent<Button>().interactable = true;
                if (GameManager.Instance.SabotageStat == 1 && !GameManager.Instance.IsPlayerRedTurn)
                {
                    PlayerScript.Instance.BluePlayerInfos.ActivationLeft--;
                    GameManager.Instance.SabotageStat = 3;
                }
                else if (GameManager.Instance.SabotageStat == 2 && GameManager.Instance.IsPlayerRedTurn)
                {
                    PlayerScript.Instance.RedPlayerInfos.ActivationLeft--;
                    GameManager.Instance.SabotageStat = 3;
                }
                UIInstance.Instance.UpdateActivationLeft();
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2);
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

                                                Try.EndPlayerTurnEffect(GameManager.Instance.IsPlayerRedTurn);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (GameObject unit in PlayerScript.Instance.UnitRef.UnitListRedPlayer)
                {

                    unit.GetComponent<UnitScript>().RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut.PeutPasCombattre);
                    unit.GetComponent<UnitScript>().RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut.Invincible);
                    unit.GetComponent<UnitScript>().RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut.PeutPasCombattre);

                }

                foreach (GameObject unit in PlayerScript.Instance.UnitRef.UnitListBluePlayer)
                {

                    unit.GetComponent<UnitScript>().RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut.PeutPasCombattre);
                    unit.GetComponent<UnitScript>().RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut.Invincible);
                    unit.GetComponent<UnitScript>().RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut.PeutPasCombattre);


                }
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.Strategie);
                if (GameManager.Instance.armeEpidemelogiqueStat != 0)
                {
                    List<GameObject> refunit = new List<GameObject>();
                    if (GameManager.Instance.IsPlayerRedTurn && GameManager.Instance.armeEpidemelogiqueStat == 2)
                    {
                        refunit = PlayerScript.Instance.UnitRef.UnitListRedPlayer;
                    }
                    else if (!GameManager.Instance.IsPlayerRedTurn && GameManager.Instance.armeEpidemelogiqueStat == 1)
                    {
                        refunit = PlayerScript.Instance.UnitRef.UnitListBluePlayer;
                    }
                    foreach (GameObject unit in refunit)
                    {

                        if (unit.GetComponent<UnitScript>().UnitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.ArmeEpidemiologique))
                        {

                            unit.GetComponent<UnitScript>().TakeDamage(1);
                            unit.GetComponent<UnitScript>().RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut.ArmeEpidemiologique);
                            foreach (int i in PlayerStatic.GetNeighbourDiag(unit.GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[unit.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false))
                            {

                                if (TilesManager.Instance.TileList[i].GetComponent<TileScript>().Unit != null)
                                {
                                    TilesManager.Instance.TileList[i].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().TakeDamage(1);
                                }
                            }
                            GameManager.Instance.armeEpidemelogiqueStat = 0;
                            refunit = null;
                        }
                    }
                    if (GameManager.Instance.possesion)
                    {
                        foreach (GameObject unit in GameManager.Instance.IsPlayerRedTurn ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer)
                        {
                            unit.GetComponent<UnitScript>().ResetStatutPossesion();
                            GameManager.Instance.possesion = false;

                        }
                    }
                }
                if (GameManager.Instance.ParalysieStat != 3)
                {
                    List<GameObject> refunit = new List<GameObject>();
                    refunit.AddRange(PlayerScript.Instance.UnitRef.UnitListRedPlayer);
                    refunit.AddRange(PlayerScript.Instance.UnitRef.UnitListBluePlayer);
                    if (GameManager.Instance.IsPlayerRedTurn && GameManager.Instance.ParalysieStat == 2 || !GameManager.Instance.IsPlayerRedTurn && GameManager.Instance.ParalysieStat == 1)
                    {
                      
                    foreach (GameObject unit in refunit)
                    {

                        if (unit.GetComponent<UnitScript>().UnitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Paralysie))
                        {
                            unit.GetComponent<UnitScript>().RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut.Paralysie);
                        }

                    }
                    }
                    GameManager.Instance.ParalysieStat = 3;
                    refunit.Clear();
                }
                UIInstance.Instance.ButtonRenfortJ2.GetComponent<Button>().interactable = false;
                PlayerScript.Instance.RedPlayerInfos.HasCreateUnit = false;
                PlayerScript.Instance.BluePlayerInfos.HasCreateUnit = false;
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
        azer = false;
        GoToActivationPhase = null;
        GoToOrgoneJ1Phase = null;
        GoToActionJ1Phase = null;
        GoToOrgoneJ2Phase = null;
        GoToActionJ2Phase = null; 
        GoToStrategyPhase = null;
       GameManager.Instance.isGamePaused = false;
       SceneManager.LoadScene(1);
    }
    public void LoadScene(int sceneToLoad)
    {
        Time.timeScale = 1;
      
        GoToDebutPhase = null;
        azer = false;
        GoToActivationPhase = null;
        GoToOrgoneJ1Phase = null;
        GoToActionJ1Phase = null;
        GoToOrgoneJ2Phase = null;
        GoToActionJ2Phase = null;
        GoToStrategyPhase = null;
        GameManager.Instance.isGamePaused = false;
        SceneManager.LoadScene(sceneToLoad);
      
    }
}