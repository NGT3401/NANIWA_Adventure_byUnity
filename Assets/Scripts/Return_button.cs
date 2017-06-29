using UnityEngine;
using System.Collections;

public class Return_button : MonoBehaviour {

    public GameObject Querychan;
    public GameObject Osaka_Field;
    public GameObject Control;
    public ChangeSkyBox ChangeSkyBox;
    public GameObject View_Button;
    public GameObject TextView;
    //public 対象スクリプト名 対象スクリプト名;

    void Start()
    {
        ChangeSkyBox = Control.GetComponent<ChangeSkyBox>();
        //対象スクリプト名 = 対象オブジェクト名.GetComponent<対象スクリプト名>();
    }
    
    /// ボタンをクリックした時の処理
    public void OnClick()
    {
        Control.GetComponent<SkyBoxStreetView>().enabled = false;
        Control.GetComponent<ChangeSkyBox>().enabled = true;
        ChangeSkyBox.SetSkybox();
        //対象スクリプト名.対象関数名();
        Querychan.SetActive(true);
        Osaka_Field.SetActive(true);
        //TextView.transform.localPosition = new Vector3(600, 0, 0);
        View_Button.transform.localPosition = new Vector3(-340, 250, 0);
    }
}
