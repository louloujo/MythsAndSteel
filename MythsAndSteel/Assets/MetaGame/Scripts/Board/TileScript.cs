using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] private GameObject _unit;
    public GameObject  Unit => _unit;

    [SerializeField] private int _line;
    public int Line => _line;

    private void Start(){
        //Met l'unité à la bonne position
        if(_unit != null){
            _unit.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _unit.transform.position.z);
        }
    }


    /*    public void Select()
        {
            if (!Tiles.Instance._Selected)
            {
                if (Unit != null)
                {
                    Unit.GetComponent<UnitScript>()._Menu.enabled = true; // Active et désactive le menu atk, mvmt, cancel, power. 
                    Tiles.Instance._Selected = true;
                    Tiles.Instance._actualTileSelected = this.gameObject;
                }
            }
            else if (Tiles.Instance._Selected)
            {
                if (Tiles.Instance._Mouvement)
                {
                    Mouvement.Instance.AddMouvement(Tiles.Instance.Tile.IndexOf(gameObject));
                }
            }
        }*/

    /// <summary>
    /// Ajoute une unité à cette case
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnitToTile(GameObject unit){
        _unit = unit;
    }

    /// <summary>
    /// Enleve l'unité qui se trouve sur cette case
    /// </summary>
    public void RemoveUnitFromTile(){
        _unit = null;
    }
}
