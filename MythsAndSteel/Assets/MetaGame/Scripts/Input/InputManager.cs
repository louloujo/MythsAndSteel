using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private KeyCode escapeEvent = KeyCode.Escape;
    [SerializeField] private KeyCode MoreInfoUnit = KeyCode.LeftShift;
    [SerializeField] private KeyCode MakeAction = KeyCode.KeypadEnter;

    void Update(){
        //Pour quitter la phase d'�v�nement qui permet de choisir une case ou une unit�
        if(Input.GetKeyDown(escapeEvent) && (GameManager.Instance.ChooseUnitForEvent || GameManager.Instance.ChooseTileForEvent)){
            GameManager.Instance.StopEventModeTile();
            GameManager.Instance.StopEventModeUnit();
        }

        //Quand on shiftclic  sur le plateau
        if(Input.GetMouseButtonDown(0) && Input.GetKey(MoreInfoUnit)){
            RaycastManager.Instance._mouseCommand.clickQuit();
            RaycastManager.Instance._mouseCommand._checkIfPlayerAsClic = false;
            RaycastManager.Instance._mouseCommand._hasCheckUnit = false;
            RaycastManager.Instance._mouseCommand._checkIfPlayerAsClic = true;
        }

        //Clic sur le plateau ou une unit�
        if(Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift)){
            if(EventSystem.current.IsPointerOverGameObject() == false){
                RaycastManager.Instance.Select();
            }
        }

        //Poser la zone d'orgone � sa nouvelle position
        if(Input.GetMouseButtonUp(0) && OrgoneManager.Instance.Selected){
            OrgoneManager.Instance.ReleaseZone();
        }

        //Quand le joueur clic sur entr�e pour valider une action
        if(Input.GetKeyDown(MakeAction)){
            if(Mouvement.Instance.IsInMouvement && Mouvement.Instance._selectedTileId.Count > 1){
                Mouvement.Instance.ApplyMouvement();
                Mouvement.Instance.DeleteChildWhenMove();
            }
        }
    }
}