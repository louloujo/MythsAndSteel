using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoSingleton<TilesManager>
{
    //List des tiles sur le plateau (rangée dans l'ordre)
    [Tooltip("NE TOUCHEZ A RIEN SI VOUS INVERSEZ L'ORDRE DES TILES DANS LA LISTE CA NE MARCHERA PLUS!")]
    [SerializeField] private List<GameObject> _tileList;
    public List<GameObject> TileList => _tileList;


    // Test mvmt -------------------------------------------
    [SerializeField] private bool Mouvement;
    public bool _Mouvement
    {
        get
        {
            return Mouvement;
        }
        set
        {
            Mouvement = value;
        }
    }
    [SerializeField] private bool Selected;
    public bool _Selected
    {
        get
        {
            return Selected;
        }
        set
        {
            Selected = value;
        }
    }
}


