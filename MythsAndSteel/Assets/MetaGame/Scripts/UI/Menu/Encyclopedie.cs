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
        LayoutButton[CurrentButtonIndex].GetComponent<Canvas>().sortingOrder = 1000;
    }

    public void Click(int index)
    {

        if (CurrentButtonIndex != index)
        {
            Debug.Log("button change");
            CurrentButtonIndex = index;
        }
    }

}
