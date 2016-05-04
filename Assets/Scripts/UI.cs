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
    List<string> perms;

    //Facebook initialization
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            Debug.Log("Facebook initializing...");
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }

        perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            Debug.Log("Setting Facebook to active app...");
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    //Login
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn){
            AccessToken aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            Debug.Log(aToken.UserId);
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

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
