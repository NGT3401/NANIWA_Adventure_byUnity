using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SourceManager : MonoBehaviour
{

    public int sourceCount;
    public GameObject source;
    public UnityEngine.UI.Text drawSourceCount;

    public int sourceSetting = 8;
    private List<Transform> sourcePositionList = new List<Transform>();

    // Use this for initialization
    void Start()
    {

        sourceCount = sourceSetting;

        //子オブジェクト全取得
        sourcePositionList.AddRange(GetComponentsInChildren<Transform>());
        sourcePositionList.RemoveAt(0);

        //sourceSettingの数だけSourceをランダム配置
        for (int i = 0; i < sourceSetting; i++)
        {
            if (sourcePositionList.Count > 0)
            {
                int num = Random.Range(0, sourcePositionList.Count);
                GameObject.Instantiate(source, sourcePositionList[num].position, new Quaternion(0, 0, 0, 0));
                sourcePositionList.RemoveAt(num);
            }

        }

        //リストクリア
        sourcePositionList.Clear();

        //子オブジェクト全削除
        foreach (Transform n in transform)
        {
            GameObject.Destroy(n.gameObject);
        }



    }

    // Update is called once per frame
    void Update()
    {

        drawSourceCount.text = "残り：" + sourceCount;

    }
}
