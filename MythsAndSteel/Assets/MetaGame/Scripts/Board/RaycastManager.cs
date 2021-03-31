using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoSingleton<RaycastManager>
{
    #region Appel de Script
    public MouseCommand _mouseCommand;
    #endregion

    #region Variables
    //Les layer qui sont détectés par le raycast
    [SerializeField] private LayerMask _layerM;

    //tile qui se trouve sous le raycast
    [SerializeField] private GameObject _tile;
    public GameObject Tile => _tile;
    //Dernière tile en mémoire par ce script
    GameObject _lastTile = null;

    //tile qui se trouve sous le raycast
    [SerializeField] private GameObject _unitInTile;
    public GameObject UnitInTile => _unitInTile;

    //Lorsque le joueur clique sur une tile
    [SerializeField] private GameObject _actualTileSelected;
    public GameObject ActualTileSelected
    {
        get
        {
            return _actualTileSelected;
        }
        set
        {
            _actualTileSelected = value;
        }
    }

    //Est ce que les joueurs peuvent jouer
    bool _isInTurn = false;

    //Event pour quand le joueur clique sur un bouton pour passer à la phase suivante
    public delegate void TileRaycastChange();
    public event TileRaycastChange OnTileChanged;
    #endregion Variables

    void Update()
    {
        //obtient le premier objet touché par le raycast
        RaycastHit2D hit = GetRaycastHit();

        //Remplace le gameObject Tile pour avoir en avoir une sauvegarde
        _tile = hit.collider != null ? hit.collider.gameObject : null;

        //Assigne l'unité si la tile qui est sélectionnée possède une unité
        _unitInTile = _tile != null ? _tile.GetComponent<TileScript>().Unit != null ? _tile.GetComponent<TileScript>().Unit : null : null;

        //Permet de combiner le Shift et le click gauche de la souris.
        if (_unitInTile == true)
        {
            //Si il il y a une unité sur la tile, le joueur peut utiliser ShiftClick.
            _mouseCommand.ShiftClick();
            //Si le joueur a utilisé le Shift puis leclick, le joueur est considéré comme click et on applique les fonctions propres au bouton des panneaux. De plus, le mouseOver est désactivé.
            if (_mouseCommand.CheckIfPlayerAsClic == true)
            {
                _mouseCommand.buttonAction(UIInstance.Instance.ButtonId);
                _mouseCommand.MouseExitWithoutClick();
            }
            else
            {
                //Si le joueur n'a pas continué sa combinaison d'action( Shif+clic), alors quand ma souris reste sur une case sans cliqué, l'interface résumé des statistiques s'active.
                _mouseCommand.MouseOverWithoutClick();
            }
        }
        else
        {
            //Si la case ne comporte pas d'unité alors le MouseOver ne s'active pas et n'affiche par l'interface résumé des statistiques.
            _mouseCommand.MouseExitWithoutClick();
        }

        //Si la tile change
        if (_tile != _lastTile || _isInTurn != GameManager.Instance.IsInTurn)
        {
            _isInTurn = GameManager.Instance.IsInTurn;
            _lastTile = _tile;
            OnTileChanged();
        }


        //Lorsque le joueur appui sur la souris
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_tile != null)
            {
                Select();
            }
        }
    }

    /// <summary>
    /// Permet d'obtenir les objets touchés par le raycast
    /// </summary>
    /// <returns></returns>
    public RaycastHit2D GetRaycastHit()
    {
        Vector2 mouseDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.mousePosition), mouseDirection);
        return Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, _layerM);
    }

    /// <summary>
    /// Quand tu cliques sur une unité
    /// </summary>
    public void Select()
    {
        if (!Mouvement.Instance.Selected)
        {
            if (_tile.GetComponent<TileScript>().Unit != null)
            {
                Mouvement.Instance.Selected = true;
                _actualTileSelected = _tile;
            }
        }

        else
        {
            if (Mouvement.Instance.IsInMouvement && !Mouvement.Instance.MvmtRunning)
            {
                if (_tile != _actualTileSelected)
                {
                    Mouvement.Instance.AddMouvement(TilesManager.Instance.TileList.IndexOf(_tile));
                }
                else
                {
                    Mouvement.Instance.StopMouvement(true);
                }
            }
        }
    }
}