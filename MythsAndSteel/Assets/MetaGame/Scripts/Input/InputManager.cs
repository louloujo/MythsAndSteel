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
        //Pour quitter la phase d'événement qui permet de choisir une case ou une unité
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

        //Clic sur le plateau ou une unité
        if(Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftShift)){
            if(EventSystem.current.IsPointerOverGameObject() == false){
                RaycastManager.Instance.Select();
            }
        }

        //Quand le joueur clic sur entrée pour valider une action
        if(Input.GetKeyDown(MakeAction)){
            if(Mouvement.Instance.IsInMouvement && Mouvement.Instance._selectedTileId.Count > 1){
                Mouvement.Instance.ApplyMouvement();
                Mouvement.Instance.DeleteChildWhenMove();
            }
        }
    }
}
