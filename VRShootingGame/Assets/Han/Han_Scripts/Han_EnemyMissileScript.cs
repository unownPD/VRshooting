using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Han_EnemyMissileScript : MonoBehaviour {

    //리지드바디
    Rigidbody rb;
    //사운드
    AudioSource AudioPlay;
    //사운드 클립
    public AudioClip SE_MissileLunch;
    public AudioClip SE_MissileExplosion;

    bool sound_ing = false;

    //중력
    public float gravity = -9.8f;
    //비행기 속도
    public float player_speed;
    //미사일 엔진점화까지 시간
    public float enginestart_time = 0.7f;
    //미사일 가속 위한 값
    public float accel_speed;
    //미사일 가속 보정치
    public float value = 200;
    //미사일 최고 속도
    public float Max_speed = 50;
    //미사일 회전 속도
    public float rotate_speed = 0.003f;
    //미사일 수명
    public float destroytime = 10;
    //미사일 데미지
    public float damage = 6;
    //미사일 피해 범위
    public float damageRange = 3;
    //미사일 피해 충격량
    public float boomPower = 10;
    //미사일 위로 충격량
    public float boomUpPower = 5;

    //미사일 연기 이펙트
    public GameObject FX_missile_smoke;
    public GameObject FX_missile_fire;

    //미사일 충돌 이펙트
    public GameObject FX_missile_enemy;
    public GameObject FX_missile_water;
    public GameObject FX_missile_ground;

    //목표 타겟
    public GameObject target;

    //현재 시간
    float currentTime;
    
    // Use this for initialization
    void Start ()
    {
        //리지드바디 겟컴퍼넌트
        rb = GetComponent<Rigidbody>();
        //오디오 소스 겟컴퍼넌트
        AudioPlay = GetComponent<AudioSource>();

        //미사일은 중력영향을 받지 않는다
        rb.useGravity = false;

        //연기,불 이펙트는 아직
        FX_missile_smoke.SetActive(false);
        FX_missile_fire.SetActive(false);

        target = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (sound_ing == false)
        {
            AudioPlay.clip = SE_MissileLunch;
            AudioPlay.PlayOneShot(SE_MissileLunch);
            sound_ing = true;
        }
        //연기,불 이펙트 on
        FX_missile_smoke.SetActive(true);
        FX_missile_fire.SetActive(true);

        //음 없애도 될듯한데...
        //rb.velocity = Vector3.zero;

        //시간에 지나면 점점 증가(보정치를 곱한 값으로)
        accel_speed += Time.deltaTime * value;

        //속력 최대값 조절
        accel_speed = Mathf.Clamp(accel_speed, 0, Max_speed);

        //타겟을 바라보는 벡터
        Vector3 dir = new Vector3(0, 0, 0);

        if (target == null)
        {
            dir = transform.forward;
        }
        else
        {
            dir = target.transform.position - transform.position;
        }

        /* 밑으로 해도 유도 작동
        Quaternion dirRot = Quaternion.LookRotation(dir);
        //내 방향(quaternion)을 천천히 타겟 방향으로
        Quaternion myRot = Quaternion.Slerp(transform.rotation, dirRot, rotate_speed * Time.deltaTime);
        //qua를 rotation 으로
        transform.rotation = myRot;
        */

        //추적 능력
        //rotate_speed += Time.deltaTime;
        //추적능력 최대값
        //rotate_speed = Mathf.Clamp(rotate_speed, 0, 1);

        transform.forward = Vector3.Lerp(transform.forward, dir, rotate_speed);

        //나의 속력 = 내가 보는 방향 * (플레이어 속력 + 가속력)
        rb.velocity = transform.forward * (player_speed + accel_speed);


        if(Vector3.Distance(target.transform.position,transform.position) < 1.0f)
        {
            destroy();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        destroy();
    }


    void destroy()
    {
        GameObject effect = Instantiate(FX_missile_enemy);

        effect.transform.position = transform.position;

        Destroy(effect, 4);

        Destroy(gameObject);
    }

}
