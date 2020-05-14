using Shaoshuai.Core;
using UnityEngine;

// NPC进行巡逻
[RequireComponent(typeof(PathFinder))]
[RequireComponent(typeof(EnemyPlayer))]
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] WaypointController waypointController;
    [SerializeField] float waitTimeMin;
    [SerializeField] float waitTimeMax;

    PathFinder path;
    EnemyPlayer npc;
    public EnemyPlayer NPC
    {
        get
        {
            if (npc == null)
                npc = GetComponent<EnemyPlayer>();
            return npc;
        }
    }
    private void Start()
    {
        waypointController.SetNextWaypoint();
    }

    private void Awake()
    {
        path = GetComponent<PathFinder>();
        path.OnDestinationReach += Path_OnDestinationReach;
        waypointController.OnWaypointChanged += WaypointController_OnWaypointChanged;
        NPC.EnemyHealth.OnDeath += EnemyHealth_OnDeath;
        NPC.OnTargetSelected += NPC_OnTargetSelected;
    }

    private void NPC_OnTargetSelected(Player obj)
    {
        if (path.agent.isActiveAndEnabled)
            path.agent.isStopped = true;
    }

    // 当npc死亡时
    private void EnemyHealth_OnDeath()
    {
        if (path.agent.isActiveAndEnabled)
            path.agent.isStopped = true;
    }

    // 由PathFinder导航，到达目的地，然后等待一段时间后前往下一个目的地
    private void Path_OnDestinationReach()
    {
        GameManager.Instance.Timer.Add(waypointController.SetNextWaypoint,
            Random.Range(waitTimeMin, waitTimeMax));
    }

    // 切换到下一个目的点
    private void WaypointController_OnWaypointChanged(Waypoint obj)
    {
        path.SetTarget(obj.transform.position);
    }

}
