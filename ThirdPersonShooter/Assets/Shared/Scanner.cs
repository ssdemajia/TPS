using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Extensions;
using Shaoshuai.Core;

[RequireComponent(typeof(SphereCollider))]
public class Scanner : MonoBehaviour
{
    [SerializeField] float scanSpeed;
    [SerializeField] [Range(0, 180)] float fieldOfView;  // 敌人的观察范围
    public LayerMask mask;
    SphereCollider rangeTrigger;

    public float ScanRange
    {
        get
        {
            if (rangeTrigger == null)
                rangeTrigger = GetComponent<SphereCollider>();
            return rangeTrigger.radius;
        }
    }

    public event System.Action OnScanReady; // 玩家被选中

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + GetViewAngle(fieldOfView / 2) * GetComponent<SphereCollider>().radius);
        Gizmos.DrawLine(transform.position, transform.position + GetViewAngle(-fieldOfView / 2) * GetComponent<SphereCollider>().radius);
    }

    Vector3 GetViewAngle(float angle)
    {
        float radian = (angle + transform.eulerAngles.y) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    void PrepareScan()
    {
        GameManager.Instance.Timer.Add(() =>
        {
            if (OnScanReady != null)
                OnScanReady();
        }, scanSpeed);
    }

    public List<T> ScanForTargets<T>()
    {
        List<T> targets = new List<T>();
        Collider[] result = Physics.OverlapSphere(transform.position, ScanRange);
        for (int i = 0; i < result.Length; i++)
        {
            var player = result[i].transform.GetComponent<T>();
            if (player == null)
                continue;

            if (!transform.IsInLineOfSight(result[i].transform.position, fieldOfView, mask, Vector3.up))
                continue;
            targets.Add(player);
        }
        PrepareScan(); // 每秒扫描
        return targets;
    }

}
