using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    private Transform target;

    [Header("Attributes")]

    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdwon = 0f;

    [Header("Unity Setup Fields")]

    public string PlayerTag = "Player";

    public Transform PartToRotate;

    public float turnSpeed = 10f;

    public GameObject BulletPrefab;
    public Transform firePoint;

	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);	
	}

    void UpdateTarget()
    {
        Debug.Log("aaa");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(PlayerTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        foreach(GameObject player in enemies)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if(distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = player;
            }
        }

        if(nearestPlayer != null && shortestDistance <= range)
        {
            target = nearestPlayer.transform;
        }

        else
        {
            target = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(target == null)
        {
            return;
        }

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        PartToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if(fireCountdwon <= 0f)
        {
            Shoot();
            fireCountdwon = 1f / fireRate;
        }

        fireCountdwon -= Time.deltaTime;
    }

    void Shoot()
    {
        Debug.Log("SHOOT!");
        GameObject bulletGO =(GameObject) Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);   
    }
}
