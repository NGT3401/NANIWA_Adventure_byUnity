using UnityEngine;
using System.Collections;

public class ClampPosition : MonoBehaviour
{

    [SerializeField]
    private bool clampX;
    [SerializeField]
    private bool clampY;
    [SerializeField]
    private bool clampZ;
    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxY;
    [SerializeField]
    private float minZ;
    [SerializeField]
    private float maxZ;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public void  Clamp()
    {

        Vector3 pos = transform.position;

        if (clampX)
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
        if (clampY)
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
        if (clampX)
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;

    }
}
