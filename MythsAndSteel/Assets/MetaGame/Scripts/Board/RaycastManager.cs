using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoSingleton<RaycastManager>{
    //Les layer qui sont détectés par le raycast
    [SerializeField] private LayerMask layerM;

    //tile qui se trouve sous le raycast
    [SerializeField] private GameObject _tile;
    public GameObject Tile => _tile;


    void Update(){
        //obtient le premier objet touché par le raycast
        RaycastHit2D hit = GetRaycastHit();

        //Remplace le gameObject Tile pour avoir en avoir une sauvegarde
        _tile = hit.collider != null? hit.collider.gameObject : null;
    }
    
    /// <summary>
    /// Permet d'obtenir les objets touchés par le raycast
    /// </summary>
    /// <returns></returns>
    RaycastHit2D GetRaycastHit(){
        Vector2 mouseDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.mousePosition), mouseDirection);
        return Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerM);
    }
}