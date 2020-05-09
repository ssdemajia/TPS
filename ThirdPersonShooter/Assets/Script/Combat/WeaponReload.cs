using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    //[SerializeField] int maxAmmo = 400;
    //[SerializeField] float reloadTime = 3f;
    //[SerializeField] int clipSize = 40; // 弹夹中的子弹数量
    //[SerializeField] int ammo = 0;
    //[SerializeField] AudioController reloadAudio;

    //public int Remain = 0; // 弹夹剩余子弹 
    //bool isReloading;
    //private void Awake()
    //{
    //    Remain = clipSize;
    //}
    //public int RemainAmmo
    //{
    //    get
    //    {
    //        return maxAmmo - ammo;
    //    }
    //}
    //public bool IsReloading
    //{
    //    get
    //    {
    //        return isReloading;
    //    }
    //}

    //public void Reload()
    //{
    //    if (IsReloading)
    //        return;
    //    isReloading = true;
    //    /* 取出子弹 */
    //    int amount = System.Math.Min(maxAmmo - ammo, clipSize) - Remain;
    //    reloadAudio.Play();
    //    GameManager.Instance.Timer.Add(()=> {
    //        ExecuteReload(amount);
    //        }
    //    , reloadTime);
    //}

    //void ExecuteReload(int amount)
    //{
    //    isReloading = false;
    //    ammo += amount;
    //    Remain += amount;
    //}

    ///* 从弹夹里面取一个子弹 */
    //public void TakeFromClip(int amount)
    //{
    //    if (Remain >= amount)
    //        Remain -= amount;
    //}

}
