﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMove : MonoBehaviour {

    List<Transform> points = new List<Transform>();

    private int destPoint = 0;
    private NavMeshAgent agent;

    public WayPointSystem path;


    void Start()
    {
        points = path.waypoint;

        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Count == 0)
        {            
            return;
        }
        
        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        //cycling to the start if necessary. 
        destPoint = (destPoint + 1) % points.Count;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.      

        if (agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
        
    }

}
