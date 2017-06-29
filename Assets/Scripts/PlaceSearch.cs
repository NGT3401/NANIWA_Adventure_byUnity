using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlaceSearch : MonoBehaviour
{

    private string path;
    public JSONNode json;
    public Text result;
    public GameObject Querychan;
    // 経度・緯度(GoogleMapで確認できる)
    private double longitude = 0.0;
    private double latitude = 0.0;
    private float intervalTime = 0.0f;

    /*void Update()
    {
        //毎フレーム読んでると処理が重くなるので、3秒毎に更新
        intervalTime += Time.deltaTime;
        if (intervalTime >= 3.0f)
        {
            Search();
            intervalTime = 0.0f;
        }
    }
    */


    public void Search()
    {
        StartCoroutine(Json());
    }

    private IEnumerator Json() {
        double x = Querychan.transform.localPosition.x;  // 経度
        double z = Querychan.transform.localPosition.z;  // 緯度
        Debug.Log("x:" + x);
        Debug.Log("z:" + z);
        longitude = 135.497280 + (x * 0.00005557272);  // 合ってるっぽい？
        latitude = 34.664900 + (z * 0.0000440444);  // 大体合ってる
        // googleプレイス検索を利用。半径50mの周辺データを検索
        path = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + latitude + "," + longitude + "&radius=50&language=ja&key=AIzaSyAp9ITm3OIvgFqrL0AEDYGYvcQG-8FmbKM";

        result.text = "\n";

        using (WWW www = new WWW(path))
        {
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("www Error:" + www.error);
                yield break;
            }

            // JSONNodeに格納
            json = JSONNode.Parse(www.text);
            // resultsは配列。パーサー。五ヶ所のデータを返す。
            for (int i = 0; i < 5; i++)
            {
                result.text += (i+1) + ". "+ json["results"][i]["name"] + "\n" + "   " + json["results"][i]["types"][0] + "\n" + "\n";
            }

        }
    }

}
