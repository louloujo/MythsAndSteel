using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Event Scriptable")]
public class EventCardClass : ScriptableObject{
    //Nombre de carte event tirable
    [SerializeField] private int _numberCarteEvent = 0;
    public int NumberCarteEvent => _numberCarteEvent;

    public void PointeursLaserOptimisés()
    { }
}
