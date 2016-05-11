using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using System.Collections.Generic;

public class UI : MonoBehaviour {
    //Different menu pages
    public GameObject main;
    public GameObject howTo;
    public GameObject options;
	
    //Methods for changing scenes
    public void StartB()
    {
        SceneManager.LoadScene(1);
        SceneManager.UnloadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    //Methods for opening/closing new menus
    public void Main()
    {
        main.SetActive(true);
        howTo.SetActive(false);
        options.SetActive(false);
    }

    public void HowTo()
    {
        main.SetActive(false);
        howTo.SetActive(true);
        options.SetActive(false);
    }

    public void Options()
    {
        main.SetActive(false);
        howTo.SetActive(false);
        options.SetActive(true);
    }
}
