using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenfortPhase : MonoBehaviour
{
    #region AppelDeScript
    Player player;
    UnitReference unitReference;
    #endregion

    [SerializeField] private List<GameObject> _createTileJ1;
    public List<GameObject> CreateTileJ1 => _createTileJ1;

    [SerializeField] private List<GameObject> _createTileJ2;
    public List<GameObject> CreateTileJ2 => _createTileJ2;

    [SerializeField] private List<GameObject> _createLeader1;
    public List<GameObject> CreateLeader1 => _createLeader1;

    [SerializeField] private List<GameObject> _createLeader2;
    public List<GameObject> CreateLeader2 => _createLeader2;

    //Fais pour J1 pour le moment.
    public void CreateRenfort()
    {
        //A inverser si l'armée Bleu Devient l'armée Rouge
        if (RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.UsineJ1))
        {
            AroundCreateTileUsine(true);
            AroundLeader(true);
        }

        //A inverser si l'armée Rouge Devient l'armée Bleu
        else if (RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.UsineJ2))
        {
            AroundCreateTileUsine(false);
            AroundLeader(false);

        }
    }

    void AroundCreateTileUsine(bool usine1)
    {
        //Pour chaque Objet dans la liste, tu tchek le paramètre.
        foreach (GameObject typeTile in TilesManager.Instance.TileList)
        {
            //Si l'armée est jouer par l'armée Bleu.
            if (usine1 == true)
            {
                //Si la tile trouvé est égale contient le terme UsineJ1.
                if (typeTile.GetComponent<TileScript>().TerrainEffectList.Contains((MYthsAndSteel_Enum.TerrainType.UsineJ1)))
                {
                    int typeTileID = int.Parse(TilesManager.Instance.TileList.IndexOf(typeTile).ToString());
                    //Debug.Log(typeTile);
                    foreach (int idtyleIndex in PlayerStatic.GetNeighbourDiag(typeTileID, TilesManager.Instance.TileList[typeTileID].GetComponent<TileScript>().Line, false))
                    {
                        //Tu ajoutes la tile correspondant à l'usineJ1
                        if (!_createTileJ1.Contains(typeTile))
                        {
                            _createTileJ1.Add(typeTile);
                        }
                        //Si le numéro n'est pas présent dans la liste, tu l'ajoutes.
                        if (!_createTileJ1.Contains(TilesManager.Instance.TileList[idtyleIndex]))
                        {
                            _createTileJ1.Add(TilesManager.Instance.TileList[idtyleIndex]);
                            //Si il y un boolean qui est retourné alors tu enlèves les éléments de la liste.
                            if (CheckConditions(_createTileJ1) == true)
                            {
                                _createTileJ1.Remove(TilesManager.Instance.TileList[idtyleIndex]);
                                //Pour chaque élément dans la liste, tu ajoutes l'effet TileCréable
                                foreach (GameObject typeWithoutEffect in _createTileJ1)
                                {
                                    if (!typeWithoutEffect.GetComponent<TileScript>().EffetProg.Contains(MYthsAndSteel_Enum.EffetProg.Zone_creable))
                                    {
                                        typeWithoutEffect.GetComponent<TileScript>().EffetProg.Add(MYthsAndSteel_Enum.EffetProg.Zone_creable);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (usine1 == false)
            {
                if (typeTile.GetComponent<TileScript>().TerrainEffectList.Contains((MYthsAndSteel_Enum.TerrainType.UsineJ2)))
                {
                    int typeTileID = int.Parse(TilesManager.Instance.TileList.IndexOf(typeTile).ToString());
                    //Debug.Log(typeTile);
                    foreach (int idtyleIndex in PlayerStatic.GetNeighbourDiag(typeTileID, TilesManager.Instance.TileList[typeTileID].GetComponent<TileScript>().Line, false))
                    {
                        //Tu ajoutes la tile correspondant à l'usineJ2
                        if (!_createTileJ2.Contains(typeTile))
                        {
                            _createTileJ2.Add(typeTile);
                        }
                        //Si le numéro n'est pas présent dans la liste, tu l'ajoutes.
                        if (!_createTileJ2.Contains(TilesManager.Instance.TileList[idtyleIndex]))
                        {
                            _createTileJ2.Add(TilesManager.Instance.TileList[idtyleIndex]);
                            //Si il y un boolean qui est retourné alors tu enlèves les éléments de la liste.
                            if (CheckConditions(_createTileJ2) == true)
                            {
                                _createTileJ1.Remove(TilesManager.Instance.TileList[idtyleIndex]);
                                //Pour chaque élément dans la liste, tu ajoutes l'effet TileCréable
                                foreach (GameObject typeWithoutEffect in _createTileJ2)
                                {
                                    if (!typeWithoutEffect.GetComponent<TileScript>().EffetProg.Contains(MYthsAndSteel_Enum.EffetProg.Zone_creable))
                                    {
                                        typeWithoutEffect.GetComponent<TileScript>().EffetProg.Add(MYthsAndSteel_Enum.EffetProg.Zone_creable);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void AroundLeader(bool leaderArmy1)
    {
        //Pour chaque Objet dans la liste, tu tchek le paramètre.
        foreach (GameObject typeTile in TilesManager.Instance.TileList)
        {
            //Si l'armée est jouer par l'armée Bleu.
            if (leaderArmy1 == true)
            {
                //Pour que quand le code regarde dans la liste, si il tombe sur un objet null, évite de crash et de continuer
                if (typeTile.GetComponent<TileScript>().Unit != null)
                {
                    //Si la tile trouvé est égale au type de l'unité demandé, ici Leader.
                    if (typeTile.GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.typeUnite == MYthsAndSteel_Enum.TypeUnite.Leader)
                    {
                        //Evite d'avoir des doublons dans la List.
                        int typeTileID = int.Parse(TilesManager.Instance.TileList.IndexOf(typeTile).ToString());
                        Debug.Log(typeTileID);
                        //Pour chaque numéro présent dans le PlayerStatic avec la valeur qu'on a convertit précédemment.
                        foreach (int idtyleIndex in PlayerStatic.GetNeighbourDiag(typeTileID, TilesManager.Instance.TileList[typeTileID].GetComponent<TileScript>().Line, false))
                        {
                            //Si le numéro n'est pas présent dans la liste, tu l'ajoutes.
                            if (!_createLeader1.Contains(TilesManager.Instance.TileList[idtyleIndex]))
                            {
                                Debug.Log("PAsse par la ?");
                                _createLeader1.Add(TilesManager.Instance.TileList[idtyleIndex]);
                                {
                                    //Si il y un boolean qui est retourné alors tu enlèves les éléments de la liste.
                                    if (CheckConditions(_createLeader1) == true)
                                    {
                                        _createLeader1.Remove(TilesManager.Instance.TileList[idtyleIndex]);
                                        //Pour chaque élément dans la liste, tu ajoutes l'effet TileCréable
                                        foreach (GameObject typeWithoutEffect in _createTileJ2)
                                        {
                                            if (!typeWithoutEffect.GetComponent<TileScript>().EffetProg.Contains(MYthsAndSteel_Enum.EffetProg.Zone_creable))
                                            {
                                                typeWithoutEffect.GetComponent<TileScript>().EffetProg.Add(MYthsAndSteel_Enum.EffetProg.Zone_creable);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Si l'armée est jouer par l'armée Rouge.
            if (leaderArmy1 == false)
            {
                //Pour que quand le code regarde dans la liste, si il tombe sur un objet null, évite de crash et de continuer.
                if (typeTile.GetComponent<TileScript>().Unit != null)
                {
                    //Si la tile trouvé est égale au type de l'unité demandé, ici Leader.
                    if (typeTile.GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.typeUnite == MYthsAndSteel_Enum.TypeUnite.Leader)
                    {
                        //Je convertis sa position dans la liste en INT pour pouvoir utiliser le PlayerStatic GetNeighbour.
                        int typeTileID = int.Parse(TilesManager.Instance.TileList.IndexOf(typeTile).ToString());
                        Debug.Log(typeTileID);
                        //Pour chaque numéro présent dans le PlayerStatic avec la valeur qu'on a convertit précédemment.
                        foreach (int idtyleIndex in PlayerStatic.GetNeighbourDiag(typeTileID, TilesManager.Instance.TileList[typeTileID].GetComponent<TileScript>().Line, false))
                        {
                            //Si le numéro n'est pas présent dans la liste, tu l'ajoutes.
                            if (!_createLeader2.Contains(TilesManager.Instance.TileList[idtyleIndex]))
                            {
                                _createLeader2.Add(TilesManager.Instance.TileList[idtyleIndex]);
                                {
                                    //Si il y un boolean qui est retourné alors tu enlèves les éléments de la liste.
                                    if (CheckConditions(_createLeader2) == true)
                                    {
                                        _createLeader2.Remove(TilesManager.Instance.TileList[idtyleIndex]);
                                        //Pour chaque élément dans la liste, tu ajoutes l'effet TileCréable
                                        foreach (GameObject typeWithoutEffect in _createTileJ2)
                                        {
                                            if (!typeWithoutEffect.GetComponent<TileScript>().EffetProg.Contains(MYthsAndSteel_Enum.EffetProg.Zone_creable))
                                            {
                                                typeWithoutEffect.GetComponent<TileScript>().EffetProg.Add(MYthsAndSteel_Enum.EffetProg.Zone_creable);
                                            }
                                        }
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
    /// Permet de déterminer si autour des cases adjacentes il y a des effets de terrain qui pourraient gêner le déploiement de troupes.
    /// </summary>
    /// <param name="tileCheck"></param>
    /// <returns></returns>
    public static bool CheckConditions(List<GameObject> tileCheck)
    {
        bool stayEffect = false;
        foreach (GameObject tileEffect in tileCheck)
        {
            //Si l'effet de Case est différent du Ravin et de l'eau.
            if (tileEffect.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Ravin)
                || tileEffect.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Eau))
            {
                stayEffect = true;
                if (tileEffect.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Nord)
                && tileEffect.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Pont_Nord))
                {
                    stayEffect = true;
                    break;
                }

                if (tileEffect.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Sud)
                && tileEffect.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Pont_Sud))
                {
                    stayEffect = true;
                    break;
                }

                if (tileEffect.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest)
                && tileEffect.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Pont_Ouest))
                {
                    stayEffect = true;
                    break;
                }

                if (tileEffect.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Est)
                && tileEffect.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Pont_Est))
                {
                    stayEffect = true;
                    break;
                }
            }

            else
            {
                stayEffect = false;
            }
        }
        return stayEffect;

        //Ajouter les barbelets.
    }
}

