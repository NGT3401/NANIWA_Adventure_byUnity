using UnityEngine;
using System.Collections;

public class ShakeCamera : MonoBehaviour {

    private Vector3 initCameraPosition;
    private float shakingTime;
    private float initShakingTime;
    private float shakingRange;
    private Vector3 shakingDirection;

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {

        if (shakingTime > 0)
        {
            float per = shakingRange * (shakingTime / initShakingTime);
            shakingTime -= Time.deltaTime;
            transform.position = initCameraPosition + shakingDirection * shakingRange * per * Mathf.Sin(4*Mathf.PI * per);
            if (shakingTime <= 0)
                transform.position = initCameraPosition;

        }        
	}

    public void ShakingCamera(float time, float range)
    {
        initCameraPosition = transform.position;
        initShakingTime = shakingTime = time;
        shakingRange = range;
        shakingDirection = transform.right * Random.Range(-1f, 1f) + transform.up * Random.Range(-1f, 1f);
    }
}
