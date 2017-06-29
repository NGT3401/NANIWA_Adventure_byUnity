using UnityEngine;
using System.Collections;

public class ChangeSkyBox : MonoBehaviour
{
    // Skyboxのマテリアル
    public Material sky;


    public void SetSkybox()
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
        skybox.material = sky;
    }

}