using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    // 플레이어 이동 움직임
    float h = 0.0f;
    float v = 0.0f;

    Transform tr;

    // 플레이어 속도
    public float Speed = 5.0f;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();	
	}
	
	// Update is called once per frame
	void Update () {

        // 좌 우 받아옴
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        // 전후좌우 이동방향 계산
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir * Time.deltaTime * Speed, Space.Self);
	}
}
