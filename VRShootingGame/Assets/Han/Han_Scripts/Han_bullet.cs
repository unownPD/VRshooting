using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총알은 무조건 rigidbody를 갖는다
[RequireComponent(typeof(Rigidbody))]
public class Han_bullet : MonoBehaviour {

    //리지드바디
    Rigidbody rb;
    //총알 속도
    public float bullet_speed = 50;
    //총알 수명
    public float destroytime = 3;
    //총알 충돌 이펙트
    public GameObject FX_bullet_enemy;
    public GameObject FX_bullet_water;
    public GameObject FX_bullet_ground;
    

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();

        //총알은 중력영향을 받지 않는다
        rb.useGravity = false;
        //총알 속도
        rb.velocity =  transform.forward * bullet_speed;
        //총알은 destroytime 후 사라진다
        Destroy(gameObject, destroytime);
	}
    
    //해당 물체가 충돌하면
    void OnCollisionEnter(Collision other)
    {
        //Layer를 검사하여 적에게 부딪히면
        if (other.gameObject.layer == LayerMask.NameToLayer("Layer_enemy"))
        {
            //적의 HP를 가져와서
            Han_enemyHP enemyHP = other.gameObject.GetComponent<Han_enemyHP>();
            //HP감소
            enemyHP.NowHP--;

            //이펙트 생성 위치 소멸
            GameObject FX_enemy = Instantiate(FX_bullet_enemy);
            FX_enemy.transform.position = transform.position;
            Destroy(FX_enemy, 1);

            //사라진다
            Destroy(gameObject);
        }

        //Layer를 검사하여 물에 부딪히면
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            GameObject FX_water = Instantiate(FX_bullet_water);

            FX_water.transform.position = transform.position;

            Destroy(FX_water, 1);

            //사라진다
            Destroy(gameObject);
        }

        //Layer를 검사하여 땅에 부딪히면
        if (other.gameObject.layer == LayerMask.NameToLayer("Layer_ground"))
        {

            //사라진다
            Destroy(gameObject);
        }

        //다른곳에 충돌하면
        else
        {
            Destroy(gameObject);
        }
    }
}
