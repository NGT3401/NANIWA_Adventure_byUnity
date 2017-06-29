using UnityEngine;
using System.Collections;

public class Source : MonoBehaviour {

    public GameObject sourceManager;

	// Use this for initialization
	void Start () {

        sourceManager = GameObject.Find("SourceManager");

        StartCoroutine("Rotate");
        //StartCoroutine("Lighting");
        

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator Rotate()
    {
        while (true)
        {
            iTween.RotateAdd(gameObject, iTween.Hash("y", 360, "time", 6.0f, "easetype", iTween.EaseType.linear));
            yield return null;
        }
    }

    private IEnumerator Lighting()
    {
        while (true)
        {
            iTween.RotateBy(gameObject, iTween.Hash("y", 360, "time", 4.0f));
            yield return new WaitForSeconds(4.0f);
        }
    }

   

}
