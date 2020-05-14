using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shaoshuai.Core;
using Shaoshuai.UI;
using System;

[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping = new Vector2(4.0f, 1f);     // 阻尼
        public Vector2 Sensitivity = new Vector2(4.0f, 2f); // 灵敏度
        public bool LockMouse = false;
    }
    [SerializeField] float gravity = 30f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float runSpeed = 10f;
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
        GameManager.Instance.LocalPlayer = this;
        MouseControl = new MouseInput();
        inputController = GameManager.Instance.inputController;
        moveController = GetComponent<CharacterController>();
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
        mouseInput.x = Mathf.Lerp(mouseInput.x, inputController.MouseInputX, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, inputController.MouseInputY, 1f / MouseControl.Damping.y);
        // 更新玩家角度
        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
        float currentAngle = lookAtTarget.transform.eulerAngles.x - mouseInput.y * MouseControl.Sensitivity.y;
        if (currentAngle > 20 && currentAngle < 330)
            return;
        lookAtTarget.transform.RotateAround(transform.position, transform.right, -mouseInput.y * MouseControl.Sensitivity.y);
        crosshair.SetRotation(-mouseInput.y * MouseControl.Sensitivity.y);
    }

    private void Move()
    {

        if (moveController.isGrounded)
        {
            moveDirection = inputController.Horizontal * transform.right * runSpeed +
                        inputController.Vertical * transform.forward * runSpeed;
            if (inputController.IsJump)
            {
                moveDirection.y = jumpSpeed;
                animator.SetBool("IsJump", true);
            }
            else
            {
                animator.SetBool("IsJump", false);
            }

        }
        else
            moveDirection.y -= gravity * Time.deltaTime;

        // 更新玩家位置
        moveController.Move(moveDirection * Time.deltaTime);
    }
}
