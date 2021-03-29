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

    [SerializeField] private int _line;
    public int Line => _line;

    private void Start(){
        //Met l'unité à la bonne position
        if(_unit != null){
            _unit.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _unit.transform.position.z);
        }
    }

    // AV. Test -----------------------------------------------------
    private void OnMouseDown()
    {
        Debug.Log("AV.Script; Click!");
        Select();
    }
        public void Select()
        {
            if (!TilesManager.Instance._Selected)
            {
                if (Unit != null)
                {
                Debug.Log("AV.Script; Unit selected!");
                TilesManager.Instance._Selected = true;
                TilesManager.Instance._actualTileSelected = this.gameObject;
                }
            }
            else if (TilesManager.Instance._Selected)
            {
                if (TilesManager.Instance._Mouvement)
                {
                Debug.Log("AV.Script; Path selected!");
                Mouvement.Instance.AddMouvement(TilesManager.Instance.TileList.IndexOf(gameObject));
                }
            }
        }


    // -----------------------------------------------------

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
