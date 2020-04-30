using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping = new Vector2(4.0f, 0f);     // 阻尼
        public Vector2 Sensitivity = new Vector2(4.0f, 0f); // 灵敏度
        public bool LockMouse = true;
    }
    [SerializeField] float gravity = 30f;
    [SerializeField] Soldier soldier;
    [SerializeField] MouseInput MouseControl;

    PlayerHealth health;
    PlayerHealth Health
    {
        get
        {
            if (health == null)
                health = GetComponent<PlayerHealth>();
            return health;
        }
    }
    InputController inputController;
    CharacterController moveController;
    Vector3 moveDirection = Vector3.zero;

    Animator animator;
    Vector2 mouseInput;
    public Crosshair crosshair;
    Transform lookAtTarget;
    private void Awake()
    {
        inputController = GameManager.Instance.InputController;
        moveController = GetComponent<CharacterController>();
        GameManager.Instance.LocalPlayer = this;
        crosshair = GetComponentInChildren<Crosshair>();
        animator = GetComponentInChildren<Animator>();
        lookAtTarget = transform.Find("CameraLookTarget");
        if (MouseControl.LockMouse)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsPause)
            return;
        if (!Health.IsAlive)
            return;
        Move();
        LookAround();
    }

    private void LookAround()
    {
        mouseInput.x = Mathf.Lerp(mouseInput.x, inputController.MouseInput.x, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, inputController.MouseInput.y, 1f / MouseControl.Damping.y);
        // 更新玩家角度
        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
        float currentAngle = lookAtTarget.transform.eulerAngles.x - mouseInput.y * MouseControl.Sensitivity.y;
        if (currentAngle > 40 && currentAngle < 300)
            return;
        lookAtTarget.transform.RotateAround(transform.position, transform.right, -mouseInput.y * MouseControl.Sensitivity.y);
        crosshair.SetRotation(-mouseInput.y * MouseControl.Sensitivity.y);
    }

    private void Move()
    {
        float speed = soldier.runSpeed;

        if (inputController.IsWalking)
            speed = soldier.walkSpeed;
        if (inputController.IsSprinting)
            speed = soldier.sprintSpeed;
        if (inputController.IsCrouching)
            speed = soldier.crouchSpeed;

        
        if (moveController.isGrounded)
        {
            moveDirection = inputController.Horizontal * transform.right * speed +
                        inputController.Vertical * transform.forward * speed;
            if (inputController.IsJump)
            {
                moveDirection.y = soldier.jumpSpeed;
                animator.SetBool("IsJump", true);
            } else
            {
                animator.SetBool("IsJump", false);
            }

        } else
            moveDirection.y -= gravity * Time.deltaTime;

        // 更新玩家位置
        moveController.Move(moveDirection * Time.deltaTime);
    }
}
