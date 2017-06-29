using UnityEngine;
using System.Collections;


public class SkyBoxStreetView : MonoBehaviour
{
    // Skyboxに貼り付けるテクスチャ
    public Texture2D[] textures;
    // GoogleStreetViewで使用する値の宣言
    // カメラの向き
    public double heading = 0.0;
    public double pitch = 0.0;
    // 取得する画像のサイズ(640が最大)
    private int width = 640;
    private int height = 640;
    // 経度・緯度(GoogleMapで確認できる)
    private double longitude = 0.0;
    private double latitude = 0.0;
    // 視野(最大120)
    public double fov;
    // 前後左右上下を設定しておく変数
    private int skyNum;
    public GameObject Querychan;



    public void start()
    {
        Querychan = GameObject.Find("Player_Query-Chan");
    }


    // メイン部分
    public void UpdateSkybox()
    {
        textures = new Texture2D[6];
        double x = Querychan.transform.localPosition.x;  // 経度
        double z = Querychan.transform.localPosition.z;  // 緯度

        longitude = 135.497280 + (x*0.00005557272);  // 合ってるっぽい？
        latitude = 34.664900 + (z* 0.0000440444);  // 大体合ってる

        // 前後左右上下をそれぞれ取得
        for (skyNum = 0; skyNum < 6; skyNum++)
        {
            fov = 90;
            switch (skyNum)
            {
                // front
                case 0:
                    heading = 0; pitch = 0;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
                // back
                case 1:
                    heading = 180; pitch = 0;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
                // left
                case 2:
                    heading = 90; pitch = 0;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
                // right
                case 3:
                    heading = 270; pitch = 0;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
                // up
                case 4:
                    heading = 0; pitch = 90;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
                // down
                case 5:
                    heading = 0; pitch = -90;
                    StartCoroutine(GetStreetViewImage(latitude, longitude, heading, pitch, fov, skyNum));
                    break;
            }
        }
    }



    // Google Map Street Viewの画像を取ってくる
    private IEnumerator GetStreetViewImage(double latitude, double longitude, double heading, double pitch, double fov, int skyNum)
    {
        string url = "http://maps.googleapis.com/maps/api/streetview?" + "size=" + width + "x" + height + "&location=" + latitude + "," + longitude + "&heading=" + heading + "&pitch=" + pitch + "&fov=" + fov + "&key=AIzaSyDVr9gn39t5SuM0PiJ2jliv3eGaPhvh_dM";
        Debug.Log(skyNum + " : " + url);
        WWW www = new WWW(url);
        yield return www;

        // texturesに取得してテクスチャをセット
        textures[skyNum] = www.texture;

        // ちゃんとテクスチャを取得できたらskyboxを作る
        CreateSkyBox();
    }


    void CreateSkyBox()
    {
        // Manifestを作成
        SkyboxManifest manifest = new SkyboxManifest(textures[0], textures[1], textures[2], textures[3], textures[4], textures[5]);

        // 作成したManifestからマテリアルを作成
        Material material = CreateSkyboxMaterial(manifest); SetSkybox(material); enabled = false;
    }


    public static Material CreateSkyboxMaterial(SkyboxManifest manifest)
    {
        // Skybox形式のマテリアルを作成してテクスチャをセット
        Material result = new Material(Shader.Find("RenderFX/Skybox"));
        result.SetTexture("_FrontTex", manifest.textures[0]);
        result.SetTexture("_BackTex", manifest.textures[1]);
        result.SetTexture("_LeftTex", manifest.textures[2]);
        result.SetTexture("_RightTex", manifest.textures[3]);
        result.SetTexture("_UpTex", manifest.textures[4]);
        result.SetTexture("_DownTex", manifest.textures[5]);

        return result;
    }


    void SetSkybox(Material material)
    {
        // メインカメラを取得して
        GameObject camera = Camera.main.gameObject;
        // Skyboxを取得
        Skybox skybox = camera.GetComponent<Skybox>();
        // Skyboxがなければ作成
        if (skybox == null)
        {
            skybox = camera.AddComponent<Skybox>();
        }
        // Skyboxにマテリアルをセット
        skybox.material = material;
    }
}


// Skyboxマニフェストの構造体
public struct SkyboxManifest
{
    public Texture2D[] textures;
    public SkyboxManifest(Texture2D front, Texture2D back, Texture2D left, Texture2D right, Texture2D up, Texture2D down)
    {
        textures = new Texture2D[6]
        {
            front, back, left, right, up, down
        };
    }


}