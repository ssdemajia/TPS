using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    public event System.Action<Waypoint> OnWaypointChanged;

    Waypoint[] waypoints;
    int currentWP = -1;
    private void Awake()
    {
        waypoints = GetComponentsInChildren<Waypoint>();
    }

    public void SetNextWaypoint()
    {
        currentWP = (currentWP + 1) % waypoints.Length ;
        if (OnWaypointChanged != null)
            OnWaypointChanged(waypoints[currentWP]);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        waypoints = GetComponentsInChildren<Waypoint>();
        Vector3 prevWP = Vector3.zero;

        foreach (var wp in waypoints)
        {
            Gizmos.DrawWireSphere(wp.transform.position, .3f);
            if (prevWP != Vector3.zero)
            {
                Gizmos.DrawLine(prevWP, wp.transform.position);
            }
            prevWP = wp.transform.position;
        }
    }
}
