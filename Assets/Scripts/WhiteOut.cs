using UnityEngine;
using System.Collections;

public class WhiteOut : MonoBehaviour {
     /// <summary>暗転用黒テクスチャ</summary>
 private Texture2D whiteTexture;
 /// <summary>フェード中の透明度</summary>
 private float fadeAlpha = 0;
 /// <summary>フェード中かどうか</summary>
 private bool isFading = false;
  
 public void Awake ()
 { 
 
  //ここで白テクスチャ作る
  this.whiteTexture = new Texture2D (32, 32, TextureFormat.RGB24, false);
  this.whiteTexture.SetPixel(0, 0, Color.white);
  //this.whiteTexture.ReadPixels (new Rect (0, 0, 32, 32), 0, 0, false);
  this.whiteTexture.Apply ();
 }
   
 public void OnGUI ()
 {
  if (!this.isFading)
   return;
 
  //透明度を更新して白テクスチャを描画
  GUI.color = new Color (255, 255, 255, this.fadeAlpha);
  GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), this.whiteTexture);
 }
 
 /// <summary>
 /// 画面遷移
 /// </summary>
 /// <param name='scene'>シーン名</param>
 /// <param name='interval'>暗転にかかる時間(秒)</param>
 public void ActWhiteOut(float interval)
 {
  StartCoroutine (FadeWhiteOut (interval));
 }
  
  
 /// <summary>
 /// シーン遷移用コルーチン
 /// </summary>
 /// <param name='scene'>シーン名</param>
 /// <param name='interval'>暗転にかかる時間(秒)</param>
 private IEnumerator FadeWhiteOut (float interval)
 {
  //だんだん暗く
  this.isFading = true;
  float time = 0;
  while (time <= interval) {
   this.fadeAlpha = Mathf.Lerp (0f, 1f, time / interval);      
   time += Time.deltaTime;
   yield return 0;
  }
   
  //だんだん明るく
  time = 0;
  while (time <= interval) {
   this.fadeAlpha = Mathf.Lerp (1f, 0f, time / interval);
   time += Time.deltaTime;
   yield return 0;
  }
 
  this.isFading = false;
 }
 

}
