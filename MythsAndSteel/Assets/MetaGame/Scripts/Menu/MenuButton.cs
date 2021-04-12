using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuButton : MonoBehaviour
{
    
    [SerializeField] GameObject CanvasToLoad; //Le Canvas que l'on veuet monter 
    [SerializeField] GameObject CanvasParent;//Le Canvas dans lequel se trouve l'objet, qui sera désafficher
    [SerializeField] bool CanReturn;//Est ce que on peut revenir au Canvas précédent?

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCanvas() //permet de charger un Canvas
    {
        CanvasToLoad.SetActive(true);
        CanvasParent.SetActive(false);
    }

    public void ReturnsCanvas()//permet de revenir au Canvas précédent
    {
        CanvasParent.SetActive(false);
        CanvasToLoad.SetActive(true);
    }

    private void Update()
    {
        if (CanReturn & Input.GetKey(KeyCode.Escape))// Retour arrière avec la touche (échappe si possible)
        {
            ReturnsCanvas();
        }
    }

}
    