using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Extensions;

//[RequireComponent(typeof(EnemyPlayer))]
public class EnemyShoot : MonoBehaviour
{
    //[SerializeField] float shootSpeed;
    //[SerializeField] float shootDurationMin;
    //[SerializeField] float shootDurationMax;
    //[SerializeField] Shooter gun;
    //bool shouldFire;
    //EnemyPlayer enemy;
    //private void Start()
    //{
    //    enemy = GetComponent<EnemyPlayer>();
    //    enemy.OnTargetSelected += Enemy_OnTargetSelected;
    //}

    //private void Enemy_OnTargetSelected(Player obj)
    //{
    //    gun.AimTarget = obj.transform;
    //    gun.AimTargetOffset = Vector3.zero;
    //    StartBurst();
    //}

    //void CrouchState()
    //{
    //    bool takeCover = Random.Range(0, 3) == 0;
    //    if (!takeCover)
    //        return;
    //    float distanceToTarget = Vector3.Distance(transform.position, gun.AimTarget.position);
    //    if (distanceToTarget > 15)
    //    {
    //        enemy.animator.SetBool("IsCrouch", true);
    //    }
    //}
    //void StartBurst()
    //{
    //    if (!enemy.EnemyHealth.IsAlive)
    //        return;
    //    shouldFire = true;
    //    CrouchState();
    //    GameManager.Instance.Timer.Add(EndBurst, Random.Range(shootDurationMin, shootDurationMax));
    //}

    //void EndBurst() {
    //    shouldFire = false;
    //    if (!enemy.EnemyHealth.IsAlive)
    //        return;
    //    if (CanSeeTarget()) // 可以看见时才进行射击
    //        GameManager.Instance.Timer.Add(StartBurst, shootSpeed);
    //}

    //// 判断是否可以看见目标
    //bool CanSeeTarget()
    //{
    //    if (!transform.IsInLineOfSight(gun.AimTarget.position, 90, enemy.scanner.mask, Vector3.up))
    //    {
    //        enemy.ClearTargetAndScan();
    //        return false;
    //    }
    //    return true;
    //}

    //private void Update()
    //{
    //    if (!enemy.EnemyHealth.IsAlive)
    //        return;
    //    if (shouldFire)
    //    {
    //        gun.Fire();
    //    }
    //}
}
