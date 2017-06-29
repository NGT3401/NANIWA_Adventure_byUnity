using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {

    public GameObject textGameOver;
    public GameObject textToTitle;
    public GameObject textRestart;

    private bool changeScene = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

       /* if (changeScene == true)
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
                FadeManager.Instance.LoadLevel("game", 1.5f);*/
        

    }

    public void ActGameOver()
    {
        StartCoroutine(OverDirection());
    }

    private IEnumerator OverDirection()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Ghost");
        GetComponent<AudioSource>().Play();
        for (int i = 0; i < go.Length; i++)
        {
            Destroy(go[i].GetComponent<Ghost>());
            go[i].GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        }
        GameObject.Find("FreeLookCameraRig").GetComponent<ShakeCamera>().ShakingCamera(1.5f, 1.5f);
        yield return new WaitForSeconds(2);
        iTween.ScaleTo(textGameOver, iTween.Hash("x", 1, "y", 1, "time", 1.0f));
        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1.0f, "time", 1.0f, "onupdate", "FadeIn"));
        yield return new WaitForSeconds(2);
        textToTitle.SetActive(true);
        textRestart.SetActive(true);
        changeScene = true;
    }

    private void FadeIn(float alpha)
    {
        if (textGameOver.activeSelf == false)
            textGameOver.SetActive(true);

        Color tc = textGameOver.GetComponent<Text>().color;
        tc.a = alpha;
        textGameOver.GetComponent<Text>().color = tc;

    }
}
