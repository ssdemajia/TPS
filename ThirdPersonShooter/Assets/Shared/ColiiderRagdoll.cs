using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiiderRagdoll : MonoBehaviour
{
    public Destructable health; // NPC的health

    void Start()
    {
        health = gameObject.GetComponent<Destructable>();
        travel(health.transform);
    }

    // 遍历身体，给ragdoll增加对应的扣血
    void travel(Transform t)
    {
        var collider = t.GetComponent<CapsuleCollider>();
        if (collider != null)
        {
            t.gameObject.AddComponent<Destructable>();
            t.GetComponent<Destructable>().parent = health;
        }
        for (int i = 0; i < t.childCount; i++)
        {
            travel(t.GetChild(i));
        }
    }
}
