using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Ce Script va gérer l'utilisation des charges d'orgones. Tous les scripts des charges d'orgone dérive de ce dernier. 
C'est pour cela qu'il ne faut surtout pas le MODIFIER !! 
 */ 
public class OrgoneManager : MonoSingleton<OrgoneManager>
{
    #region Variables
    [Header("PARENT JAUGE D'ORGONE")]
    //Jauge d'orgone joueur rouge
    [SerializeField] private GameObject _redPlayerPanelOrgone = null;
    public GameObject RedPlayerPanelOrgone => _redPlayerPanelOrgone;

    //Jauge d'orgone joueur bleu
    [SerializeField] private GameObject _bluePlayerPanelOrgone = null;
    public GameObject BluePlayerPanelOrgone => _bluePlayerPanelOrgone;

    [Space]
    //Liste des enfants de la jauge d'orgone du joueur rouge
    [SerializeField] private List<Image> _redPlayerCharge = new List<Image>();
    public List<Image> RedPlayerCharge => _redPlayerCharge;

    //Liste des enfants de la jauge d'orgone du joueur bleu
    [SerializeField] private List<Image> _bluePlayerCharge = new List<Image>();
    public List<Image> BluePlayerCharge => _bluePlayerCharge;


    [Header("ZONE ORGONE")]
    //Est ce qu'une jauge d'orgone est sélectionnée
    [SerializeField] private bool _selected = false;
    public bool Selected => _selected;

    //Zone d'orgone joueur rouge
    [SerializeField] private GameObject _redPlayerZone = null;
    public GameObject RedPlayerZone => _redPlayerZone;

    //Zone d'orgone joueur bleu
    [SerializeField] private GameObject _bluePlayerZone = null;
    public GameObject BluePlayerZone => _bluePlayerZone;

    public bool DoingOrgoneCharge = false;
    public bool ischarge5Blue = false;


    #endregion Variables

    private void Start(){
        GameManager.Instance.ManagerSO.GoToOrgoneJ1Phase += ActivateOrgoneArea;
        GameManager.Instance.ManagerSO.GoToOrgoneJ2Phase += ActivateOrgoneArea;
    }

    public void ReleaseZone(){
        if(GameManager.Instance.IsPlayerRedTurn){
            _redPlayerZone.GetComponent<ZoneOrgone>().ReleaseZone();
        }
        else{
            _bluePlayerZone.GetComponent<ZoneOrgone>().ReleaseZone();
        }

        _selected = false;
    }

    public void StartToMoveZone(){
        if(GameManager.Instance.IsPlayerRedTurn && !_redPlayerZone.GetComponent<ZoneOrgone>().HasMoveOrgoneArea && !_redPlayerZone.GetComponent<ZoneOrgone>().IsInValidation)
        {
            _redPlayerZone.GetComponent<ZoneOrgone>().AddOrgoneAtRange();
            _selected = true;
        }
        else if(!GameManager.Instance.IsPlayerRedTurn && !_bluePlayerZone.GetComponent<ZoneOrgone>().HasMoveOrgoneArea && !_bluePlayerZone.GetComponent<ZoneOrgone>().IsInValidation)
        {
            _bluePlayerZone.GetComponent<ZoneOrgone>().AddOrgoneAtRange();
            _selected = true;
        }
    }

    /// <summary>
    /// Permet de connaitre la nouvelle valeur de la jauge d'orgone en fonction d'un variation positif ou négatif
    /// </summary>
    public int ChangeOrgone(int currentValue, int fluctuation){
        int newValue = currentValue + fluctuation;
        return newValue;
    }

    /// <summary>
    /// Détermine et applique l'animation de l'UI de la jauge d'orgone. A DÉTERMINER !
    /// </summary>
    public void UpdateOrgoneUI()
    {
        /* Pour les animations voici ma proposition
         il faudrait faire une liste de boutons avec les boutons d'une jauge d'orgone
         il faudrait la valeur actuelle de l'orgone 
         et la variation
         on applique ensuite un for qui dépend qui s'arrete quand on a appliquer tout la fluctuation de l'orgone et 
        qui a chaque fois applique l'animation pour chaque boutons concernés
        on aurait du coup un valeur spécifique temporaire qui corresponderait à la valeur actuelle qui se baisse au fur et à mesure pour comptabiliser le nombre d'animation qui reste à faire
        
         */
    }

    public void ActivateOrgoneArea(){
        if(GameManager.Instance.IsPlayerRedTurn){
            _redPlayerZone.GetComponent<ZoneOrgone>().ActivationArea();
        }
        else{
            _bluePlayerZone.GetComponent<ZoneOrgone>().ActivationArea();
        }
    }
}

namespace MythsAndSteel.Orgone{
    public static class OrgoneCheck{

        /// <summary>
        /// Determine si le joueur peut utiliser un pouvoir d'orgone
        /// </summary>
        /// <param name="cost"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool CanUseOrgonePower(int cost, int player){
            bool canUse = false;

            if(GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2){
                Debug.Log("orgonephase");
                if(GameManager.Instance.IsPlayerRedTurn == (player == 1? true : false)){
                    Debug.Log("jsp" + player);
                    if(player == 1){
                        if(PlayerScript.Instance.RedPlayerInfos.OrgonePowerLeft > 0 && PlayerScript.Instance.RedPlayerInfos.OrgoneValue >= cost){
                            canUse = true;
                            Debug.Log("oui");
                        }
                    }
                    else{
                        if(PlayerScript.Instance.BluePlayerInfos.OrgonePowerLeft > 0 && PlayerScript.Instance.BluePlayerInfos.OrgoneValue >= cost){
                            canUse = true;
                            Debug.Log("ouioui");
                        }
                    }
                }
            }

            return canUse;
        }
    }
}
