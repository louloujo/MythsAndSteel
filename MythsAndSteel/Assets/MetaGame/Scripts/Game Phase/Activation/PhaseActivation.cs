using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyButtons;

/*
Ce Script gère la phase d'activation : le choix de la carte d'activation, la confirmation du choix pour chaque joueur,
ainsi que la comparaison entre les deux cartes choisies pour savoir quelle joueur a l'initiative et combien d'unité chaque joueur peut activer.
Ce script renvoie comme principals informations :
- une bool IsPlayer1Starting pour savoir qui commence
- une float J1DernièreValeurActivation et une float J2DernièreValeurActivation pour savoir le montant d'unité activable pendant le tour
- Une list de CarteActivation CarteActivationUtilisée pour savoir quelle carte activation ont était utilisée depuis le début de la partie
*/

public class PhaseActivation : MonoBehaviour{
    //Variables pour le joueur avec les cartes bleu
    //Carte du joueur 1
    [SerializeField] List<CarteActivation> BlueCartesActivation = new List<CarteActivation>();
    
    //Panel qui contient les cartes Activation Bleu
    [SerializeField] private GameObject J1Panel;

    bool J1Choix = true;
    bool J1Verif = false;
    CarteActivation J1CarteVerif = null;
    List<CarteActivation> J1CartesNonVerif = new List<CarteActivation>();
    private float J1DernièreValeurActivation;
    
    private bool J1CarteChoisie = false;

    //Variables pour le joueur 2
    //Carte du joueur 2
    [SerializeField] private List<CarteActivation> RedCartesActivation = new List<CarteActivation>();

    //Panel qui contient les cartes Activation Rouge
    [SerializeField] private GameObject J2Panel;

    bool J2Choix = true;
    bool J2Verif = false;
    CarteActivation J2CarteVerif = null;
    List<CarteActivation> J2CartesNonVerif = new List<CarteActivation>();
    private float J2DernièreValeurActivation;

    private bool J2CarteChoisie = false;
    
   //Variables communes
    private List<CarteActivation> CarteActivationUtilisée = new List<CarteActivation>();

    [SerializeField] private GameObject _result = null;



    private void Start(){
        UIInstance.Instance.UiManager.ActivateActivationPhase(false);
        _result.SetActive(false);
    }


    void Update(){
        if(GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.Activation){
            //Joueur 1
            if(!J1CarteChoisie){
                //On vérifie si le joueur 1 choisit sa carte
                if(J1Verif && Input.GetKeyDown(J1CarteVerif.inputCarteActivation)){
                    foreach(CarteActivation Carteactivations in BlueCartesActivation){
                        J1Panel.transform.GetChild(Carteactivations.IndexCarteActivation).GetComponent<Image>().color = new Color(0f, 0f, 1f, 1f);
                    }

                    J1DernièreValeurActivation = float.Parse(J1CarteVerif.valeurActivation);
                    CarteActivationUtilisée.Add(J1CarteVerif);
                    BlueCartesActivation.Clear();

                    foreach(CarteActivation carteactivations in J1CartesNonVerif){
                        BlueCartesActivation.Add(carteactivations);
                    }

                    //Le joueur 1 a choisit sa carte
                    J1CarteChoisie = true;
                    if(CheckIfBothPlayerHasChoose()) ShowResult();

                    J1Verif = false;

                }

                else if(J1Verif){
                    foreach(CarteActivation Carteactivation in J1CartesNonVerif){
                        if(Input.GetKeyDown(Carteactivation.inputCarteActivation)){
                            foreach(CarteActivation Carteactivations in BlueCartesActivation){
                                J1Panel.transform.GetChild(Carteactivations.IndexCarteActivation).GetComponent<Image>().color = new Color(0f, 0f, 1f, 1f);
                            }
                            J1Choix = true;
                            J1Verif = false;
                        }
                    }
                }

                //Première partie du code qui s'execute, on regarde si le joueur appuie sur une touche,
                //et si c'est le cas on sauvegarde la carte qui correspond à la touche pressée et les autres dans deux list différentes.
                //On passe alors à la phase de vérification
                else if(J1Choix){
                    J1CarteVerif = null;
                    J1CartesNonVerif.Clear();

                    foreach(CarteActivation carteactivation in BlueCartesActivation){
                        if(Input.GetKeyDown(carteactivation.inputCarteActivation)){
                            foreach(CarteActivation carteactivations in BlueCartesActivation){
                                Image image = J1Panel.transform.GetChild(carteactivations.IndexCarteActivation).GetComponent<Image>();
                                image.color = new Color(0f, 1f, 0f, 1f);
                                J1CarteVerif = carteactivation;
                                if(carteactivations == J1CarteVerif){ }
                                else if(carteactivations != J1CarteVerif){
                                    J1CartesNonVerif.Add(carteactivations);
                                }
                            }
                            J1Verif = true;
                            J1Choix = false;
                        }
                    }
                }
            }

            //Meme principe que pour le joueur 1 mais avec les variables que le joueur 2
            if(!J2CarteChoisie){
                if(J2Verif && Input.GetKeyDown(J2CarteVerif.inputCarteActivation)){
                    foreach(CarteActivation Carteactivations in RedCartesActivation){
                        J2Panel.transform.GetChild(Carteactivations.IndexCarteActivation).GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
                    }

                    J2DernièreValeurActivation = float.Parse(J2CarteVerif.valeurActivation);
                    CarteActivationUtilisée.Add(J2CarteVerif);
                    RedCartesActivation.Clear();

                    foreach(CarteActivation carteactivations in J2CartesNonVerif){
                        RedCartesActivation.Add(carteactivations);
                    }

                    //Le joueur 2 a choisit sa carte
                    J2CarteChoisie = true;
                    if(CheckIfBothPlayerHasChoose()) ShowResult();

                    J2Verif = false;
                }
                else if(J2Verif){

                    foreach(CarteActivation Carteactivation in J2CartesNonVerif){
                        if(Input.GetKeyDown(Carteactivation.inputCarteActivation)){
                            foreach(CarteActivation Carteactivations in RedCartesActivation){
                                J2Panel.transform.GetChild(Carteactivations.IndexCarteActivation).GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
                            }
                            J2Choix = true;
                            J2Verif = false;
                        }
                    }
                }

                else if(J2Choix){

                    J2CarteVerif = null;
                    J2CartesNonVerif.Clear();
                    foreach(CarteActivation carteactivation in RedCartesActivation){
                        if(Input.GetKeyDown(carteactivation.inputCarteActivation)){
                            foreach(CarteActivation carteactivations in RedCartesActivation){
                                Image image = J2Panel.transform.GetChild(carteactivations.IndexCarteActivation).GetComponent<Image>();
                                image.color = new Color(0f, 1f, 0f, 1f);
                                J2CarteVerif = carteactivation;
                                if(carteactivations == J2CarteVerif){}
                                else if(carteactivations != J2CarteVerif){
                                    J2CartesNonVerif.Add(carteactivations);
                                }
                            }

                            J2Verif = true;
                            J2Choix = false;
                        }
                    }
                }

            }
        }
    }

    /// <summary>
    /// Reset la phase d'activation
    /// </summary>
    public void ResetPhaseActivation(){
        //Variables pour le joueur 1
        J1CartesNonVerif.Clear();
        J1CarteVerif = null;
        J1Panel.SetActive(true);
        J1Choix = true;
        J1Verif = false;

        J1DernièreValeurActivation = 0f;
        J1CarteChoisie = false;

        //Variables pour le joueur 2
        J2CarteVerif = null;
        J2CartesNonVerif.Clear();
        J2Panel.SetActive(true);
        J2Choix = true;
        J2Verif = false;

        J2DernièreValeurActivation = 0f;
        J2CarteChoisie = false;

        _result.SetActive(false);
    }

    /// <summary>
    /// Detecte si les deux joueurs ont choisit leur carte activation
    /// </summary>
    /// <returns></returns>
    public bool CheckIfBothPlayerHasChoose(){
        bool bothHasPlay = false;
        bothHasPlay = J1CarteChoisie && J2CarteChoisie ? true : false;
        return bothHasPlay;
    }

    /// <summary>
    /// Lorsque les joueurs vont cliquer sur le bouton pour aller à la phase suivante
    /// </summary>
    public void ShowResult(){
        float InitiativeValeur = J1DernièreValeurActivation - J2DernièreValeurActivation;
        if(InitiativeValeur < 0){
            GameManager.Instance.SetPlayerStart(false);
            J1Panel.transform.GetChild(J1CarteVerif.IndexCarteActivation).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            J2Panel.transform.GetChild(J2CarteVerif.IndexCarteActivation).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            _result.SetActive(true);
        }

        if(InitiativeValeur > 0){
            GameManager.Instance.SetPlayerStart(true);
            J1Panel.transform.GetChild(J1CarteVerif.IndexCarteActivation).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            J2Panel.transform.GetChild(J2CarteVerif.IndexCarteActivation).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            _result.SetActive(true);
        }

        J1Panel.SetActive(false);
        J2Panel.SetActive(false);
    }

    /// <summary>
    /// Passe à la phase suivante
    /// </summary>
    public void SwitchPhase(){
        UIInstance.Instance.UiManager.ActivateActivationPhase(false);
        GameManager.Instance.NextPhase();
    }
}
    

        
    
        


