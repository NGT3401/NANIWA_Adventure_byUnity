using UnityEngine;
using System.Collections;

public class TitleSceneManager : MonoBehaviour
{

    public GameObject canvas;

    private int changeText;


    // Use this for initialization
    void Start()
    {

        changeText = 0;
        //Application.targetFrameRate = 60;
        //BgmManager.Instance.Play("パステルハウス");

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) && changeText == 0)
        {
            canvas.transform.FindChild("Page1").gameObject.SetActive(false);
            canvas.transform.FindChild("Page2").gameObject.SetActive(true);
            changeText = 1;


        }
        else if (Input.GetKeyDown(KeyCode.Return) && changeText == 1)
        {
            canvas.transform.FindChild("Page2").gameObject.SetActive(false);
            canvas.transform.FindChild("Page3").gameObject.SetActive(true);
            changeText = 2;


        }
        else if (Input.GetKeyDown(KeyCode.Return) && changeText == 2)
        {
            FadeManager.Instance.LoadLevel("game", 2.0f);
            changeText = 3;


        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }
}
