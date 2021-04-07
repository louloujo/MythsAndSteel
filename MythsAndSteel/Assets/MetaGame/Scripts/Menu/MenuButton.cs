using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] int SceneToLoad;
    [SerializeField] GameObject CanvasToLoad;
    [SerializeField] GameObject CanvasParent;
    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCanvas()
    {
        CanvasToLoad.SetActive(true);
        CanvasParent.SetActive(false);
        
    }

    
}
    