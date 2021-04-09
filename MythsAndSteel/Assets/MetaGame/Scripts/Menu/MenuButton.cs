using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuButton : MonoBehaviour
{
    
    [SerializeField] GameObject CanvasToLoad; //Le Canvas que l'on veuet monter 
    [SerializeField] GameObject CanvasParent;//Le Canvas dans lequel se trouve l'objet, uqi sera désafficher
    [SerializeField] bool CanReturn;

    


    

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCanvas()
    {
        CanvasToLoad.SetActive(true);
        CanvasParent.SetActive(false);
    }

    public void ReturnsCanvas()
    {
        CanvasParent.SetActive(false);
        CanvasToLoad.SetActive(true);
    }

    private void Update()
    {
        if (CanReturn & Input.GetKey(KeyCode.Escape))
        {
            ReturnsCanvas();
        }
    }

}
    