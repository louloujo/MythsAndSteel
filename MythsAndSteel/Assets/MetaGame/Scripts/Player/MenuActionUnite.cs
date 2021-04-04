using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Ce script gère l'Affichage de Menu d'Action d'une Unité (déplacement, attaque, pouvoir) et tout ses vérifications.
/// </summary>
public class MenuActionUnite : MonoBehaviour
{
    public MouseCommand mousecommand;
    
    private bool J1unit = true;
    //PlayerScripts
    private bool J1aEncorePointsActivation = true;
    private bool J2aEncorePointsActivation = true;
    // a rajoutter dans le playerscriptunite
     bool pouvoiractif = false;
    [SerializeField]
    private GameObject UIMenuActionUnite;
    [SerializeField]
    private RectTransform Button1;
    [SerializeField]
    private RectTransform Button2;
    [SerializeField]
    private RectTransform Button3;
    [SerializeField]
  
    
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        //On vérifie qu'on dans la phase d'action du joueur 1
        if (GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1)
        {

            // Si le joueur clique sur une tile ou il y a pas d'unité ferme le menu d'action
                if (RaycastManager.Instance.UnitInTile == false && Input.GetMouseButtonDown(0))
                {
                    UIMenuActionUnite.SetActive(false);
                }
            //On vérifie si le joueur clique sur une tile ou il y a une tile
            if (RaycastManager.Instance.UnitInTile == true && Input.GetMouseButtonDown(0))
            {
                // On vérifie si le joueur a encore des points d'activations
                if (J1aEncorePointsActivation == true)
                {
                    // On vérifie que l'unité cliqué appartient au joueur 1 puis on lance l'affichage du menu avec la fonction menuaffichage
                    if (PlayerStatic.CheckIsUnitArmy(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), J1unit) == true)
                    {

                      
                        menuaffichage(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), pouvoiractif);
                    }

                }
            }
        }
        //Même principe mais pour le joueur2
        if (GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2)
        {
            if (RaycastManager.Instance.UnitInTile == false && Input.GetMouseButtonDown(0))
            {
                UIMenuActionUnite.SetActive(false);
            }


            if (RaycastManager.Instance.UnitInTile == true && Input.GetMouseButtonDown(0))
            {
                if (RaycastManager.Instance.UnitInTile == false && Input.GetMouseButtonDown(0))
                {
                    UIMenuActionUnite.SetActive(false);
                }
                if (J1aEncorePointsActivation == true)
                {

                    if (PlayerStatic.CheckIsUnitArmy(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), J1unit) == false)
                    {

                      
                        menuaffichage(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), pouvoiractif);
                    }

                }
            }
        }




    }
    /// <summary>
    /// Cette fonction gère l'affichage des différentes actions que le joueur va pouvoir effectuer sur l'unité contre un point activation.
    /// </summary>
    void menuaffichage(UnitScript unitscript, bool capaActive)
    {
      //Vérifie que l'unité à une capacité active et désactive le bouton si elle en a pas
        if (capaActive == false)
        {
            Button3.gameObject.SetActive(false);
        }
        //Vérifie si l'unité a effectuer tout son déplacement si c'est le cas alors décale la position des boutons en supprimant le bouton de déplacement
        if (unitscript.IsMoveDone == true)
        {
            Button3.position = Button2.position;
            Button2.position = Button1.position;
            Button1.gameObject.SetActive(false);

 

        }
        // place le menu à coté de l'unité selectionnée
        mousecommand.ActivateUI(UIMenuActionUnite, 2, 2, false) ;
    }
    public void déplacement()
    {
        Debug.Log("déplacement");
        UIMenuActionUnite.SetActive(false);
    }
    public void attaque()
    {
        Debug.Log("attaque");
        UIMenuActionUnite.SetActive(false);
    }

    public void capacité()
    {
        Debug.Log("pouvoir");
        UIMenuActionUnite.SetActive(false);
    }
}
