using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoSingleton<Menu>
{
    [SerializeField] private GameObject[] arrow;

    private void Start()
    {
        foreach (GameObject ar in arrow)
        {
            ar.SetActive(false);
        }
    }


    public enum type
    {
        cancel, mvmt, attack, power
    }
    [SerializeField] private type _type;

    public void Selector()
    {
        foreach(GameObject ar in arrow)
        {
            ar.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Click()
    {
        switch (_type)
        {
            case type.attack:
                break;
            case type.cancel:
                break;
            case type.mvmt:
                if (!TilesManager.Instance._Mouvement)
                {
                    Mouvement.Instance.StartMouvement(GetComponentInParent<UnitScript>().ActualTiledId, GetComponentInParent<UnitScript>().MoveLeft);
                    TilesManager.Instance._Mouvement = true;
                }
                break;
            case type.power:
                break;
        }
    }
}
