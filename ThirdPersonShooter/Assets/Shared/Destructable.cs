using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public int hitPoints = 100;
    public event System.Action OnDeath;
    public event System.Action<float> OnDamageReceived;

    [SerializeField] AudioController hitAudio;
    [HideInInspector]
    public Destructable parent; // 父节点，碰撞到躯干后对角色的health扣值
    public int damageTaken;     // 总共收到的伤害
    protected int currentDamage; // 当前的收到的伤害

    public int HitPointsRemain
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

    public virtual void TakeDamage(int amount, string location)
    {
        if (!IsAlive)
            return;
        int coefficient = 1;  // 对不同部分攻击造成的伤害
        switch (location)
        {
            case "Bip001 L Thigh":
                coefficient = 2;
                break;
            case "Bip001 R Thigh":
                coefficient = 2;
                break;
            case "Bip001 Head":  // 爆头
                coefficient = 3;
                break;
            default:
                break;
        }

        amount *= coefficient;
        currentDamage = amount;
        damageTaken += amount;

        hitAudio.Play();

        if (OnDamageReceived != null)
            OnDamageReceived(damageTaken);
        if (HitPointsRemain <= 0)
            Die();
    }

    public void Reset()
    {
        damageTaken = 0;
    }
}
