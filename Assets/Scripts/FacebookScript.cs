using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic;

public class FacebookScript : MonoBehaviour {

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
    }

    public void LogIn()
    {
        List<string> perms = new List<string>() { "public_profile", "email", "user_friends" };
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

    public IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        byte[] screenshot = tex.EncodeToPNG();

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", screenshot, "Screenshot.png");
        FB.API("me/photos", HttpMethod.POST, APICallback, wwwForm);
    }

    public void PostScreenshot()
    {
        StartCoroutine(TakeScreenshot());
    }

    private void APICallback(IGraphResult result)
    {
        if (FB.IsLoggedIn)
        {
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
        if (FB.IsLoggedIn)
        {
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
}
