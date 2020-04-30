using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public void Move(Vector2 direction)
    {
        transform.position += transform.forward * direction.y * Time.deltaTime +
                              transform.right * direction.x * Time.deltaTime;
    }
}
