using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName ="Terrain Scriptable")]
public class TerrainTypeClass : ScriptableObject
{
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Desc;
    [SerializeField] private SpriteRenderer Rendu;
    [SerializeField] private TextMeshProUGUI Ressources;

    [SerializeField] private TerrainType[] _EffetDeTerrain;
    public TerrainType[] EffetDeTerrain
    {
        get
        {
            return _EffetDeTerrain;
        }
    }

    public void Synch(TileScript tile)
    {
        TerrainType Saved = FindEffect(tile.TerrainEffectList[0]);
        Title.text = Saved._terrainName;
        Desc.text = Saved._description;
        Rendu.sprite = Saved.render;
        if (tile.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_de_ressource))
        {
            int i = 0; // Temp valeur de ressource.
            Ressources.text = i <= 0 ?  "" : "Ressources sur la case: " + i;
        }
        else { Ressources.text = ""; }
    }

    protected TerrainType FindEffect(MYthsAndSteel_Enum.TerrainType Type)
    {
        foreach(TerrainType tr in _EffetDeTerrain)
        {
            foreach (MYthsAndSteel_Enum.TerrainType TypeTerrain in tr._eventType)
            {
                if (TypeTerrain == Type)
                {
                    return tr;
                }
            }
        }
        return null;
    }
}

/// <summary>
/// Class qui regroupe toutes les variables pour une tile.
/// </summary>
[System.Serializable]
public class TerrainType 
{
    public string _terrainName = "";
    [TextArea] public string _description = "";
    public List<MYthsAndSteel_Enum.TerrainType> _eventType = new List<MYthsAndSteel_Enum.TerrainType>();
    public Sprite render;
}
