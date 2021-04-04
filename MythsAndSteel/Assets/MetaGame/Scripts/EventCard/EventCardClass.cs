using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="Event Scriptable")]
public class EventCardClass : ScriptableObject{
    //Nombre de cartes events
    [SerializeField] private int _numberOfEventCard = 0;
    public int NumberOfEventCard => _numberOfEventCard;

    //Liste des cartes events
    [SerializeField] private List<EventCard> _eventCardList = new List<EventCard>();
    public List<EventCard> EventCardList => _eventCardList;

    [SerializeField] private float _spaceBetweenTwoEvents = 0f;

    int _redPlayerPos = 0;
    int _bluePlayerPos = 0;

    private void OnValidate(){
        int number = 0;
        foreach(EventCard card in _eventCardList){
            if(card._isEventInFinalGame){
                number++;
            }
        }

        _numberOfEventCard = number;
    }

    /// <summary>
    /// Call an event
    /// </summary>
    /// <param name="id"></param>
    public void CallEvent(int id)
    {
        switch(id)
        {
            case 0:
                PointeursLaserOptimisés();
                break;
        }
    }

    /// <summary>
    /// Lorsqu'un événement est utilisé il faut le retirer
    /// </summary>
    public void RemoveEventsAfterUse(){
        //Joueur rouge
        //Cherche si une carte event n'a pas été utilisée
        foreach(GameObject gam in PlayerScript.Instance.EventCardList._eventGamRedPlayer){
            bool hasFindEvent = false;

            foreach(MYthsAndSteel_Enum.EventCard ev in PlayerScript.Instance.EventCardList._eventCardRedPlayer){
                if(ev == gam.GetComponent<EventCardContainer>().EventCardInfo._eventType){
                    hasFindEvent = true;
                }
            }

            //Si l'événement n'a pas été trouvé
            if(!hasFindEvent){
                RemoveEventGam(gam, 1);
            }

            UpdateVisualUI(PlayerScript.Instance.EventCardList._eventGamRedPlayer, 1);
        }            
    }

    /// <summary>
    /// Met a jour la position des cartes events dans l'interface
    /// </summary>
    /// <param name="gam"></param>
    public void UpdateVisualUI(List<GameObject> gam, int player){
        if(player == 1){
            if(PlayerScript.Instance.EventCardList._eventCardRedPlayer.Count <= 3){
                ResetEventParentPos(1);

                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, false, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos);
            }
            else{
                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos);
            }
        }
        else if(player == 2)
        {
            if(PlayerScript.Instance.EventCardList._eventCardRedPlayer.Count <= 3){
                ResetEventParentPos(2);

                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, false, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos);
            }
            else{
                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos);
            }
        }
        else{
            Debug.LogError("Vous essayez d'd'update l'ui d'un joueur qui n'existe pas");
        }
    }
    
    /// <summary>
    /// Update la position des events dans la liste
    /// </summary>
    /// <param name="gam"></param>
    /// <param name="player"></param>
    void UpdateEventList(List<GameObject> gam, int player)
    {
        if(player == 1){
            if(_redPlayerPos == 0){
                //Déplace les events à leurs bonnes positions
                gam[0].transform.position = player == 1 ? new Vector3(300, 540 - _spaceBetweenTwoEvents, 0) : new Vector3(1620, 540 - _spaceBetweenTwoEvents, 0);

                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y + _spaceBetweenTwoEvents,
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
            else{
                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y + _spaceBetweenTwoEvents,
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
        }

        else if(player == 2){
            if(_bluePlayerPos == 0){
                //Déplace les events à leurs bonnes positions
                gam[0].transform.position = player == 1 ? new Vector3(300, 540 - _spaceBetweenTwoEvents, 0) : new Vector3(1620, 540 - _spaceBetweenTwoEvents, 0);

                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y + _spaceBetweenTwoEvents,
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
            else{
                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y + _spaceBetweenTwoEvents,
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Met a jour le visuel des boutons et leur interaction
    /// </summary>
    /// <param name="upButton"></param>
    /// <param name="downButton"></param>
    /// <param name="active"></param>
    void UpdateButtonPlayer(GameObject upButton, GameObject downButton, bool active, int numberOfCard, int pos){
        upButton.GetComponent<Image>().sprite = active ? pos == numberOfCard - 3? UIInstance.Instance.DesactivateButtonSprite : UIInstance.Instance.ActivateButtonSprite : UIInstance.Instance.DesactivateButtonSprite;
        upButton.GetComponent<Button>().interactable = pos == numberOfCard - 3 ? false : active;
        downButton.GetComponent<Image>().sprite = active ? pos == 0 ? UIInstance.Instance.DesactivateButtonSprite : UIInstance.Instance.ActivateButtonSprite : UIInstance.Instance.DesactivateButtonSprite;
        downButton.GetComponent<Button>().interactable = pos == 0 ? false : active;
    }

    /// <summary>
    /// détruit une carte event
    /// </summary>
    /// <param name="gam"></param>
    void RemoveEventGam(GameObject gam, int player){
        if(player == 1){
            PlayerScript.Instance.EventCardList._eventGamRedPlayer.Remove(gam);
            PlayerScript.Instance.EventCardList._eventCardRedPlayer.Remove(gam.GetComponent<EventCardContainer>().EventCardInfo._eventType);
        }
        else if(player == 2){
            PlayerScript.Instance.EventCardList._eventGamBluePlayer.Remove(gam);
            PlayerScript.Instance.EventCardList._eventCardBluePlayer.Remove(gam.GetComponent<EventCardContainer>().EventCardInfo._eventType);
        }
        else{
            Debug.LogError("Vous essayez d'enlever une carte event a un joueur qui n'existe pas");
        }
        Destroy(gam);

        UpdateVisualUI(PlayerScript.Instance.EventCardList._eventGamRedPlayer, 1);
        UpdateVisualUI(PlayerScript.Instance.EventCardList._eventGamBluePlayer, 2);
    }

    /// <summary>
    /// Quand le joueur appuie sur le bouton pour monter dans la liste des cartes events
    /// </summary>
    /// <param name="player"></param>
    public void GoUp(int player){
        if(player == 1){
            _redPlayerPos++;
            UpdateEventsParentPos(1);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos);
        }
        else if(player == 2){
            _bluePlayerPos++;
            UpdateEventsParentPos(2);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos);
        }
        else{
            Debug.LogError("vous essayez de déplacer les cartes events d'un joueur qui n'existe pas");
        }
    }

    /// <summary>
    /// Quand le joueur appuie pour descendre dans la liste des cartes events
    /// </summary>
    /// <param name="player"></param>
    public void GoDown(int player){
        if(player == 1){
            _redPlayerPos--;
            UpdateEventsParentPos(1);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos);
        }
        else if(player == 2){
            _bluePlayerPos--;
            UpdateEventsParentPos(2);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos);
        }
        else{
            Debug.LogError("vous essayez de déplacer les cartes events d'un joueur qui n'existe pas");
        }
    }

    /// <summary>
    /// Update la position du parent des cartes events d'un joueur
    /// </summary>
    void UpdateEventsParentPos(int player){
        if(player == 1){
            UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition = new Vector3(UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition.x, -_spaceBetweenTwoEvents * _redPlayerPos, UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition.z);
        }
        else if(player == 2){
            UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition = new Vector3(UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition.x, -_spaceBetweenTwoEvents * _bluePlayerPos, UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition.z);
        }
        else{
            Debug.LogError("vous essayez de déplacer les cartes events d'un joueur qui n'existe pas");
        }
    }

    /// <summary>
    /// Reset la position du parent des cartes events d'un joueur
    /// </summary>
    /// <param name="player"></param>
    void ResetEventParentPos(int player){
        if(player == 1){
            _redPlayerPos =0;
            UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition = Vector3.zero;
        }
        else if(player == 2){
            _bluePlayerPos =0;
            UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition = Vector3.zero;
        }
        else{
            Debug.LogError("vous essayez de déplacer les cartes events d'un joueur qui n'existe pas");
        }
    }

    #region Evenement
    /// <summary>
    /// Carte event du pointeur optimisé
    /// </summary>
    public void PointeursLaserOptimisés(){

    }
    #endregion Evenement









}

/// <summary>
/// Class qui regroupe toutes les variables pour une carte event
/// </summary>
[System.Serializable]
public class EventCard {
    public string _eventName = "";
    [TextArea] public string _description = "";
    public MYthsAndSteel_Enum.EventCard _eventType = MYthsAndSteel_Enum.EventCard.Activation_de_nodus;
    public int _eventCost = 0;
    public bool _isEventInFinalGame = true;
    public Sprite _eventSprite = null;
}
