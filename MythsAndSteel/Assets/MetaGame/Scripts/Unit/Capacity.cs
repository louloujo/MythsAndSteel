using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Capacity : MonoBehaviour
{
    //----------------------------Capacité 1--------------------------------------
    [Header("-----------------Capacité 1-----------------")]
    [SerializeField] string _Capacity1Name = "";
    public string Capacity1Name => _Capacity1Name;

    [TextArea]
    [SerializeField] string _Capacity1Description = "";
    public string Capacity1Description => _Capacity1Description;

    [SerializeField] int _Capacity1Cost;
    public int Capacity1Cost => _Capacity1Cost;

    virtual public void Capacite1()
    {
        Debug.Log("Active La capacité 1");
    }


    [Header("-----------------Capacité 2-----------------")]
    //----------------------------Capacité 2--------------------------------------
    [SerializeField] bool _isCapacity2Exist;
    public bool isCapacity2Exist => _isCapacity2Exist;

    [SerializeField] string _Capacity2Name = "";
    public string Capacity2Name => _Capacity1Name;

    [TextArea]
    [SerializeField] string _Capacity2Description = "";
    public string Capacity2Description => _Capacity1Description;

    [SerializeField] int _Capacity2Cost;
    public int Capacity2Cost => _Capacity2Cost;
    public virtual void Capacite2()
    {
        Debug.Log("Active La capacité 2");
    }

}