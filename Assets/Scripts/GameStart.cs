using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

    public GameObject textGameStart;

	// Use this for initialization
	void Start () {

        StartCoroutine("TextAnimation");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator TextAnimation()
    {
        textGameStart.SetActive(true);
        GetComponent<AudioSource>().Play();
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 800, "time", 0, "onupdate", "ChangePosX", "easeType", iTween.EaseType.linear));
        yield return new WaitForSeconds(0.1f);
        iTween.ValueTo(gameObject, iTween.Hash("from", 800, "to", 50, "time", 0.3f, "onupdate", "ChangePosX", "easeType", iTween.EaseType.linear));
        yield return new WaitForSeconds(0.3f);
        iTween.ValueTo(gameObject, iTween.Hash("from", 50, "to", -50, "time", 2.0f, "onupdate", "ChangePosX", "easeType", iTween.EaseType.linear));
        yield return new WaitForSeconds(2f);
        iTween.ValueTo(gameObject, iTween.Hash("from", -50, "to", -800, "time", 0.3f, "onupdate", "ChangePosX", "easeType", iTween.EaseType.linear));
        yield return new WaitForSeconds(0.3f);
        textGameStart.SetActive(false);
        yield return null;
    }

    private void ChangePosX(float x){

        Vector2 text = textGameStart.GetComponent<RectTransform>().anchoredPosition;
        text.x = x;
        textGameStart.GetComponent<RectTransform>().anchoredPosition = text;

    }

    private void MoveInOut(GameObject text, float iniPos, float time)
    {
        text.SetActive(true);
        iTween.MoveTo(text, iTween.Hash("x", iniPos));
        //iTween.MoveTo(text, iTween.Hash("x", 200, "time", 0.5));
        //iTween.MoveTo(text, iTween.Hash("x", -200, "time", time, "delay", 0.5));
        //iTween.MoveTo(text, iTween.Hash("x", -1*iniPos, "time", 0.5, "delay", 0.5+time));

    }

    /*public void SetGameover()
    {
        
        iTween.FadeTo(plane, iTween.Hash("alpha", 0f, "time", 1.0f, "oncomplete", "SetPlaneFalse", "oncompletetarget", gameObject));
        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1.0f, "time", 2.0f, "delay", 2.5f, "onupdate", "FadeIn", "oncomplete", "ActiveToTitle"));

    }

    private void SetPlaneFalse()
    {
        plane.SetActive(false);
        //print("false complete");
    }

    private void FadeIn(float alpha)
    {
        if (textGameover.gameObject.activeSelf == false)
            textGameover.gameObject.SetActive(true);

        Color tc = textGameover.GetComponent<Text>().color;
        tc.a = alpha;
        textGameover.GetComponent<Text>().color = tc;

    }

    private void ActiveToTitle()
    {
        textToTitle.gameObject.SetActive(true);
        isToTitle = true;
    }

    public bool getIsGameover()
    {
        return isGameover;
    }*/
}
