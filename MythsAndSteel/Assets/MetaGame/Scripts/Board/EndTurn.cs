using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    [Header("Nombre d'objectif à capturer pour la victoire par équipe.")]
    [SerializeField] private int RedObjCount;
    [SerializeField] private int BlueObjCount;

    [Header("Nombre d'objectif actuellement capturé par équipe.")]
    [SerializeField] private int ObjOwnedByRed;
    [SerializeField] private int ObjOwnedByBlue;

    List<GameObject> goalTileList = new List<GameObject>();

    private void Start()
    {
        foreach(GameObject Tile in TilesManager.Instance.TileList)
        {
            if(Tile.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif))
            {
                goalTileList.Add(Tile);
            }
        }

        GameManager.Instance.ManagerSO.GoToStrategyPhase += CheckResources;
        GameManager.Instance.ManagerSO.GoToStrategyPhase += CheckOwner;
    }

    /// <summary>
    /// Check si des ressources doivent être distribuées.
    /// </summary>
    public void CheckResources(){
        foreach(GameObject Tile in TilesManager.Instance.ResourcesList)
        {
            TileScript S = Tile.GetComponent<TileScript>();

            if(S.Unit != null){
                UnitScript US = S.Unit.GetComponent<UnitScript>();

                S.RemoveRessources(1, PlayerStatic.CheckIsUnitArmy(US.GetComponent<UnitScript>(), true) == true ? 1 : 2);
            }
            if(S.ResourcesCounter == 0){
                S.TerrainEffectList.Remove(MYthsAndSteel_Enum.TerrainType.Point_de_ressource);
            }
        }
    }

    /// <summary>
    /// Prend les nouveaux propriétaires des objectifs et check ensuite les conditions de victoire.
    /// </summary>
    public void CheckOwner()
    {
        foreach(GameObject Tile in TilesManager.Instance.TileList)
        {
            TileScript S = Tile.GetComponent<TileScript>();
            if(S.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif))
            {
                if(S.Unit != null)
                {
                    UnitScript US = S.Unit.GetComponent<UnitScript>();
                    if(US.UnitSO.IsInRedArmy && !US.UnitStatus.Contains(MYthsAndSteel_Enum.UnitStatut.PeutPasPrendreDesObjectifs))
                    {
                        Debug.Log("Objectif dans le camp rouge.");
                        ChangeOwner(S, true);
                    }
                    else if(!US.UnitSO.IsInRedArmy && !US.UnitStatus.Contains(MYthsAndSteel_Enum.UnitStatut.PeutPasPrendreDesObjectifs))
                    {
                        Debug.Log("Objectif dans le camp bleu.");
                        ChangeOwner(S, false);
                    }
                }
                else
                {
                    UnitScript US = S.Unit.GetComponent<UnitScript>();
                    if(!US.UnitStatus.Contains(MYthsAndSteel_Enum.UnitStatut.PeutPasPrendreDesObjectifs))
                    {
                        Debug.Log("Objectif neutre");
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
    void ChangeOwner(TileScript TileSc, bool RedArmy){
        if(TileSc.OwnerObjectiv == MYthsAndSteel_Enum.Owner.blue && RedArmy)
        {
            ObjOwnedByBlue--;
            TileSc.ChangePlayerObj(MYthsAndSteel_Enum.Owner.red);
            ObjOwnedByRed++;
        }
        if(TileSc.OwnerObjectiv == MYthsAndSteel_Enum.Owner.red && !RedArmy)
        {
            ObjOwnedByRed--;
            TileSc.ChangePlayerObj(MYthsAndSteel_Enum.Owner.blue);
            ObjOwnedByBlue++;
        }
        if(TileSc.OwnerObjectiv == MYthsAndSteel_Enum.Owner.neutral)
        {
            if(RedArmy)
            {
                TileSc.ChangePlayerObj(MYthsAndSteel_Enum.Owner.red);
                ObjOwnedByRed++;
            }
            else
            {
                TileSc.ChangePlayerObj(MYthsAndSteel_Enum.Owner.blue);
                ObjOwnedByBlue++;
            }
        }
    }

    /// <summary>
    /// Called by CheckOwner();
    /// </summary>
    protected void CheckVictory()
    {
        if(ObjOwnedByBlue == BlueObjCount)
        {
            // Blue win. End game.
            Debug.Log("Blue win.");
        }
        if(ObjOwnedByRed == RedObjCount)
        {
            // Red win. End game.
            Debug.Log("Red win.");
        }
    }
}
