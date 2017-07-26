using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Han_enemy : MonoBehaviour {

    enum GameState
    {
        move,
        chase,
        dodge,
        escape,
        damage,
        death
    }

    GameState m_state;

    Rigidbody rb;

    Han_enemyHP HPscript;

    public float speed = 5;

    float currentTime;

    public float deathTime;

    float movecurrentTime;
    float damagecurrentTime;
    float chasecurrentTime;

    public float attacTime;

    public GameObject EnemyMissile;

    

    public GameObject target;

    public GameObject deathsmoke;
    public GameObject deathexplosion;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        HPscript = GetComponent<Han_enemyHP>();

        m_state = GameState.move;

        deathsmoke.SetActive(false);

        target = GameObject.Find("AAA");
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (m_state)
        {
            case GameState.move:
                move();
                break;

            case GameState.chase:
                chase();
                break;

            case GameState.dodge:
                dodge();
                break;

            case GameState.escape:
                escape();
                break;

            case GameState.damage:
                damage();
                break;

            case GameState.death:
                death();
                break;
        }        

        if(HPscript.NowHP <= 0)
        {
            m_state = GameState.death;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        m_state = GameState.damage;

    }


    void move()
    {
        rb.velocity = transform.forward * speed;
        
        movecurrentTime += Time.deltaTime;

        if(movecurrentTime > 5)
        {
            Debug.Log("aaa");

            movecurrentTime = 0;

            target = GameObject.Find("Han_Cockpit");

            m_state = GameState.chase;

           
        }        
    }

    void chase()
    {
        Vector3 dir = target.transform.position - transform.position;

        transform.forward = Vector3.Lerp(transform.forward, dir, 0.01f * Time.deltaTime);

        rb.velocity = transform.forward * (speed + 2);

        chasecurrentTime += Time.deltaTime;

        
        if(chasecurrentTime > attacTime)
        {
            attacTime = Random.Range(2.0f, 6.0f);

            GameObject missile = Instantiate(EnemyMissile);

            missile.transform.position = transform.position + (-Vector3.up * 2);

            chasecurrentTime = 0;
        }



    }

    void dodge()
    {

    }

    void escape()
    {

    }

    void damage()
    {
        damagecurrentTime += Time.deltaTime;

        if(damagecurrentTime > 1)
        {
            rb.velocity = Vector3.zero;

            damagecurrentTime = 0;

            if (target == null)
            {
                m_state = GameState.move;
            }
            else
            {
                m_state = GameState.chase;
            }            
        }
    }

    void death()
    {
        currentTime += Time.deltaTime;

        rb.useGravity = true;

        deathsmoke.SetActive(true);

        if(currentTime > deathTime)
        {
            GameObject explosion = Instantiate(deathexplosion);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
        }
        
    }
}
