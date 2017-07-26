using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Han_CamRotate : MonoBehaviour {

    //민감도
    public float sensitivity = 100;

    float yRot;
    float xRot;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        yRot += h * sensitivity * Time.deltaTime;
        xRot += v * sensitivity * Time.deltaTime;

        transform.localEulerAngles = new Vector3(xRot, yRot, 0);
    }
}
