using UnityEngine;
using System.Collections;

public class SkyboxChanger : MonoBehaviour {

    // Skyboxのマテリアル
    public Material skybox;

    void Start()
    {
        
    }

    public void ChangeSkybox()
    {
        // Skyboxを変更する
        RenderSettings.skybox = skybox;
    }
}
