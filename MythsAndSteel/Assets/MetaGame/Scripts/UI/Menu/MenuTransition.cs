using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
Ce Script permet d'afficher ou d'enlever les Options en appuyant sur Echap. 
Il fait apparaitre une autre scène qui va se superposer à la scène principal.
 */

[CreateAssetMenu(menuName = "META/Transition Menu")]
public class MenuTransition : ScriptableObject
{
    /// <summary>
    /// Active une scène
    /// </summary>
    /// <param name="sceneId"></param>
    public void LoadScene(int sceneId)
    {
       SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Désactive une scène
    /// </summary>
    /// <param name="sceneId"></param>
    public void DesactivMenu(int sceneId)
    { 
        SceneManager.UnloadSceneAsync(sceneId);
    }
}
