using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Han_PlayerFire : MonoBehaviour {

    //VR로 보는 카메라(지금은 PC로 놓는다)
    public GameObject view_camera;
    //주인공 기체
    public GameObject Player_Plane;
    //크로스헤어
    public GameObject crosshair;

    //핸드 컨트롤러
    public OVRInput.Controller handcontroller;

    //버튼
    public OVRInput.Button Button_Fire_R;
    public OVRInput.Button Button_Fire_L;


    //발사할 무기들
    public GameObject Weapon_bullet;
    public GameObject Weapon_missile;
    public GameObject Weapon_lazer;

    //무기 쿨타임
    public float CoolTime_bullet = 0.05f;
    public float CoolTime_missile = 3;
    public float CoolTime_lazer = 0;

    //무기 탄환수
    public int Max_bullet = 1000;
    public int Max_missile = 32;
    public int Max_lazer = 2000;

    //지금 탄환수
    public int Now_bullet;
    public int Now_missile;
    public int Now_lazer;

    //불렛의 건트리거가 작동 되냐 안되냐 체크
    public bool bullet_GunTrigger;
    //트리거 민감도
    public float bullet_GunTriggerSen = -0.3f;

    //현재시간
    public float currentTime;

    //록온 스크립트 위치파악
    public GameObject LockScript;
    Han_LockON LockON_List;

    //발사할 위치
    public Transform FireR;
    public Transform FireL;
    public Transform FireCenter;

    //레이저 발사포대1,2
    public GameObject Lazer_01;
    public GameObject Lazer_02;

    //레이저 두깨
    public float LazerWidth = 1;

    //레이저 충돌시
    public GameObject FX_LazerImpact;

    //레이저 라인렌더러
    LineRenderer lr_01;
    LineRenderer lr_02;

    //사운드 관련
    AudioSource AudioPlay;
    public AudioClip SE_BulletFire;

    //진동 테스트
    public bool useHaptics = false;
    public bool useSound = false;
    
    OVRHapticsClip hapticsClip;
    float hapticsClipLength;
    float hapticsTimeout;

    //무기 상태
    public enum GameState
    {
        bullet,
        missile,
        lazer
    }

    public GameState m_state;    

    // Use this for initialization
    void Start ()
    {
        Now_bullet = Max_bullet;
        Now_missile = Max_missile;
        Now_lazer = Max_lazer;

        LockON_List = LockScript.GetComponent<Han_LockON>();

        AudioPlay = GetComponent<AudioSource>();

        lr_01 = Lazer_01.GetComponent<LineRenderer>();
        lr_02 = Lazer_02.GetComponent<LineRenderer>();

        FX_LazerImpact.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		switch(m_state)
        {
            case GameState.bullet:
                bullet();
                break;

            case GameState.missile:
                missile();
                break;

            case GameState.lazer:
                lazer();
                break;
        }        
	}


    public void test(int aa)
    {
        if(aa == 1)
        {
            m_state = GameState.bullet;
        }
    }


    public void bullet()
    {
        //총포의 방향은 카메라의 방향과 일치한다
        //FireR.transform.forward = view_camera.transform.forward;
        //FireL.transform.forward = view_camera.transform.forward;
        //총포의 방향은 크로스헤어 위치로
        FireR.transform.forward = crosshair.transform.position - FireR.transform.position;
        FireL.transform.forward = crosshair.transform.position - FireL.transform.position;

        currentTime += Time.deltaTime;

        //방아쇠 값을 받아서 0 ~ -1사이 값
        float GunTriggerR = Input.GetAxis("GunTriggerR");
        float GunTriggerL = Input.GetAxis("GunTriggerL");


        //둘 중에 하나라도 일정 수치 이하면 트리거 온 (Input.GetMouseButton(0) 
        if (GunTriggerR < bullet_GunTriggerSen || GunTriggerL < bullet_GunTriggerSen || Input.GetMouseButton(0))
        {
            bullet_GunTrigger = true;
            
        }
        //둘 다 일정수치(TriggerSen) 이상이면 트리거 오프
        else if(GunTriggerR > bullet_GunTriggerSen && GunTriggerL > bullet_GunTriggerSen)
        {
            bullet_GunTrigger = false;
        }

        if (bullet_GunTrigger && Now_bullet > 0 && CoolTime_bullet < currentTime)
        {
            AudioPlay.PlayOneShot(SE_BulletFire);

            //총알 생성
            GameObject bulletR = Instantiate(Weapon_bullet);
            GameObject bulletL = Instantiate(Weapon_bullet);
            //총알 위치
            bulletR.transform.position = FireR.position;
            bulletL.transform.position = FireL.position;
            //총알 방향
            bulletR.transform.forward = FireR.forward;
            bulletL.transform.forward = FireL.forward;

            //총탄 소비 -2
            Now_bullet = Now_bullet - 2;

            //시간 초기화
            currentTime = 0;

            //진동
            AudioSource AudioPlayTemp = GetComponent<AudioSource>();

            hapticsClip = new OVRHapticsClip(AudioPlayTemp.clip);
            hapticsClipLength = AudioPlayTemp.clip.length;
            AudioPlay = AudioPlayTemp;

            OVRHaptics.RightChannel.Preempt(hapticsClip);
        }       
    }

    public void missile()
    {
        //총포의 방향은 기체의 방향과 일치한다
        FireR.transform.forward = Player_Plane.transform.forward;
        FireL.transform.forward = Player_Plane.transform.forward;

        currentTime += Time.deltaTime;

        if (Input.GetButtonDown("GunButtonR") && Now_missile > 0 && CoolTime_missile < currentTime)
        {
            //락온 한 녀석이 존재할 경우
            if(LockON_List.Lock_ONobj.Count > 0 )
            {                
                for (int i = 0; i < LockON_List.Lock_ONobj.Count; i++)
                {
                    //미사일 생성
                    GameObject missile = Instantiate(Weapon_missile);

                    Han_missile script = missile.GetComponent<Han_missile>();

                    //미사일 위치
                    missile.transform.position = FireR.position;

                    //미사일 방향
                    missile.transform.forward = FireR.forward;

                    //미사일 소비
                    Now_missile = Now_missile - 1;

                    //쿨타임 초기화
                    currentTime = 0;

                    //타겟을 집어넣는다
                    script.target = LockON_List.Lock_ONobj[i];

                    if(Now_missile == 0)
                    {
                        break;
                    }
                }                               
            }
            else
            {
                //미사일 생성
                GameObject missile = Instantiate(Weapon_missile);

                Han_missile script = missile.GetComponent<Han_missile>();

                //미사일 위치
                missile.transform.position = FireR.position;

                //미사일 방향
                missile.transform.forward = FireR.forward;

                //미사일 소비
                Now_missile = Now_missile - 1;

                //쿨타임 초기화
                currentTime = 0;

                //타겟 없음
                script.target = null;
            }
        }

        else if (Input.GetButtonDown("GunButtonL") && Now_missile > 0 && CoolTime_missile < currentTime)
        {
            //락온 한 녀석이 존재할 경우
            if (LockON_List.Lock_ONobj.Count > 0)
            {
                for (int i = 0; i < LockON_List.Lock_ONobj.Count; i++)
                {
                    //미사일 생성
                    GameObject missile = Instantiate(Weapon_missile);

                    Han_missile script = missile.GetComponent<Han_missile>();

                    //미사일 위치
                    missile.transform.position = FireL.position;

                    //미사일 방향
                    missile.transform.forward = FireL.forward;

                    //미사일 소비
                    Now_missile = Now_missile - 1;

                    //쿨타임 초기화
                    currentTime = 0;

                    //타겟을 집어넣는다
                    script.target = LockON_List.Lock_ONobj[i];

                    if (Now_missile == 0)
                    {
                        break;
                    }
                }
            }
            else
            {
                //미사일 생성
                GameObject missile = Instantiate(Weapon_missile);

                Han_missile script = missile.GetComponent<Han_missile>();

                //미사일 위치
                missile.transform.position = FireL.position;

                //미사일 방향
                missile.transform.forward = FireL.forward;

                //미사일 소비
                Now_missile = Now_missile - 1;

                //쿨타임 초기화
                currentTime = 0;

                //타겟 없음
                script.target = null;
            }
        }
    }
    
    public void lazer()
    {
        currentTime += Time.deltaTime;

        //방아쇠 값을 받아서 0 ~ -1사이 값
        float GunTriggerR = Input.GetAxis("GunTriggerR");
        float GunTriggerL = Input.GetAxis("GunTriggerL");

        if (GunTriggerR < bullet_GunTriggerSen || GunTriggerL < bullet_GunTriggerSen || Input.GetMouseButton(0))
        {
            bullet_GunTrigger = true;

        }
        //둘 다 일정수치(TriggerSen) 이상이면 트리거 오프
        else if (GunTriggerR > bullet_GunTriggerSen && GunTriggerL > bullet_GunTriggerSen)
        {
            bullet_GunTrigger = false;
        }

        if (bullet_GunTrigger && Now_lazer > 0 && CoolTime_lazer < currentTime)
        {
            
            Ray ray = new Ray(FireCenter.position, FireCenter.forward);

            RaycastHit hitinfo;

            currentTime = 0;

            Now_lazer--;

            if (Physics.Raycast(ray, out hitinfo))
            {
                //레이저 보이게 하는 기능
                lr_01.enabled = true;
                lr_02.enabled = true;

                lr_01.SetPosition(0, FireCenter.position);
                lr_02.SetPosition(0, FireCenter.position);

                if(hitinfo.collider != null)
                {
                    lr_01.SetPosition(1, hitinfo.point);
                    lr_02.SetPosition(1, hitinfo.point);

                    FX_LazerImpact.SetActive(true);

                    FX_LazerImpact.transform.position = hitinfo.point;

                    FX_LazerImpact.transform.forward = hitinfo.normal;
                }
                

                lr_01.startWidth = LazerWidth;
                lr_01.endWidth = LazerWidth;

                lr_02.startWidth = LazerWidth;
                lr_02.endWidth = LazerWidth;               
                
                //적인거 확인
                if(hitinfo.transform.gameObject.layer == LayerMask.NameToLayer("Layer_enemy"))
                {
                    //적의 HP를 가져와서
                    Han_enemyHP enemyHP = hitinfo.transform.gameObject.GetComponent<Han_enemyHP>();
                    //HP감소
                    enemyHP.NowHP--;
                }

            }
            else
            {
                lr_01.enabled = true;
                lr_02.enabled = true;

                lr_01.SetPosition(0, FireCenter.position);
                lr_02.SetPosition(0, FireCenter.position);

                lr_01.SetPosition(1, FireCenter.position + FireCenter.transform.forward * 900);
                lr_02.SetPosition(1, FireCenter.position + FireCenter.transform.forward * 900);

                lr_01.startWidth = LazerWidth;
                lr_01.endWidth = LazerWidth;

                lr_02.startWidth = LazerWidth;
                lr_02.endWidth = LazerWidth;
            }
        }
        else
        {
            FX_LazerImpact.SetActive(false);

            lr_01.enabled = false;
            lr_02.enabled = false;
        }
    }
}
