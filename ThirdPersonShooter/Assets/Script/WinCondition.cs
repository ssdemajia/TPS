using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] Destructable[] targets;
    int targetsDeathCount;

    private void Start()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].OnDeath += WinCondition_OnDeath;
        }
    }

    private void WinCondition_OnDeath()
    {
        targetsDeathCount += 1;
        if (targetsDeathCount == targets.Length)
            GameManager.Instance.EventBus.RaiseEvent("OnWin");
    }
}
