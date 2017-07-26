using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class Han_PlayerMove : MonoBehaviour {

    //플레이어
    public GameObject player;

    //회전 관련 조이스틱
    public GameObject RotateHandleR;
    public GameObject RotateHandleL;
    //처음 각도
    public GameObject ResetRotateHandleR;
    public GameObject ResetRotateHandleL;


    float xAngle;
    float yAngle;
    float zAngle;

    public float rotateValue = 0.25f;

    //속도 관련 조이스틱
    public GameObject SpeedHandleR;
    public GameObject SpeedHandleL;
    //처음 위치
    public Vector3 Pos_FirstSpeedHandleR;
    public Vector3 Pos_FirstSpeedHandleL;

    //그립했는지 안했는지 파악
    public bool RotateGrip;
    public bool SpeedGrip;

    //속도 보정값
    public float speedvalue = 50;
    

    //플레이어 속도
    public float moveSpeed;

    public GameObject cameraaaaa;


    // Use this for initialization
    void Start ()
    {
        //초기 각도 확보
        //FirsRotateHandleR = RotateHandleR.transform.localRotation;
        //FirsRotateHandleL = RotateHandleL.transform.localRotation;


        //초기위치 확보
        Pos_FirstSpeedHandleR = SpeedHandleR.transform.localPosition;
        Pos_FirstSpeedHandleL = SpeedHandleL.transform.localPosition;       
    }
	
	// Update is called once per frame
	void Update ()
    {
        RotateGrip = Han_hand.grapRotateRok;

        if (RotateGrip == true)
        {
            Quaternion rotR = RotateHandleR.transform.localRotation;
            Quaternion rotL = RotateHandleL.transform.localRotation;            
            
            Quaternion rot = rotR;

            Vector3 RRR = RotateHandleR.transform.localEulerAngles;


            
            if (RRR.x > 180)
            {
                xAngle += rotateValue * Time.deltaTime  * Mathf.Abs(RRR.x - 360);
                
            }
            else
            {
                xAngle -= rotateValue * Time.deltaTime * RRR.x ;
            }
            
            if (RRR.y > 180)
            {
                yAngle += rotateValue * Time.deltaTime * Mathf.Abs(RRR.y - 360);
            }
            else
            {
                yAngle -= rotateValue * Time.deltaTime * RRR.y;
            }
            
            if (RRR.z > 180)
            {
                zAngle += rotateValue * Time.deltaTime * Mathf.Abs(RRR.z - 360);

                Debug.Log(RRR.z);
            }
            else
            {
                zAngle -= rotateValue * Time.deltaTime * RRR.z;
            }
            


            player.transform.eulerAngles = new Vector3(-xAngle,-yAngle,-zAngle);
            //player.transform.rotation = Quaternion.Slerp(player.transform.rotation,rot,0.05f);
        }
        //그립을 놓으면 원래 자리로 돌아감...천천히
        else if(RotateGrip == false)
        {
            RotateHandleR.transform.localRotation = Quaternion.Lerp(RotateHandleR.transform.localRotation, ResetRotateHandleR.transform.localRotation, 0.006f);

            RotateHandleL.transform.localRotation = Quaternion.Lerp(RotateHandleL.transform.localRotation, ResetRotateHandleL.transform.localRotation, 0.006f);

            Quaternion rotR = RotateHandleR.transform.localRotation;
            Quaternion rotL = RotateHandleL.transform.localRotation;

            Quaternion rot = rotR;

            //player.transform.rotation = rot;
        }

        
        //SpeedHandleR z값 제한
        float limitRZ = Mathf.Clamp(SpeedHandleR.transform.localPosition.z, Pos_FirstSpeedHandleR.z - 0.15f, Pos_FirstSpeedHandleR.z + 0.2f);
        //SpeedHandleR 핸들의 위치
        SpeedHandleR.transform.localPosition = new Vector3(Pos_FirstSpeedHandleR.x, Pos_FirstSpeedHandleR.y, limitRZ);
        //SpeedHandleL z값 제한
        float limitLZ = Mathf.Clamp(SpeedHandleL.transform.localPosition.z, Pos_FirstSpeedHandleL.z - 0.2f, Pos_FirstSpeedHandleL.z + 0.3f);
        //SpeedHandleL 핸들의 위치
        SpeedHandleL.transform.localPosition = new Vector3(Pos_FirstSpeedHandleL.x, Pos_FirstSpeedHandleL.y, limitLZ);

        //핸들이 얼만큼 이동 되어 있나
        float SpeedRZ = SpeedHandleR.transform.localPosition.z - Pos_FirstSpeedHandleR.z;
        float SpeedLZ = SpeedHandleL.transform.localPosition.z - Pos_FirstSpeedHandleL.z;

        //그 사이 값
        moveSpeed = (SpeedRZ + SpeedLZ) * 0.5f * speedvalue;




        transform.Translate(transform.forward * moveSpeed * Time.deltaTime,Space.World);

        //rb.velocity = transform.forward * moveSpeed;
    }
}
