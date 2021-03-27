using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UIManager")]
public class UIManager : ScriptableObject
{
    [Header("Cartes événements")]
    [SerializeField] EventCardClass _eventCardClass = null;

    /// <summary>
    /// Appelle la fonction dans le GameManager pour passer à la phase suivante
    /// </summary>
    public void ClicToNextPhase(){
        GameManager.Instance.CallEventToSwitchPhase();
    }

    /// <summary>
    /// Call an event
    /// </summary>
    /// <param name="id"></param>
    public void CallEvent(int id){
        switch(id){
            case 0:
                _eventCardClass.PointeursLaserOptimisés();
                break;
        }
    }

    /// <summary>
    /// get a random event card and add it to the player
    /// </summary>
    /// <param name="PlayerId"></param>
    public void AddRandomEvent(int PlayerId){
        int randomEvent = Random.Range(0, _eventCardClass.NumberCarteEvent + 1);
    }
}