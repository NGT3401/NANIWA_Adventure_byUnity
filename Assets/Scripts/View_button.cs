using UnityEngine;
using System.Collections;

public class View_button : MonoBehaviour
{
    public GameObject Querychan;
    public GameObject Osaka_Field;
    public GameObject Control;
    public GameObject TextView;
    public SkyBoxStreetView SkyBoxStreetView;
    //public 対象スクリプト名 対象スクリプト名;
    
    void Start()
    {
        SkyBoxStreetView = Control.GetComponent<SkyBoxStreetView>();
        //対象スクリプト名 = 対象オブジェクト名.GetComponent<対象スクリプト名>();
    }

    /// ボタンをクリックした時の処理
    public void OnClick()
    {
        Control.GetComponent<SkyBoxStreetView>().enabled = true;
        SkyBoxStreetView.UpdateSkybox();
        //対象スクリプト名.対象関数名();
        Control.GetComponent<ChangeSkyBox>().enabled = false;
        Querychan.SetActive(false);
        Osaka_Field.SetActive(false);
        //TextView.transform.position = new Vector3(600, 0, 0);
        this.gameObject.transform.localPosition = new Vector3(600, 0, 0);
    }

}