using Shaoshuai.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    [SerializeField] int maxAmmo = 400;
    [SerializeField] float reloadTime = 1f;
    [SerializeField] int clipSize = 40; // 弹夹中的子弹数量
    [SerializeField] AudioController reloadAudio;
    [SerializeField] AmmoCounter ammoCounter; // UI中显示子弹数

    public int Remain = 0; // 弹夹剩余子弹 
    bool isReloading;

    private void Awake()
    {
        maxAmmo = GameManager.Instance.CurrentPlayer.ammo;
        maxAmmo -= clipSize;
        Remain = clipSize;
    }

    public bool IsReloading
    {
        get
        {
            return isReloading;
        }
    }

    public void Reload()
    {
        if (IsReloading)
            return;
        isReloading = true;

        /* 取出子弹 */
        int amount = System.Math.Min(maxAmmo, clipSize) - Remain;

        reloadAudio.Play();
        GameManager.Instance.Timer.Add(() =>
        {
            ExecuteReload(amount);
        }, reloadTime);
    }

    void ExecuteReload(int amount)
    {
        isReloading = false;
        Remain += amount;
        maxAmmo -= amount;
    }

    /* 从弹夹里面取一个子弹 */
    public void TakeFromClip(int amount)
    {
        if (Remain >= amount)
        {
            Remain -= amount;
            GameManager.Instance.CurrentPlayer.ammo -= amount;
        }
            
    }

    private void LateUpdate()
    {
        if (ammoCounter == null)
            return;
        ammoCounter.Display(Remain, maxAmmo);
    }

    // 玩家死亡后重置状态
    public void Reset()
    {
        maxAmmo = GameManager.Instance.CurrentPlayer.level * 100;
        GameManager.Instance.CurrentPlayer.ammo = maxAmmo;
        Reload();
    }
}
