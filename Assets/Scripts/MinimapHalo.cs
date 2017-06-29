using UnityEngine;
using System.Collections;

public class MinimapHalo : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        transform.position = transform.parent.position + new Vector3(0, 40 - transform.parent.position.y, 0);
        transform.localPosition = Vector3.Scale(transform.localPosition, transform.parent.lossyScale);

    }
}
