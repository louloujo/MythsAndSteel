using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    VictoryScreen victoryScreen;
    [Header("Nombre d'objectif à capturer pour la victoire par équipe.")]
    [SerializeField] private int RedObjCount;
    [SerializeField] private int BlueObjCount;

    [Header("Nombre d'objectif à capturer pour la victoire par équipe Rouge.")]
    [SerializeField] List<GameObject> RedgoalTileList = new List<GameObject>();

    [Header("Nombre d'objectif à capturer pour la victoire par l'équipe Bleu.")]
    [SerializeField] List<GameObject> BluegoalTileList = new List<GameObject>();

    private void Start()
    {
        foreach (GameObject Tile in TilesManager.Instance.TileList)
        {
            if (Tile.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif_Rouge))
            {
                RedgoalTileList.Add(Tile);
            }
        }
        foreach (GameObject Tile in TilesManager.Instance.TileList)
        {
            if (Tile.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif_Bleu))
            {
                BluegoalTileList.Add(Tile);
            }
        }
        GameManager.Instance.ManagerSO.GoToStrategyPhase += EndTerrainEffect;
        GameManager.Instance.ManagerSO.GoToStrategyPhase += CheckResources;
        GameManager.Instance.ManagerSO.GoToStrategyPhase += CheckOwner;
      
    }

    public void EndTerrainEffect()
    {
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
                                                    TerrainGestion.Instance.EndTurn(Try3, TS.GetComponent<TileScript>());
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                    {
                                        TerrainGestion.Instance.EndTurn(Try, TS.GetComponent<TileScript>());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// Check si des ressources doivent être distribuées.
    /// </summary>
    public void CheckResources()
    {
        Debug.Log("Ressources");
        foreach (GameObject Tile in TilesManager.Instance.ResourcesList)
        {
            TileScript S = Tile.GetComponent<TileScript>();
            if (S.ResourcesCounter != 0)
            {
                if (S.Unit != null)
                {
                    UnitScript US = S.Unit.GetComponent<UnitScript>();
                    if(GameManager.Instance.VolDeRavitaillementStat != 3)
                    {
                       if(S.Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy && GameManager.Instance.VolDeRavitaillementStat == 2)
                        {
                       
                            S.RemoveRessources(1, 2);
                      
                        }
                       else if (!S.Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy && GameManager.Instance.VolDeRavitaillementStat == 1)
                        {
                       
                            S.RemoveRessources(1, 1);
                     
                        }
                        else
                        {

                            S.RemoveRessources(1, PlayerStatic.CheckIsUnitArmy(US.GetComponent<UnitScript>(), true) == true ? 1 : 2);
                        }
                    }
                    else
                    {

                    S.RemoveRessources(1, PlayerStatic.CheckIsUnitArmy(US.GetComponent<UnitScript>(), true) == true ? 1 : 2);
                    }
                }
            }

            if (S.ResourcesCounter == 0)
            {
                S.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Point_de_ressource);
                S.TerrainEffectList.Remove(MYthsAndSteel_Enum.TerrainType.Point_de_ressource);
                S.CreateEffect(MYthsAndSteel_Enum.TerrainType.Point_de_ressources_vide);
                S.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Point_de_ressources_vide);
            }
        }
        GameManager.Instance.VolDeRavitaillementStat = 3;
    }

    /// <summary>
    /// Prend les nouveaux propriétaires des objectifs et check ensuite les conditions de victoire.
    /// </summary>
    public void CheckOwner()
    {
        Debug.Log("Ressources");
        foreach (GameObject Tile in RedgoalTileList)
        {
            TileScript S = Tile.GetComponent<TileScript>();
            if (S.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif_Rouge))
            {
                if (S.Unit != null)
                {
                    UnitScript US = S.Unit.GetComponent<UnitScript>();
                    if (US.UnitSO.IsInRedArmy && !US.UnitStatus.Contains(MYthsAndSteel_Enum.UnitStatut.PeutPasPrendreDesObjectifs))
                    {
                        Debug.Log("Objectif dans le camp rouge.");
                        ChangeOwner(S, true);
                    }
                    else if (!US.UnitSO.IsInRedArmy && !US.UnitStatus.Contains(MYthsAndSteel_Enum.UnitStatut.PeutPasPrendreDesObjectifs))
                    {
                        Debug.Log("Objectif dans le camp bleu.");
                        ChangeOwner(S, false);
                    }
                }
            }
        }
        foreach (GameObject Tile in BluegoalTileList)
        {
            TileScript S = Tile.GetComponent<TileScript>();
            if (S.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif_Bleu))
            {
                if (S.Unit != null)
                {
                    UnitScript US = S.Unit.GetComponent<UnitScript>();
                    if (US.UnitSO.IsInRedArmy && !US.UnitStatus.Contains(MYthsAndSteel_Enum.UnitStatut.PeutPasPrendreDesObjectifs))
                    {
                        Debug.Log("Objectif dans le camp rouge.");
                        ChangeOwner(S, true);
                    }
                    else if (!US.UnitSO.IsInRedArmy && !US.UnitStatus.Contains(MYthsAndSteel_Enum.UnitStatut.PeutPasPrendreDesObjectifs))
                    {
                        Debug.Log("Objectif dans le camp bleu.");
                        ChangeOwner(S, false);
                    }
                }
            }
        }
        CheckVictory();
    }

    /// <summary>
    /// Change le propriétaire d'un objectif.
    /// </summary>
    /// <param name="TileSc"></param>
    /// <param name="RedArmy"></param>
    void ChangeOwner(TileScript TileSc, bool RedArmy)
    {
        if (TileSc.OwnerObjectiv == MYthsAndSteel_Enum.Owner.blue && RedArmy)
        {
            PlayerScript.Instance.BluePlayerInfos.GoalCapturePointsNumber--;
            TileSc.ChangePlayerObj(MYthsAndSteel_Enum.Owner.red);
            PlayerScript.Instance.RedPlayerInfos.GoalCapturePointsNumber++;
        }
        if (TileSc.OwnerObjectiv == MYthsAndSteel_Enum.Owner.red && !RedArmy)
        {
            PlayerScript.Instance.RedPlayerInfos.GoalCapturePointsNumber--;
            TileSc.ChangePlayerObj(MYthsAndSteel_Enum.Owner.blue);
            PlayerScript.Instance.BluePlayerInfos.GoalCapturePointsNumber++;
        }
        if (TileSc.OwnerObjectiv == MYthsAndSteel_Enum.Owner.neutral)
        {
            if (RedArmy)
            {
                TileSc.ChangePlayerObj(MYthsAndSteel_Enum.Owner.red);
                PlayerScript.Instance.RedPlayerInfos.GoalCapturePointsNumber++;
            }
            else
            {
                TileSc.ChangePlayerObj(MYthsAndSteel_Enum.Owner.blue);
                PlayerScript.Instance.BluePlayerInfos.GoalCapturePointsNumber++;
            }
        }
    }

    /// <summary>
    /// Called by CheckOwner();
    /// </summary>
    protected void CheckVictory()
    {
        switch (PlayerPrefs.GetInt("Bataille"))
        {
            case 1: // RETHEL
                if (true /*Prendre 2 objectifs simultanément*/)
                {
                    GameManager.Instance.VictoryForArmy(1); // Allemand
                }

                if (GameManager.Instance.ActualTurnNumber == 12)
                {
                    GameManager.Instance.VictoryForArmy(2); // OUI
                }
                break;


            case 2: // SHANGHAI
                if (PlayerScript.Instance.RedPlayerInfos.GoalCapturePointsNumber == RedObjCount) /* RedObjCount = 2 */
                {
                    GameManager.Instance.VictoryForArmy(1); // OUI
                }

                if (GameManager.Instance.ActualTurnNumber == 12)
                {
                    GameManager.Instance.VictoryForArmy(2); // OUI
                }
                break;


            case 3: // STALINGRAD
                if (true /*Avoir le plus d'objectifs à la fin du tour 12 (un check au tour 6, puis un check au tour 12)  SI EGALITE T12 ALL WIN */) 
                {
                    GameManager.Instance.VictoryForArmy(1); // Allemand
                }

                if (true /*Avoir le plus d'objectifs à la fin du tour 12 (un check au tour 6, puis un check au tour 12)*/)
                {
                    GameManager.Instance.VictoryForArmy(2); // Russe
                }
                break;


            case 4: // HUSKY
                if (GameManager.Instance.ActualTurnNumber == 10)
                {
                    GameManager.Instance.VictoryForArmy(1); // OUI
                }

                if (true /* Prendre 2 objectifs simultanément a partir du T5*/)
                {
                    GameManager.Instance.VictoryForArmy(2); // UK
                }
                break;


            case 5: // GUADALCANAL
                if (GameManager.Instance.ActualTurnNumber == 12)
                {
                    GameManager.Instance.VictoryForArmy(1); // OUI
                }

                if (PlayerScript.Instance.BluePlayerInfos.GoalCapturePointsNumber == BlueObjCount) /* BlueObjCount = 3 */
                {
                    GameManager.Instance.VictoryForArmy(2); // OUI
                }
                break;


            case 6: // EL ALAMEIN
                if (PlayerScript.Instance.RedPlayerInfos.GoalCapturePointsNumber == RedObjCount /* ou tuer la chaine de commandement */) /* RedObjCount = 1 */
                {
                    GameManager.Instance.VictoryForArmy(1); // Allemand
                }

                if (GameManager.Instance.ActualTurnNumber == 10)
                {
                    GameManager.Instance.VictoryForArmy(2); // OUI
                }
                break;


            case 7:
                if (PlayerScript.Instance.RedPlayerInfos.GoalCapturePointsNumber == RedObjCount) /* RedObjCount = 1 */
                {
                    GameManager.Instance.VictoryForArmy(1); // OUI
                }

                if (GameManager.Instance.ActualTurnNumber == 12)
                {
                    GameManager.Instance.VictoryForArmy(2); // OUI
                }
                break;
        }
    }
    // PlayerScript.Instance.RedPlayerInfos.GoalCapturePointsNumber == RedObjCount && PlayerScript.Instance.BluePlayerInfos.GoalCapturePointsNumber != BlueObjCount
}
