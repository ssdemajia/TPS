using Shaoshuai.Core;
using Shaoshuai.UI;
using UnityEngine;

[RequireComponent(typeof(EnemyPlayer))]
[RequireComponent(typeof(EnemyPatrol))]
public class EnemyHealth : Destructable
{
    [SerializeField] ExpCounter expCounter;
    [SerializeField] Ragdoll ragdoll;
    [SerializeField] SpawnPoint[] spawnPoints;
    FloatBar floatBar;
    EnemyPlayer enemy;
    EnemyPatrol patrol;

    private void Awake()
    {
        patrol = GetComponent<EnemyPatrol>();
        enemy = GetComponent<EnemyPlayer>();
        floatBar = GetComponentInChildren<FloatBar>();
    }

    void SpawnAtNewPoint()
    {
        int index = Random.Range(0, spawnPoints.Length);
        transform.position = spawnPoints[index].transform.position;
        transform.rotation = spawnPoints[index].transform.rotation;
    }

    public override void TakeDamage(int amount, string location)
    {
        base.TakeDamage(amount, location);

        FloatTextManager.CreateFloatText(transform, currentDamage);
    }
    public override void Die()
    {
        base.Die();
        expCounter.Kill();
        ragdoll.EnableRagdoll(false);
        GameManager.Instance.Timer.Add(() =>
        {
            // 恢复
            Reset();
            ragdoll.EnableRagdoll(true);
            enemy.ClearTargetAndScan(); // 开始扫描
            SpawnAtNewPoint();
            patrol.ResetPatrol();
        }, 2f);
    }

    private void LateUpdate()
    {
        floatBar.UpdateHP(HitPointsRemain, hitPoints);
    }
}
