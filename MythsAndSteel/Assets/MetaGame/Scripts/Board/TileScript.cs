using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] private GameObject _unit;
    public GameObject Unit
    {
        get
        {
            return _unit;
        }
        set
        {
            _unit = value;
        }
    }

    //Liste des enfants de la case
    [SerializeField] private List<GameObject> Child;
    public List<GameObject> _Child
    {
        get
        {
            return Child;
        }
        set
        {
            Child = value;
        }
    }

    //Ligne sur laquelle se trouve la case
    [SerializeField] private int _line;
    public int Line => _line;

    //Id de la case
    [SerializeField] private int _tileId;
    public int TileId => _tileId;
    
    [Space(20)]
    //Liste des effets de terrain sur chaque tile
    [SerializeField] private List<MYthsAndSteel_Enum.TerrainType> _terrainEffectList = new List<MYthsAndSteel_Enum.TerrainType>();
    public List<MYthsAndSteel_Enum.TerrainType> TerrainEffectList => _terrainEffectList;


    //Liste des effets de terrain sur chaque tile
    [SerializeField] private List<MYthsAndSteel_Enum.EffetProg> _effetProg = new List<MYthsAndSteel_Enum.EffetProg>();
    public List<MYthsAndSteel_Enum.EffetProg> EffetProg => _effetProg;


    [Header("VARIABLES EFFET DE TERRAIN")]
    [SerializeField] MYthsAndSteel_Enum.Owner _ownerObjectiv = MYthsAndSteel_Enum.Owner.neutral;
    public MYthsAndSteel_Enum.Owner OwnerObjectiv => _ownerObjectiv;

    [SerializeField] int _resourcesCounter = 0;
    public int ResourcesCounter => _resourcesCounter;

    private void Start(){
        //Met l'unité à la bonne position
        if(_unit != null){
            _unit.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _unit.transform.position.z);
            _unit.GetComponent<UnitScript>().ActualTiledId = TilesManager.Instance.TileList.IndexOf(this.gameObject);
        }

        for(int i = 0; i < transform.childCount; i++){
            _Child.Add(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Ajoute une unité à cette case
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnitToTile(GameObject unit){
        _unit = unit;
        _unit.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _unit.transform.position.z);
        _unit.GetComponent<UnitScript>().ActualTiledId = TilesManager.Instance.TileList.IndexOf(this.gameObject);
    }

    /// <summary>
    /// Enleve l'unité qui se trouve sur cette case
    /// </summary>
    public void RemoveUnitFromTile(){
        _unit = null;
    }

    /// <summary>
    /// Active un enfant à l'unité
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject ActiveChildObj(MYthsAndSteel_Enum.ChildTileType type, Sprite sprite = null){
        GameObject child = null;

        foreach(GameObject gam in _Child){ 
            string tag = "";

            switch(type){
                case MYthsAndSteel_Enum.ChildTileType.MoveSelect:
                    tag = "MoveSelectable";
                    break;
                case MYthsAndSteel_Enum.ChildTileType.AttackSelect:
                    tag = "AttackSelectable";
                    break;
                case MYthsAndSteel_Enum.ChildTileType.EventSelect:
                    tag = "SelectableTile";
                    break;
            }

            if(gam.tag == tag){
                child = gam;
                child.GetComponent<SpriteRenderer>().enabled = true;
                child.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                if(sprite != null) child.GetComponent<SpriteRenderer>().sprite = sprite;
            }
        }
        return child;
    }

    /// <summary>
    /// Désactive un enfant à l'unité
    /// </summary>
    /// <param name="type"></param>
    /// <param name="destroy"></param>
    /// <returns></returns>
    public GameObject DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType type, bool destroy = false){
        GameObject child = null;

        foreach(GameObject gam in _Child)
        {
            string tag = "";

            switch(type)
            {
                case MYthsAndSteel_Enum.ChildTileType.MoveSelect:
                    tag = "MoveSelectable";
                    break;
                case MYthsAndSteel_Enum.ChildTileType.AttackSelect:
                    tag = "AttackSelectable";
                    break;
                case MYthsAndSteel_Enum.ChildTileType.EventSelect:
                    tag = "SelectableTile";
                    break;
            }

            if(gam.tag == tag)
            {
                child = gam;
                child.GetComponent<SpriteRenderer>().enabled = false;
                if(destroy){
                    _Child.Remove(child);
                    Destroy(child);
                    child = null;
                }
            }
        }
        return child;
    }

    /// <summary>
    /// Change le joueur qui possède le drapeau
    /// </summary>
    /// <param name="own"></param>
    public void ChangePlayerObj(MYthsAndSteel_Enum.Owner own){
        _ownerObjectiv = own;
    }

    /// <summary>
    /// Enleve des ressources à la case
    /// </summary>
    /// <param name="value"></param>
    public void RemoveRessources(int value, int player){
        if(_resourcesCounter - value >= 0){
            _resourcesCounter -= value;

            if(player == 1)
            {
                PlayerScript.Instance.RedPlayerInfos.Ressource += value;
            }
            else
            {
                PlayerScript.Instance.BluePlayerInfos.Ressource += value;
            }
        }
        else{
            int ressourceToGice = value - (_resourcesCounter - value);

            if(player == 1){
                PlayerScript.Instance.RedPlayerInfos.Ressource += value;
            }
            else{
                PlayerScript.Instance.BluePlayerInfos.Ressource += value;
            }
        }
    }
}
