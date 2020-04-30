using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Destructable
{
    [SerializeField] SpawnPoint[] spawnPoints;
    //[SerializeField] Ragdoll ragdoll;
    void SpawnAtNewPoint()
    {
        int index = Random.Range(0, spawnPoints.Length);
        transform.position = spawnPoints[index].transform.position;
        transform.rotation = spawnPoints[index].transform.rotation;
    }
    public override void Die()
    {
        base.Die();
        //SpawnAtNewPoint();
        //ragdoll.EnableRagdoll(false);
    }

}
