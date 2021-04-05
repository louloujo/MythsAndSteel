using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Ce script gère l'Affichage de Menu d'Action d'une Unité (déplacement, attaque, pouvoir) et tout ses vérifications.
/// </summary>
public class MenuActionUnite : MonoBehaviour
{
    [SerializeField] private MouseCommand _mousecommand;
    public MouseCommand MouseCommand => _mousecommand;
    
    //PlayerScripts
    private bool J1aEncorePointsActivation = true;
    private bool J2aEncorePointsActivation = true;

    // a rajoutter dans le playerscriptunite
    [SerializeField] bool pouvoiractif = true;
    [SerializeField] private GameObject UIMenuActionUnite;
    [SerializeField] private RectTransform _moveBtn;
    [SerializeField] private RectTransform _atkBtn;
    [SerializeField] private RectTransform _powerBtn;
    [SerializeField] private RectTransform _quitBtn;

    [SerializeField] private GameObject _movePanel = null;

    private void Start(){
        _movePanel.SetActive(false);
        UIMenuActionUnite.SetActive(false);
    }

    /// <summary>
    /// Affiche le panneau avec les boutons
    /// </summary>
    public void ShowPanel(){
        if(J1aEncorePointsActivation && GameManager.Instance.IsPlayerRedTurn){
            menuaffichage(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), pouvoiractif);
        }
        else if(J2aEncorePointsActivation && !GameManager.Instance.IsPlayerRedTurn){
            menuaffichage(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), pouvoiractif);
        }
    }

    /// <summary>
    /// Ferme le panneau avec les boutons
    /// </summary>
    public void closePanel(){
        UIMenuActionUnite.SetActive(false);
        _moveBtn.gameObject.GetComponent<MouseOverUI>().StopOver();
        _atkBtn.gameObject.GetComponent<MouseOverUI>().StopOver();
        _powerBtn.gameObject.GetComponent<MouseOverUI>().StopOver();
        _quitBtn.gameObject.GetComponent<MouseOverUI>().StopOver();
    }

    /// <summary>
    /// Cette fonction gère l'affichage des différentes actions que le joueur va pouvoir effectuer sur l'unité contre un point activation.
    /// </summary>
    void menuaffichage(UnitScript unitscript, bool capaActive)
    {
      //Vérifie que l'unité à une capacité active et désactive le bouton si elle en a pas
        if (capaActive == false)
        {
            _powerBtn.gameObject.SetActive(false);
        }
        //Vérifie si l'unité a effectuer tout son déplacement si c'est le cas alors décale la position des boutons en supprimant le bouton de déplacement
        if (unitscript.IsMoveDone == true)
        {
            _moveBtn.gameObject.SetActive(false);
        }

        // place le menu à coté de l'unité selectionnée
        MouseCommand.ActivateUI(UIMenuActionUnite, 0.5f, 3, false, true);
    }
    public void déplacement()
    {
        Mouvement.Instance.StartMvmtForSelectedUnit();
        Mouvement.Instance.Selected = true;
        closePanel();
    }
    public void attaque()
    {
        Debug.Log("attaque");
        closePanel();
    }

    public void capacité()
    {
        Debug.Log("pouvoir");
        closePanel();
    }



    public void ShowMovementPanel(){
        _movePanel.SetActive(true);
    }

    public void CloseMovementPanel(){
        _movePanel.SetActive(false);
    }
}
