using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameClear : MonoBehaviour {

    public GameObject textGameClear;
    public GameObject textToTitle;
    public GameObject textRestart;

    private bool changeScene = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        /*if (changeScene == true)
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
                FadeManager.Instance.LoadLevel("game", 2);*/

	}

    public void ActGameClear()
    {
        StartCoroutine(ClearDirection());
    }

    private IEnumerator ClearDirection()
    {
        GetComponent<WhiteOut>().ActWhiteOut(2);
        yield return new WaitForSeconds(2);
        GetComponent<SkyboxChanger>().ChangeSkybox();
        GameObject[] go = GameObject.FindGameObjectsWithTag("Ghost");
        for (int i = 0; i < go.Length; i++)
        {
            Destroy(go[i]);
        }
        yield return new WaitForSeconds(2);
        iTween.ScaleTo(textGameClear, iTween.Hash("x", 1, "y", 1, "time", 1.0f));
        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1.0f, "time", 1.0f, "onupdate", "FadeIn"));
        yield return new WaitForSeconds(1);
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        textToTitle.SetActive(true);
        textRestart.SetActive(true);
        changeScene = true;
    }

    private void FadeIn(float alpha)
    {
        if (textGameClear.activeSelf == false)
            textGameClear.SetActive(true);

        Color tc = textGameClear.GetComponent<Text>().color;
        tc.a = alpha;
        textGameClear.GetComponent<Text>().color = tc;

    }

}
