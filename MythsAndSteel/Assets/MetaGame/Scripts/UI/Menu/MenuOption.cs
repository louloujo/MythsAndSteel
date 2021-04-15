using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
Ce Script permet d'afficher ou d'enlever les Options en appuyant sur Echap. 
Il fait apparaitre une autre scène qui va se superposer à la scène principal.
 */

[CreateAssetMenu(menuName = "META/Option menu")]
public class MenuOption : ScriptableObject
{
    public void ActiveMenu()
    {
       SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    public void DesactivMenu()
    { 
        SceneManager.UnloadSceneAsync(1);
    }
}
