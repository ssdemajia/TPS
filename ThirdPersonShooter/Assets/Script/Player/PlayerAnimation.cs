using Shaoshuai.Core;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Crosshair crosshair;
    [SerializeField] float sensitive = 1f;
    InputController inputController;
    Animator animator;
    Transform chest;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        inputController = GameManager.Instance.inputController;
        chest = animator.GetBoneTransform(HumanBodyBones.Spine);
        crosshair = GetComponentInChildren<Crosshair>();
    }

    void Update()
    {
        if (GameManager.Instance.IsPause)
            return;
        animator.SetFloat("Vertical", inputController.Vertical);
        animator.SetFloat("Horizontal", inputController.Vertical);
        animator.SetBool("IsReload", inputController.Reload);
        animator.SetBool("IsShoot", inputController.Fire1);
        animator.SetBool("IsJump", inputController.IsJump);
    }

    private void LateUpdate()
    {
        // 随着十字瞄准改变身躯上下动作
        chest.rotation *= Quaternion.Euler(0, 0, -crosshair.RotationX * sensitive);
    }
}
