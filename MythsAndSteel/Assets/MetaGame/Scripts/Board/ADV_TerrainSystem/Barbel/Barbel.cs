using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbel : TerrainParent
{
    [SerializeField] private int _Turnleft;
    public int TurnLeft
    {
        get
        {
            return _Turnleft;
        }
        set
        {
            _Turnleft = value;
            if(_Turnleft <= 0)
            {
                Remove();
            }
        }
    }
    [HideInInspector] public BarbelGestion BarbelG;
    [SerializeField] private BarbelGestion.Direction _Direc;
    public BarbelGestion.Direction Direc
    {
        get
        {
            return _Direc;
        }
        set
        {
            UpdateRender();
            _Direc = value;
        }
    }

    public void Start()
    {
        UpdateRender();
    }

    public void UpdateRender()
    {
        if(Direc == BarbelGestion.Direction.Est)
        {
            GetComponent<SpriteRenderer>().sprite = BarbelG.Vertical;
            transform.localPosition = Vector3.zero;
            transform.localPosition += new Vector3(.5f, 0, 0);
        }
        if (Direc == BarbelGestion.Direction.Nord)
        {
            GetComponent<SpriteRenderer>().sprite = BarbelG.Horizontal;
            transform.localPosition = Vector3.zero;
            transform.localPosition += new Vector3(0, .5f, 0);
        }
        if (Direc == BarbelGestion.Direction.Sud)
        {
            GetComponent<SpriteRenderer>().sprite = BarbelG.Horizontal;
            transform.localPosition = Vector3.zero;
            transform.localPosition += new Vector3(0, -.5f, 0);
        }
        if (Direc == BarbelGestion.Direction.Ouest)
        {
            GetComponent<SpriteRenderer>().sprite = BarbelG.Vertical;
            transform.localPosition = Vector3.zero;
            transform.localPosition += new Vector3(-.5f, 0, 0);
        }
    }

    public override void ComingFromUp(UnitScript Unit)
    {
        if(Direc == BarbelGestion.Direction.Nord)
        {
            Debug.Log("Damage");
            Unit.TakeDamage(2);
        }
        base.ComingFromUp(Unit);
    }

    public override void ComingFromDown(UnitScript Unit)
    {
        if (Direc == BarbelGestion.Direction.Sud)
        {
            Debug.Log("Damage");
            Unit.TakeDamage(2);
        }
        base.ComingFromDown(Unit);
    }

    public override void ComingFromLeft(UnitScript Unit)
    {
        if (Direc == BarbelGestion.Direction.Ouest)
        {
            Debug.Log("Damage");
            Unit.TakeDamage(2);
        }
        base.ComingFromLeft(Unit);
    }

    public override void ComingFromRight(UnitScript Unit)
    {
        if (Direc == BarbelGestion.Direction.Est)
        {
            Debug.Log("Damage");
            Unit.TakeDamage(2);
        }
        base.ComingFromRight(Unit);
    }

    public override void QuitToDown(UnitScript Unit)
    {
        if (Direc == BarbelGestion.Direction.Sud)
        {
            Debug.Log("Damage");
            Unit.TakeDamage(2);
        }
        base.QuitToDown(Unit);
    }

    public override void QuitToLeft(UnitScript Unit)
    {
        if (Direc == BarbelGestion.Direction.Ouest)
        {
            Debug.Log("Damage");
            Unit.TakeDamage(2);
        }
        base.ComingFromLeft(Unit);
        base.QuitToLeft(Unit);
    }

    public override void QuitToRight(UnitScript Unit)
    {
        if (Direc == BarbelGestion.Direction.Est)
        {
            Debug.Log("Damage");
            Unit.TakeDamage(2);
        }
        base.ComingFromRight(Unit);
        base.QuitToRight(Unit);
    }

    public override void QuitToUp(UnitScript Unit)
    {
        if (Direc == BarbelGestion.Direction.Nord)
        {
            Debug.Log("Damage");
            Unit.TakeDamage(2);
        }
        base.ComingFromUp(Unit);
        base.QuitToUp(Unit);
    }

    public void Remove()
    {
        GetComponent<Animator>().SetBool("Out", true);
    }

    public void OnDestroy()
    {       
        BarbelG.BarbelActive.Remove(this);
        if (BarbelG.BarbelActive.Contains(this))
        {
            BarbelG.BarbelActive.Remove(this);
        }
    }
}
