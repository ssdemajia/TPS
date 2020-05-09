using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //[SerializeField] Crosshair crosshair;
    //[SerializeField] float sensitive = 1f;
    //InputController controller;
    //Animator animator;
    //Transform chest;
    //void Awake()
    //{
    //    animator = GetComponentInChildren<Animator>();
    //    controller = GameManager.Instance.InputController;
    //    chest = animator.GetBoneTransform(HumanBodyBones.Spine);
    //}

    //void Update()
    //{
    //    if (GameManager.Instance.IsPause)
    //        return;

    //    animator.SetFloat("Vertical", controller.Vertical);
    //    animator.SetFloat("Horizontal", controller.Horizontal);
    //    animator.SetBool("IsWalk", controller.IsWalking);
    //    animator.SetBool("IsSprint", controller.IsSprinting);
    //    animator.SetBool("IsCrouch", controller.IsCrouching);
    //    animator.SetBool("IsReload", controller.Reload);
    //    animator.SetBool("IsShoot", controller.Fire1);
    //}

    //private void LateUpdate()
    //{
    //    // 随着十字瞄准改变身躯上下动作
    //    chest.rotation *= Quaternion.Euler(0, 0, -crosshair.RotationX * sensitive);
    //}
}
