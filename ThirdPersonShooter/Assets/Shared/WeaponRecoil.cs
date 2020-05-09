using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 后座力
[RequireComponent(typeof(Shooter))]
public class WeaponRecoil : MonoBehaviour
{
    //[System.Serializable]
    //public struct Layer
    //{
    //    public AnimationCurve curve;
    //    public Vector3 direction;
    //}

    //[SerializeField]
    //Layer[] layers;

    //[SerializeField]
    //float speed;

    //[SerializeField]
    //float cooldown;

    //[SerializeField]
    //float strength;

    //Shooter shooter;
    //Shooter Shooter
    //{
    //    get
    //    {
    //        if (shooter == null)
    //        {
    //            shooter = GetComponent<Shooter>();
    //        }
    //        return shooter;
    //    }
    //}

    //Crosshair crosshair;
    //Crosshair Crosshair
    //{
    //    get
    //    {
    //        if (crosshair == null)
    //        {
    //            crosshair = GameManager.Instance.LocalPlayer.crosshair;
    //        }
    //        return crosshair;
    //    }
    //}
    //float nextCooldown;
    //float recoilActiveTime;

    //public void Activate()
    //{
    //    nextCooldown = Time.time + cooldown;
    //}

    //private void Update()
    //{
    //    if (nextCooldown > Time.time)
    //    {
    //        recoilActiveTime += Time.deltaTime;
    //        float percentage = getPercentage();

    //        Vector3 recoilAmount = Vector3.zero;
    //        for (int i = 0; i < layers.Length; i++)
    //        {
    //            recoilAmount += layers[i].direction * layers[i].curve.Evaluate(percentage);
    //        }
    //        Shooter.AimTargetOffset = Vector3.Lerp(Shooter.AimTargetOffset, 
    //            recoilAmount + Shooter.AimTargetOffset, strength * Time.deltaTime);
    //        Crosshair.ApplyScale(percentage * Random.Range(strength*7, strength*9));
    //    }
    //    else
    //    {
    //        recoilActiveTime -= Time.deltaTime;
            
    //        if (recoilActiveTime < 0)
    //            recoilActiveTime = 0;

    //        if (recoilActiveTime == 0)
    //        {
    //            Shooter.AimTargetOffset = Vector3.zero;
    //        }
    //        Crosshair.ApplyScale(getPercentage());
    //    }

    //}

    //float getPercentage()
    //{
    //    return Mathf.Clamp01(recoilActiveTime / speed);
    //}
}
