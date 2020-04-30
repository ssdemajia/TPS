using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] Shooter gun;
    private void Update()
    {
        if (GameManager.Instance.InputController.Fire1)
        {
            gun.Fire();
        }
    }
}
