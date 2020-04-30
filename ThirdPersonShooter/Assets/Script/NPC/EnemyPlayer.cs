using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyPlayer : MonoBehaviour
{
    PathFinder pathFinder;

    public Scanner scanner;
    public Animator animator;
    public event System.Action<Player> OnTargetSelected; // 通过scanner发现player后

    EnemyHealth enemyHealth;
    public EnemyHealth EnemyHealth
    {
        get
        {
            if (enemyHealth == null)
                enemyHealth = GetComponent<EnemyHealth>();
            return enemyHealth;
        }
    }
    Player priorityTarget;
    List<Player> targets;

    private void Start()
    {
        pathFinder = GetComponent<PathFinder>();
        scanner.OnScanReady += Scanner_OnScanReady;
        Scanner_OnScanReady();
        enemyHealth.OnDeath += EnemyHealth_OnDeath;
    }

    private void EnemyHealth_OnDeath()
    {
        
    }

    private void Scanner_OnScanReady()
    {
        if (priorityTarget != null)
            return;
        targets = scanner.ScanForTargets<Player>();
        if (targets.Count == 1)
            priorityTarget = targets[0];
        else
            SelectClosestTarget(targets);

        if (priorityTarget != null)
        {
            if (OnTargetSelected != null)
                OnTargetSelected(priorityTarget);
        }
    }

    void CheckEaseWeapon()
    {

    }

    void CheckContinuePatrol()
    {
        if (priorityTarget != null)
            return;
        pathFinder.agent.isStopped = false;
    }
    // 清除目标，然后开始扫描
    internal void ClearTargetAndScan()
    {
        priorityTarget = null;
        GameManager.Instance.Timer.Add(CheckEaseWeapon, UnityEngine.Random.Range(3, 9));
        GameManager.Instance.Timer.Add(CheckContinuePatrol, UnityEngine.Random.Range(3, 9));
        Scanner_OnScanReady();
    }

    // 选择范围内的player
    private void SelectClosestTarget(List<Player> players)
    {
        float closetTarget = scanner.ScanRange;
        foreach (var possibleTarget in players)
        {
            if (Vector3.Distance(transform.position, possibleTarget.transform.position) < closetTarget)
            {
                priorityTarget = possibleTarget;
                break;
            }
        }
    }

    private void Update()
    {
        if (!EnemyHealth.IsAlive)
            return;
        animator.SetFloat("Vertical", pathFinder.agent.velocity.z);
        if (priorityTarget == null)
            return;
        transform.LookAt(priorityTarget.transform.position);
    }
}
