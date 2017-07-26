using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Han_enemytestttttt : MonoBehaviour {

    public GameObject mother;

    Han_enemyHP hp;

    Rigidbody rb;

    BoxCollider bc;

	// Use this for initialization
	void Start ()
    {
        hp = mother.GetComponent<Han_enemyHP>();

        rb = GetComponent<Rigidbody>();

        bc = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(hp.NowHP <= 0)
        {
            rb.isKinematic = false;

            bc.isTrigger = false;
        }
	}
}
