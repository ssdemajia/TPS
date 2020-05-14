using UnityEngine;

public class InputController : MonoBehaviour
{
    public float Horizontal;
    public float Vertical;
    public float MouseInputX;
    public float MouseInputY;

    public bool Fire1;
    public bool Reload;
    public bool IsWalking;
    public bool IsSprinting;
    public bool IsCrouching;
    public bool IsJump;
    public bool Escape;

    // 更新当前玩家输入
    private void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        MouseInputX = Input.GetAxisRaw("Mouse X");
        MouseInputY = Input.GetAxisRaw("Mouse Y");

        Fire1 = Input.GetButton("Fire1");
        Reload = Input.GetKey(KeyCode.R);
        Escape = Input.GetKey(KeyCode.Escape);
        IsWalking = Input.GetKey(KeyCode.LeftShift);
        IsCrouching = Input.GetKey(KeyCode.C);
        IsSprinting = Input.GetKey(KeyCode.LeftAlt);
        IsJump = Input.GetKey(KeyCode.Space);
    }
}
