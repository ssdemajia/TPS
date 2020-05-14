using Shaoshuai.Core;
using Shaoshuai.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Destructable
{
    [SerializeField] ExpCounter expCounter;
    [SerializeField] Ragdoll ragdoll;
    FloatBar floatBar;
    private void Awake()
    {
        floatBar = FloatBarManager.CreateFloatBar(transform, HitPointsRemain, hitPoints);
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
        //GameManager.Instance.EventBus.RaiseEvent("EnemyDeath");
    }

    private void LateUpdate()
    {
        floatBar.UpdateHP(HitPointsRemain, hitPoints);
    }
}
