using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    VictoryScreen victoryScreen;
    [Header("Nombre d'objectif à capturer pour la victoire par équipe.")]
    [SerializeField] private int RedObjCount;
    [SerializeField] private int BlueObjCount;
   
    [SerializeField] List<GameObject> goalTileList = new List<GameObject>();

    private void Start()
    {
        foreach (GameObject Tile in TilesManager.Instance.TileList)
        {
            if (Tile.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif))
            {
                goalTileList.Add(Tile);
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
                    S.RemoveRessources(1, PlayerStatic.CheckIsUnitArmy(US.GetComponent<UnitScript>(), true) == true ? 1 : 2);
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
    }

    /// <summary>
    /// Prend les nouveaux propriétaires des objectifs et check ensuite les conditions de victoire.
    /// </summary>
    public void CheckOwner()
    {
        Debug.Log("Ressources");
        foreach (GameObject Tile in goalTileList)
        {
            TileScript S = Tile.GetComponent<TileScript>();
            if (S.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif))
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

        if(PlayerScript.Instance.BluePlayerInfos.GoalCapturePointsNumber == BlueObjCount && PlayerScript.Instance.RedPlayerInfos.GoalCapturePointsNumber != RedObjCount)


        {
            GameManager.Instance.VictoryForArmy(2);
        }

        if(PlayerScript.Instance.RedPlayerInfos.GoalCapturePointsNumber == RedObjCount && PlayerScript.Instance.BluePlayerInfos.GoalCapturePointsNumber != BlueObjCount)

        {
            GameManager.Instance.VictoryForArmy(1);
        }
    }

}
