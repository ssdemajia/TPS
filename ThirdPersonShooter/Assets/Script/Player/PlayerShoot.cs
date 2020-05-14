using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shaoshuai.Core;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] Shooter gun;
    private void Update()
    {
        if (GameManager.Instance.inputController.Fire1)
        {
            gun.Fire();
        }
    }
}
