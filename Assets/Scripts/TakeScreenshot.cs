using UnityEngine;
using System.Collections;

public class TakeScreenshot : MonoBehaviour {

    public GameObject menu;
    string path;

    void Start()
    {
        path = Application.persistentDataPath;
    }
    public void ScreenShot()
    {
        menu.SetActive(false);
        //take screenshot
        Application.CaptureScreenshot(path + "/" + "Screenshot.png");
		Debug.Log ("PATH " + path);
        menu.SetActive(true);
    }
}
