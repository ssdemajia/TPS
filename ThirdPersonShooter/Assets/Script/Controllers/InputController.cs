using System.Collections;
using System.Collections.Generic;
using Shaoshuai;
using Shaoshuai.Core;
using Shaoshuai.Message;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float Vertical;
    public float Horizontal;
    public FixedVec2 MouseInput;
    public bool Fire1;
    public bool Reload;
    public bool IsWalking;
    public bool IsSprinting;
    public bool IsCrouching;
    public bool IsJump;
    public bool Escape;

    private void Update()
    {
        Vertical = Input.GetAxis("Vertical");
        Horizontal = Input.GetAxis("Horizontal");
        FixedVec2 inputHV = new FixedVec2(Vertical, Horizontal);
        MouseInput = new FixedVec2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Fire1 = Input.GetButton("Fire1");
        Reload = Input.GetKey(KeyCode.R);
        Escape = Input.GetKey(KeyCode.Escape);
        IsWalking = Input.GetKey(KeyCode.LeftShift);
        IsCrouching = Input.GetKey(KeyCode.C);
        IsSprinting = Input.GetKey(KeyCode.LeftAlt);
        IsJump = Input.GetKey(KeyCode.Space);

        GameManager.CurrentInput = new PlayerInput()
        {
            mousePos = MouseInput,
            inputHV = inputHV,
            fire1 = Fire1,
            reload = Reload,
            isJump = IsJump
        };

    }
}
