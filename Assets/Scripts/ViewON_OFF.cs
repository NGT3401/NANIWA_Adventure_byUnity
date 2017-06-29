using UnityEngine;
using System.Collections;

public class ViewON_OFF : MonoBehaviour
{

    public GameObject TextView;
   // public GameObject MiniMap;
    public int flag = 0;

    public void OnClick()
    {
        // 0：ON to OFF
        if (flag == 0)
        {
            TextView.transform.position = new Vector3(600, 0, 0);
           // MiniMap.transform.position = new Vector3(0, 800, 0);
            flag = 1;
        }
        // 1:OFF to ON
        else if (flag == 1)
        {
            TextView.transform.localPosition = new Vector3(-240, 270, 0);
           // MiniMap.transform.localPosition = new Vector3(300, 180, 0);
            flag = 0;
        }
    }
}