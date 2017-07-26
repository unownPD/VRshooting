using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Han_hand : MonoBehaviour {


    //컨트롤러
    public OVRInput.Controller handControllerR;
    public OVRInput.Controller handControllerL;

    //진동 테스트
    public bool useHaptics = false;
    public bool useSound = false;

    AudioSource AudioPlay;
    OVRHapticsClip hapticsClip;
    float hapticsClipLength;
    float hapticsTimeout;

    //손
    public GameObject handR;
    public GameObject handL;

    //범위
    public float grapRange = 0.1f;

    //잡았는지 체크해야 할 것들...로 하는게 편할 듯
    public GameObject RotateHandleR;
    public GameObject RotateHandleL;
    public GameObject SpeedHandleR;
    public GameObject SpeedHandleL;


    //눌렀는가 아닌가 체크
    public bool grapRdown;
    public bool grapLdown;

    //잡기 성공 했는가?
    public static bool grapRotateRok;
    public bool grapRotateLok;
    public bool grapSpeedRok;
    public bool grapSpeedLok;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float grapR = Input.GetAxis("HandleGripR");
        float grapL = Input.GetAxis("HandleGripL");
        

        //꽉 쥐는 순간에
        if(grapR == 1 && grapRdown == false)
        {
            //이걸 통해 1번만 작동
            grapRdown = true;

            //레이를 발사
            Ray ray = new Ray(handR.transform.position, handR.transform.forward);

            //grapRange 범위 만큼 원형으로 발사
            RaycastHit[] hitinfos = Physics.SphereCastAll(ray, grapRange, 0);

            //무언가 잡히면
            if(hitinfos.Length > 0)
            {
                for(int i = 0; i < hitinfos.Length; i++)
                {
                    //RotateHandleR을 잡으면
                    if(hitinfos[i].transform.gameObject == RotateHandleR)
                    {
                        grapRotateRok = true;
                    }
                    //SpeedHandleR을 잡으면
                    else if(hitinfos[i].transform.gameObject == SpeedHandleR)
                    {
                        grapSpeedRok = true;
                    }
                }
            }
        }
        //핸들을 꽉쥐고 있지않으면 다 풀림
        else if(grapR < 1)
        {
            grapRdown = false;
            grapRotateRok = false;
            grapSpeedRok = false;
        }

        if(grapRotateRok == true)
        {
            grabRotateR();
        }

        if (grapSpeedRok == true)
        {
            grabSpeedR();
        }
    }

    void grabRotateR()
    {
        //handR.transform.parent = RotateHandleR.transform.parent;

        handR.transform.position = RotateHandleR.transform.position;

        RotateHandleR.transform.parent.transform.localRotation = OVRInput.GetLocalControllerRotation(handControllerR);
    }

    void grabSpeedR()
    {
        //handR.transform.parent = SpeedHandleR.transform.parent;

        //진동
        AudioSource AudioPlayTemp = GetComponent<AudioSource>();

        hapticsClip = new OVRHapticsClip(AudioPlayTemp.clip);
        hapticsClipLength = AudioPlayTemp.clip.length;
        AudioPlay = AudioPlayTemp;

        OVRHaptics.RightChannel.Preempt(hapticsClip);

        SpeedHandleR.transform.parent.transform.position = handR.transform.position;
    }
}
