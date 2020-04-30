using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] float hitPoints = 5f;
    public event System.Action OnDeath;
    public event System.Action OnDamageReceived;

    public float damageTaken;
    public float HitPointsRemain
    {
        get
        {
            return hitPoints - damageTaken;
        }
    }

    public bool IsAlive
    {
        get
        {
            return HitPointsRemain > 0f;
        }
    }
    public virtual void Die()
    {
        OnDeath?.Invoke();
    }

    public virtual void TakeDamage(float amount)
    {
        if (!IsAlive)
            return;
        damageTaken += amount;
        if (OnDamageReceived != null)
            OnDamageReceived();
        if (HitPointsRemain <= 0)
            Die();
    }

    public void Reset()
    {
        damageTaken = 0;
    }
}
