using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Han_missile : MonoBehaviour {

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
    //비행기(플레이어)
    public GameObject player;
    //비행기 스크립트
    Han_PlayerMove moveScript;
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
    public float rotate_speed = 0.5f;
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

    //상태나열 유도아닐때, 유도일때, 사라질때 
    enum GameState
    {
        notguided,
        guided,
        destroy
    }

    GameState m_state;

    Han_LockON what;

    // Use this for initialization
    void Start ()
    {
        //플레이어를 찾아서
        player = GameObject.Find("Han_Cockpit");
        //가지고 있는 스크립트를 받아
        moveScript = player.GetComponent<Han_PlayerMove>();
        //플레이어의 이동속도를 받아 저장한다
        player_speed = moveScript.moveSpeed;

        //리지드바디 겟컴퍼넌트
        rb = GetComponent<Rigidbody>();
        //오디오 소스 겟컴퍼넌트
        AudioPlay = GetComponent<AudioSource>();

        //미사일은 중력영향을 받지 않는다
        rb.useGravity = false;

        //연기,불 이펙트는 아직
        FX_missile_smoke.SetActive(false);
        FX_missile_fire.SetActive(false);

        //타겟이 들어가있는지 아닌지에 대해서 체크하는 함수
        targetcheck();
    }

    void targetcheck()
    {
        //타겟이 없으면 notguided
        if(target == null)
        {
            m_state = GameState.notguided;
        }
        //타겟이 있으면 guided
        else
        {
            m_state = GameState.guided;
        }
    }

    void Update()
    {
        switch (m_state)
        {
            case GameState.notguided:
                notguided();
                break;

            case GameState.guided:
                guided();
                break;

            case GameState.destroy:
                destroy();
                break;
        }

        //시간이 흘러 destroy시간을 넘어가면 destroy로 변경
        currentTime += Time.deltaTime;

        if(currentTime > destroytime)
        {
            m_state = GameState.destroy;
        }
    }

    void notguided()
    {
        //추적하기 전에
        if (currentTime < enginestart_time)
        {
            //플레이어 속도로 이동(관성)
            rb.velocity = transform.forward * player_speed + (transform.up * gravity);
        }
        else
        {
            //사운드 작동
            if(sound_ing == false)
            {
                AudioPlay.clip = SE_MissileLunch;
                AudioPlay.PlayOneShot(SE_MissileLunch);
                sound_ing = true;
            }

            //연기,불 이펙트 on
            FX_missile_smoke.SetActive(true);
            FX_missile_fire.SetActive(true);
            
            //가속력 증가(보정치를 곱한 값으로)
            accel_speed += Time.deltaTime * value;

            //가속력 최대값 조절
            accel_speed = Mathf.Clamp(accel_speed, 0, Max_speed);

            //나의 속력 = 내가 보는 방향 * (플레이어 속력 + 가속력)
            rb.velocity = transform.forward * (player_speed + accel_speed);            
        }
    }

    void guided()
    {
        //추적하기 전에
        if (currentTime < enginestart_time)
        {
            //플레이어 속도로 이동(관성)
            rb.velocity = transform.forward * player_speed + (transform.up * gravity);
        }
        else
        {
            //사운드 작동
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
        }        
    }

    //시간이 지나서 Destroy에 오면
    void destroy()
    {
        Destroy(gameObject);
    }    

    void OnCollisionEnter(Collision other)
    {
        //Layer를 검사하여 적에게 부딪히면
        if (other.gameObject.layer == LayerMask.NameToLayer("Layer_enemy"))
        {
            GameObject effect = Instantiate(FX_missile_enemy);

            effect.transform.position = transform.position;

            Destroy(effect, 4);

            Ray ray = new Ray(transform.position, transform.forward);

            RaycastHit[] hitinfos = Physics.SphereCastAll(ray, damageRange, damageRange, 1 << LayerMask.NameToLayer("Layer_enemy"));
            
            if (hitinfos.Length > 0)
            {
                for(int i = 0; i < hitinfos.Length; i++)
                {
                    Rigidbody rb = hitinfos[i].transform.gameObject.GetComponent<Rigidbody>();

                    Han_enemyHP enemyHP = hitinfos[i].transform.gameObject.GetComponent<Han_enemyHP>();
                    
                    //rb.useGravity = true;

                    if(Vector3.Distance(transform.position, hitinfos[i].transform.position) < (damageRange * 0.3f))
                    {
                        enemyHP.NowHP = enemyHP.NowHP - damage;
                    }

                    if(Vector3.Distance(transform.position, hitinfos[i].transform.position) < (damageRange * 0.7f) && Vector3.Distance(transform.position, hitinfos[i].transform.position) > (damageRange * 0.3f))
                    {                       
                        enemyHP.NowHP = enemyHP.NowHP - (damage * 0.6f);
                    }

                    if(Vector3.Distance(transform.position, hitinfos[i].transform.position) > (damageRange * 0.7f))
                    {
                        enemyHP.NowHP = enemyHP.NowHP - (damage * 0.3f);
                    }
                    Debug.Log("Hitinfo:" + hitinfos[i].transform.gameObject + "Distance:" + Vector3.Distance(transform.position,hitinfos[i].transform.position));

                    rb.AddExplosionForce(boomPower, transform.position, damageRange, boomUpPower);
                }
            }
            
            //사라진다
            Destroy(gameObject);
        }

        //Layer를 검사하여 물에 부딪히면
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {

            //사라진다
            Destroy(gameObject);
        }

        //Layer를 검사하여 땅에 부딪히면
        if (other.gameObject.layer == LayerMask.NameToLayer("Layer_ground"))
        {

            //사라진다
            Destroy(gameObject);
        }
        //다른 것
        else
        {
            Destroy(gameObject);
        }
    }
}
