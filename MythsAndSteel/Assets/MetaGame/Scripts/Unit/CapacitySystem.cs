using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapacitySystem : MonoSingleton<CapacitySystem>
{
    [SerializeField] private Sprite attacklaunchspritebutton;
    [SerializeField] private Sprite attackcancelspritebutton;

    public void Updatebutton()
    {
        GameObject Unit = RaycastManager.Instance.ActualUnitSelected;

        if (Unit != null)
        {
            if (!Mouvement.Instance.MvmtRunning && !Attaque.Instance.attackselected && !Unit.GetComponent<UnitScript>().IsActivationDone && !Mouvement.Instance.mvmtrunning)
            {
                ButtonLaunchCapacity.SetActive(true);

                if (Unit.GetComponent<UnitScript>().GotCapacity())
                {
                    if (Unit.GetComponent<UnitScript>().RunningCapacity)
                    {
                        ButtonLaunchCapacity.GetComponent<Image>().sprite = attackcancelspritebutton;
                        UIInstance.Instance.DesactivateNextPhaseButton();
                    }
                    else
                    {
                        ButtonLaunchCapacity.GetComponent<Image>().sprite = attacklaunchspritebutton;
                        UIInstance.Instance.ActivateNextPhaseButton();
                    }
                }
            }
            else
            {
                ButtonLaunchCapacity.SetActive(false);
                UIInstance.Instance.ActivateNextPhaseButton();
            }
        }
    }

    [SerializeField] private GameObject ButtonLaunchCapacity;

    public void CapacityButton()
    {
        GameObject Unit = RaycastManager.Instance.ActualUnitSelected;

        if (Unit != null)
        {
            if (!Unit.GetComponent<UnitScript>().IsActivationDone)
            {
                if (!Unit.GetComponent<UnitScript>().RunningCapacity)
                {
                    Attaque.Instance.StopAttack();
                    Mouvement.Instance.StopMouvement(true);
                    Unit.GetComponent<UnitScript>().StartCapacity();
                    Unit.GetComponent<UnitScript>().RunningCapacity = true;
                    Updatebutton();
                }
                else if (Unit.GetComponent<UnitScript>().RunningCapacity)
                {
                    Unit.GetComponent<UnitScript>().StopCapacity();
                    Unit.GetComponent<UnitScript>().RunningCapacity = false;
                    Updatebutton();
                }
            }
        }
    }


}
