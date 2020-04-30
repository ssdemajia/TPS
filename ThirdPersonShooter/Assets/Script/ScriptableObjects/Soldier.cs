using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Soldier", menuName ="Data/Soldier")]
public class Soldier : ScriptableObject
{
    public float jumpSpeed = 10f;
    public float runSpeed = 10f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 15f;
    public float crouchSpeed = 5f;
}
