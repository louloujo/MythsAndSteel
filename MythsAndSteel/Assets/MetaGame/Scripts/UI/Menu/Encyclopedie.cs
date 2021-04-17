using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encyclopedie : MonoBehaviour
{
    public GameObject[] LayoutButton;
    

    public int CurrentButtonIndex = 0;

    [SerializeField] bool _SameIndex;
    
   
    private void Start()
    {
        LayoutButton[CurrentButton].GetComponent<Canvas>().sortingOrder = 1000;
    }

}
