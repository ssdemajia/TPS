using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] float rateOfFire;
    [SerializeField] Projectile projectile; // 子弹
    [SerializeField] Transform muzzle; // 子弹出发的地址
    [SerializeField] AudioController audio; // 射击声


    private int shootableMask; // 可射击
    public Transform AimTarget;
    public Vector3 AimTargetOffset;

    WeaponReload reloader;
    float nextFireAllowed;
    private ParticleSystem muzzleFireParticle;
    protected bool canFire;

    WeaponRecoil weapon; // 后座力
    public WeaponRecoil WeaponRecoil
    {
        get
        {
            if (weapon == null)
                weapon = GetComponent<WeaponRecoil>();
            return weapon;
        }
    }


    private void Awake()
    {
        reloader = GetComponent<WeaponReload>();
        muzzleFireParticle = muzzle.GetComponent<ParticleSystem>();
    }

    protected void Reload()
    {
        if (reloader)
            reloader.Reload();
    }

    void FireEffect()
    {
        if (muzzleFireParticle == null)
            return;
        muzzleFireParticle.Play();
    }

    public virtual void Fire()
    {
        canFire = false;

        if (Time.time < nextFireAllowed)
            return;

        if (reloader != null)
        {
            if (reloader.IsReloading) // 正在装弹
                return;
            if (reloader.Remain == 0) // 弹夹里面没子弹了
                return;
            reloader.TakeFromClip(1);
        }
        nextFireAllowed = Time.time + rateOfFire;

        bool isLocalPlayerControlled = AimTarget == null;

        // 敌人，AimTarget是玩家
        if (!isLocalPlayerControlled)
        {
            muzzle.LookAt(AimTarget.position + AimTargetOffset);
        }

        var bullet = Instantiate(projectile, muzzle.position, muzzle.rotation);
        // 玩家控制
        if (isLocalPlayerControlled)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0)); // 摄像头指向屏幕中心
            RaycastHit hit;
            Vector3 targetPos = ray.GetPoint(500); // 射线500米外位置
            if (Physics.Raycast(ray, out hit))  // 如果碰到物体
            {
                targetPos = hit.point;
            }
            bullet.transform.LookAt(targetPos + AimTargetOffset);
        }

        if (WeaponRecoil != null)
            WeaponRecoil.Activate();

        canFire = true;
        audio.Play();
        FireEffect();
    }
}
