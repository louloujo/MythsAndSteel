using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Va être convertit en pouvoir non ?
[CreateAssetMenu(menuName = "Unit/Unit Transport")]
public class Transport_Unit_SO : Unit_SO
{
    [Header("Listes des unités qui sont transportablles par l'unité")]
    //Listes des unités qui peuvent être transportables.
    [SerializeField] private List<GameObject> _unitTransport;
    public List<GameObject> UnitTransport => _unitTransport;

   
    //Nombre d'unité au maximum transportable
    [Header("Capacité de transport maximale")]
    [Range(1,4)]
    [SerializeField] private int _maxCapacityTransport;
    public int MaxCapacityTransport => _maxCapacityTransport;
}
