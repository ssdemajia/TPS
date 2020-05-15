using Shaoshuai.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Destructable
{
    [SerializeField] HPCounter hp;
    [SerializeField] SpawnPoint spawnPoint;
    [SerializeField] WeaponReload reload;
    CharacterController character;

    private void Start()
    {
        character = GetComponent<CharacterController>();
        hitPoints = GameManager.Instance.CurrentPlayer.level * 100;
        damageTaken = hitPoints - GameManager.Instance.CurrentPlayer.hp;
    }
    void SpawnAtNewPoint()
    {
        // 因为CharacterController会再Update里更新，所以这里关闭它
        character.enabled = false;
        transform.position = spawnPoint.transform.position;
        transform.rotation = spawnPoint.transform.rotation;
        character.enabled = true;
    }

    public override void Die()
    {
        base.Die();
        SpawnAtNewPoint();
        Reset();
        reload.Reset();
        
    }

    private void LateUpdate()
    {
        if (hp == null)
            return;
        GameManager.Instance.CurrentPlayer.hp = HitPointsRemain;
        hitPoints = GameManager.Instance.CurrentPlayer.level * 100;
        hp.Display(HitPointsRemain, hitPoints);
    }
}
