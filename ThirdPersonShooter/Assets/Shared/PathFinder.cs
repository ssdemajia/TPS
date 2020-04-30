using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PathFinder : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] float distThreshold; 
    public event System.Action OnDestinationReach;
    bool reached;
    bool Reached
    {
        get { return reached; }
        set
        {
            reached = value;
            if (reached)
            {
                if (OnDestinationReach != null)
                    OnDestinationReach();
            }
        }
    }
   
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
        Reached = false; // 需要清除已经到达的状态
    }

    private void Update()
    {
        if (Reached)
            return;
        if (agent.remainingDistance < distThreshold)
        {
            Reached = true;
        }
    }
}
