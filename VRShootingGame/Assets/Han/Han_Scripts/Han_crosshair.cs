using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Han_crosshair : MonoBehaviour {

    //크로스헤어 이미지
    public GameObject crosshair;

    //크로스헤어 첫 크기
    Vector3 originScale;
    
    //크기 보정값
    public float value;

	// Use this for initialization
	void Start ()
    {
        //크로스헤어 크기 측정
        originScale = crosshair.transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hitinfo;

        int layerMask = 1 << LayerMask.NameToLayer("UI");

        layerMask = ~layerMask;

        if(Physics.Raycast(ray, out hitinfo, 900 ,layerMask))
        {
            //크로스헤어의 위치 = 레이가 부딪힌 곳
            crosshair.transform.position = hitinfo.point;

            //크로스헤어의 크기를 원래 크기 * 거리 * 보정값
            crosshair.transform.localScale = originScale * hitinfo.distance * value;
        }
	}
}
