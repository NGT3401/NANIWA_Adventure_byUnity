
using UnityEngine;
using System.Collections;

public class Select_button : MonoBehaviour
{

    static bool isLoading;

    // シーンの移動(引数と入して渡されたシーンを呼ぶ)
    public void SelectScene(string buttonName)
    {
        if (isLoading == false)
        {
            FadeManager.Instance.LoadLevel(buttonName, 1.5f);
            isLoading = true;
        }


    }

    void Start()
    {
        isLoading = false;
    }

    void Update()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }
        }
    }
}