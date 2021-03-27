using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    //List des tiles sur le plateau (rangée dans l'ordre)
    [Tooltip("NE TOUCHEZ A RIEN SI VOUS INVERSEZ L'ORDRE DES TILES DANS LA LISTE CA NE MARCHERA PLUS!")]
    [SerializeField] private List<GameObject> _tileList;
    public List<GameObject> TileList => _tileList;
}


