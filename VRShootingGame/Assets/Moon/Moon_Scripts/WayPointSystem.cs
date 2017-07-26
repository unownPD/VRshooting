using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WayPointSystem : MonoBehaviour {

    public List<Transform> waypoint = new List<Transform>();

    public bool disableinGame;

    int index = 0; 

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (!disableinGame)
        {
            Transform[] tem = GetComponentsInChildren<Transform>();

            if (tem.Length > 0)
            {
                waypoint.Clear();

                index = 0;

                foreach (Transform t in tem)
                {
                    if (t != transform)
                    {
                        t.name = "Way" + index.ToString();
                        waypoint.Add(t);

                        index++;
                    }
                }

            }
        }
    }
    void OnDrawGizmos()
    {
        if(waypoint.Count > 0)
        {
            Gizmos.color = Color.green;

            foreach (Transform t in waypoint)
            {
                Gizmos.DrawSphere(t.position, 1f);
            }

            Gizmos.color = Color.red;

            for (int a = 0; a < waypoint.Count - 1; a++)
            {
                Gizmos.DrawLine(waypoint [a].position, waypoint [a + 1].position);
            }
        }
    }
}
