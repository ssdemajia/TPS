using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    //[SerializeField] SpawnPoint[] spawnPoints;
    Rigidbody[] rigidbodies;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        EnableRagdoll(true);
    }

    //void SpawnAtNewPoint()
    //{
    //    int index = Random.Range(0, spawnPoints.Length);
    //    transform.position = spawnPoints[index].transform.position;
    //    transform.rotation = spawnPoints[index].transform.rotation;
    //}

    //public override void Die()
    //{
    //    base.Die();
    //    EnableRagdoll(false);
        //animator.enabled = false;
        //GameManager.Instance.Timer.Add(() =>
        //{
        //    EnableRagdoll(true);
        //    SpawnAtNewPoint();
        //    Reset();
        //    animator.enabled = true;
        //}, 2);
    //}

    //private void Update()
    //{
    //    if (!IsAlive)
    //        return;

    //}
    public void EnableRagdoll(bool value)
    {
        animator.enabled = value; // 需要关闭动画来启动死亡状态
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = value;
        }
    }
}
