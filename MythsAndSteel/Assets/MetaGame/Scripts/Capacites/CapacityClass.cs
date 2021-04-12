using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="CapacyList")]
public class CapacityClass : ScriptableObject
{
    [SerializeField] private List<Capacity> _capacityList = new List<Capacity>();
    public List<Capacity> EventCardList => _capacityList;
    public int test;




}


[System.Serializable]
public class Capacity
{
    public string CapacityName = "";
    [TextArea]
    public string Description = "";
}