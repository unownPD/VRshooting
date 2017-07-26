using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public float MoveSpeed = 10f;
    public float RotateSpeed = 80f;
    public float Distance = 0;

    public Transform WaipointsParent;
    public Transform[] Waypoints;
    public Transform targetWP;

    public int targetWPIndex = 0;

	// Use this for initialization
	void Start () {

        GetWaypoints();

        targetWP = Waypoints[targetWPIndex];

    }
	
	// Update is called once per frame
	void Update () {

        FollowWaypoints();

    }

    void FollowWaypoints()
    {
        Distance = Vector3.Distance(transform.position, Waypoints[targetWPIndex].position);

        if (Distance > 1f)
        {   
            Vector3 direction = (targetWP.position - transform.position).normalized;

            transform.Translate(direction * MoveSpeed * Time.deltaTime, Space.World);

            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }

        else
        {
            targetWPIndex++;

            if(targetWPIndex == Waypoints.Length)
            {
                targetWPIndex = 0;
            }
        }

        targetWP = Waypoints[targetWPIndex];
    }

    void GetWaypoints()
    {
        int numofWPs = WaipointsParent.childCount;

        Waypoints = new Transform[numofWPs];

        for (int i = 0; i < Waypoints.Length; i++)
        {
            Waypoints[i] = WaipointsParent.GetChild(i);
        }
    }
}
