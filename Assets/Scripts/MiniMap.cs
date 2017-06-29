using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;


public class MiniMap : MonoBehaviour
{
    private float intervalTime = 0.0f;
    private int width = 300;
    private int height = 200;
    private double longitude;
    private double latitude;
    private int zoom = 17;
    public GameObject Querychan;
    public GameObject Search_button;
    public PlaceSearch PlaceSearch;
    public GameObject Result;
    public JSONNode json;
    public int i, flag;
    public string[] lng = new string[5];
    public string[] lat = new string[5];
    private string path;


    void Start()
    {
        Querychan = GameObject.Find("Player_Query-Chan_Sightseeing");
        GetPos();
        GetMap();
    }


    void Update()
    {
        //毎フレーム読んでると処理が重くなるので、3秒毎に更新
        intervalTime += Time.deltaTime;
        if (intervalTime >= 3.0f)
        {
            GetPos();
            GetMap();
            intervalTime = 0.0f;
        }
    }


    void GetPos()
    {
        double x = Querychan.transform.localPosition.x;  // 経度
        double z = Querychan.transform.localPosition.z;  // 緯度

        longitude = 135.497280 + (x * 0.00005557272);  // 合ってるっぽい？
        latitude = 34.664900 + (z * 0.0000440444);  // 大体合ってる
       
        //GPSで取得した緯度経度を変数に代入
        StartCoroutine(GetGPS());
    }


    void GetMap()
    {
        //マップを取得
        StartCoroutine(GetStreetViewImage(latitude, longitude, zoom));
    }

    private IEnumerator GetGPS()
    {
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }
        Input.location.Start();
        int maxWait = 120;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            print("Location: " +
                  Input.location.lastData.latitude + " " +
                  Input.location.lastData.longitude + " " +
                  Input.location.lastData.altitude + " " +
                  Input.location.lastData.horizontalAccuracy + " " +
                  Input.location.lastData.timestamp);
        }
        Input.location.Stop();
    }


    private IEnumerator GetStreetViewImage(double latitude, double longitude, double zoom)
    {
        string url = "http://maps.googleapis.com/maps/api/staticmap?center=" + latitude + "," + longitude + "&zoom=" + zoom + "&size=" + width + "x" + height + "&markers=size:mid%7Ccolor:red%7C" + latitude + "," + longitude + "&sensor=false&key=AIzaSyBtmhHP6cOdVj0s0aPUf1SLoWLiKD2Zblg";


        /*    if (flag == 0)
        {
            //現在地マーカーはここの「&markers」以下で編集可能
            url = "http://maps.googleapis.com/maps/api/staticmap?center=" + latitude + "," + longitude + "&zoom=" + zoom + "&size=" + width + "x" + height + "&markers=size:mid%7Ccolor:red%7C" + latitude + "," + longitude + "&sensor=false&key=AIzaSyBtmhHP6cOdVj0s0aPUf1SLoWLiKD2Zblg";
        }*/
        if (flag == 1)
        {
            path = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + latitude + "," + longitude + "&radius=50&language=ja&key=AIzaSyAp9ITm3OIvgFqrL0AEDYGYvcQG-8FmbKM";
            using (WWW www2 = new WWW(path))
            {
                yield return www2;

                if (!string.IsNullOrEmpty(www2.error))
                {
                    Debug.LogError("www Error:" + www2.error);
                    yield break;
                }

                // JSONNodeに格納
                json = JSONNode.Parse(www2.text);
                Debug.Log(www2.text);
            }
            // resultsは配列。パーサー。五ヶ所のデータを返す。
            for (int i = 0; i < 5; i++)
            {
                lng[i] = json["results"][i]["geometry"]["location"]["lng"];
                lat[i] = json["results"][i]["geometry"]["location"]["lat"];
                Debug.Log(lng[i]);
                Debug.Log(lat[i]);
            }
            flag = 0;
        }

        //現在地マーカーはここの「&markers」以下で編集可能
        url = "http://maps.googleapis.com/maps/api/staticmap?center=" + latitude + "," + longitude + "&zoom=" + zoom + "&size=" + width + "x" + height + "&markers=size:mid%7Ccolor:red%7C" + latitude + "," + longitude + "&markers=size:mid%7Ccolor:green%7Clabel:1%7C" + lat[0] + "," + lng[0] + "&markers=size:mid%7Ccolor:green%7Clabel:2%7C" + lat[1] + "," + lng[1] + "&markers=size:mid%7Ccolor:green%7Clabel:3%7C" + lat[2] + "," + lng[2] + "&markers=size:mid%7Ccolor:green%7Clabel:4%7C" + lat[3] + "," + lng[3] + "&markers=size:mid%7Ccolor:green%7Clabel:5%7C" + lat[4] + "," + lng[4] + "&sensor=false&key=AIzaSyBtmhHP6cOdVj0s0aPUf1SLoWLiKD2Zblg";

        //}
        WWW www = new WWW(url);
        yield return www;

        //マップの画像をTextureとして貼り付ける
        RawImage rawImage = GetComponent<RawImage>();
        rawImage.texture = www.textureNonReadable;

    }


    public void Switch()
    {
        flag = 1;
    }

}