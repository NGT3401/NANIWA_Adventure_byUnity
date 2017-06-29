using UnityEngine;
using System.Collections;

public class Search_button : MonoBehaviour
{

    public GameObject TextView;
    public PlaceSearch PlaceSearch;
    public MiniMap MiniMap;
    //public 対象スクリプト名 対象スクリプト名;


    /// ボタンをクリックした時の処理
    public void OnClick()
    {
        TextView.transform.localPosition = new Vector3(-240, 270, 0);
        MiniMap.transform.localPosition = new Vector3(300, 180, 0);
        MiniMap.Switch();
        PlaceSearch.Search();
        //対象スクリプト名.対象関数名();
    }
}

