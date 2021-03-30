using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Script contenant les controles clavier+souris pour l'UI ainsi que le MouseOver quand la souris survole un élément d'UI.
/// </summary>
public class MouseCommand : MonoBehaviour
{
    [SerializeField] private bool _checkIfPlayerAsClic;
    public bool CheckIfPlayerAsClic => _checkIfPlayerAsClic;


    [Header("UI Statistiques Unité")]
    //Le panneau à afficher lorsqu'on souhaite voir les statistiques de l'unité en cliquant.
    [SerializeField] private GameObject _mouseOverUI;
    public GameObject MouseOverUI => _mouseOverUI;
    //Le panneau ou les panneaux à afficher lorsqu'on souhaite le shift click sur l'unité
    [SerializeField] private List<GameObject> _shiftUI;
    public List<GameObject> ShiftUI => _shiftUI;
    [Header("Délai Coroutine pour MouseOver")]
    //Paramètre de délai qui s'applique à la couritine.
    [SerializeField] private float _timeToWait = 2f;
    public float TimeToWait => _timeToWait;
    [Header("Valeur de la position de l'UI en Mouse")]
    //Permet de modifier la position de l'UI dans l'escpace
    [SerializeField] private int _offsetX;
    public int OffSetX => _offsetX;
    [SerializeField] private int _offsetY;
    public int OffSetY => _offsetY;


    /// <summary>
    /// Permet de déterminer quand le joueur appuie sur le Shift puis le clic Gauche de la souris.
    /// </summary>
    public void ShiftClick()
    {
        //Si le joueur appuie sur la touche Shift Gauche (Attention, il y a 2 shifts sur un clavier !)
        if (Input.GetKey("left shift"))
        {
            //Si le joueur appuie sur le click gauche de sa souris.
            if (Input.GetMouseButtonDown(0))
            {
                //Si le joueur a éxécuté les actions précédentes, il est considéré comme quoi le joueur a cliqué donc on active le premier panneau.
                _checkIfPlayerAsClic = true;
                ActivateUI(ShiftUI[0], _offsetX, _offsetY);
            }
        }
    }

    /// <summary>
    /// Permet d'activer un élément de l'UI en utilisant un Raycast distint de la position et d'assigner une position custom par rapport au Canvas (Conflit avec le Canvas).
    /// </summary>
    /// <param name="uiElements"></param>
    /// <param name="offSetX"></param>
    /// <param name="offSetY"></param>
    public void ActivateUI(GameObject uiElements, int offSetX, int offSetY)
    {
        //Reprendre la position du raycast qui a sélectionné la tile
        RaycastHit2D hit = RaycastManager.Instance.GetRaycastHit();
        //Je stop l'ensemble des coroutines en cour.
        StopAllCoroutines();
        //Je détermine une position de référence dans l'espace (pour que la position de l'UI soit par rapport au Canvas et non à l'objet).
        Vector3 pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + offSetX, hit.transform.position.y + offSetY, hit.transform.position.z));
        //Rendre l'élément visible.
        uiElements.SetActive(true);
        //Si la position de l'UI est différente de celle de la position de référence alors tu prends cette position comme référence.
        if (uiElements.transform.position != pos)
        {
            uiElements.transform.position = pos;
        }
    }
    /// <summary>
    /// Permet de déterminer et d'afficher un élément quand la souris passe au dessus d'une tuile possédant une unité.
    /// </summary>
    public void MouseOverWithoutClick()
    {
        //Si le joueur n'a pas cliqué, alors tu lances la coroutine.
        if (_checkIfPlayerAsClic == false)
        {
            //Coroutine : Une coroutine est une fonction qui peut suspendre son exécution (yield) jusqu'à la fin de la YieldInstruction donnée.
            StartCoroutine(ShowObject(TimeToWait));
        }
        else
        {
            //Si le joueur click, alors je cache le MouseOver.
            MouseExitWithoutClick();
        }
    }

    /// <summary>
    /// Fonction pour désactiver en MouseOver.
    /// </summary>
    public void MouseExitWithoutClick()
    {
        //Arrete l'ensemble des coroutines dans la scène.
        StopAllCoroutines();
        _mouseOverUI.SetActive(false);
    }

    /// <summary>
    /// Correspond au paramètre qu'on rentre dans la coroutine.
    /// </summary>
    /// <param name="Timer"></param>
    /// <returns></returns>
    IEnumerator ShowObject(float TimeToWait)
    {
        //J'utilise un délai pour que le boutton apparaisse après un délai.
        yield return new WaitForSeconds(TimeToWait);
        //J'active l'élément et je lui assigne des paramètres.
        ActivateUI(MouseOverUI, _offsetX, _offsetY);
    }

    /// <summary>
    /// JE prends une liste de boutton, a chaque bouton j'assigne un index. 
    /// </summary>
    /// <param name="button"></param>
    public void buttonAction(List<Button> button)
    {
        //0 et  1 sont pour les boutons quitter, 2 et 3 sont pour switch entre la Page 1 et la Page 2
        button[0].onClick.AddListener(clickQuit);
        button[1].onClick.AddListener(clickQuit);
        button[2].onClick.AddListener(switchWindows1);
        button[3].onClick.AddListener(switchWindows2);

        //Fonction qui permet de cacher les Pages 1 et 2 du carnet.
        void clickQuit()
        {
            //Je retourne la valeur comme quoi il a clické à false car il a fini son action de Shift+Clic et désactive les 2 pages.
            _checkIfPlayerAsClic = false;
            ShiftUI[0].SetActive(false);
            ShiftUI[1].SetActive(false);
        }

        //Switch entre la page 1 et la page 2.
        void switchWindows1()
        {
            //J'active le Panneau 2 car le joueur a cliqué sur le bouton permettant de transitionner de la page 1 à la page 2. De plus, je masque la page 1.
            ActivateUI(ShiftUI[1], -_offsetX, _offsetY);
            ShiftUI[0].SetActive(false);
        }

        //Switch entre la page 2 et la page 1.
        void switchWindows2()
        {
            //J'active le Panneau 1 car le joueur a cliqué sur le bouton permettant de transitionner de la page 2 à la page 1. De plus, je masque la page 2.
            ActivateUI(ShiftUI[0], _offsetX, _offsetY);
            ShiftUI[1].SetActive(false);
        }
    }
}
