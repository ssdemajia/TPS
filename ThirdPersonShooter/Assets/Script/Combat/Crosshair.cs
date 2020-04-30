using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    //[SerializeField] Texture2D image;
    //[SerializeField] int size;
    //[SerializeField] float maxAngle;
    //[SerializeField] float minAngle;
    public float RotationX;
    //private void OnGUI()
    //{
    //    // 得到准星在屏幕空间的坐标
    //    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
    //    screenPos.y = Screen.height - screenPos.y;
    //    // 校正准星位置
    //    GUI.DrawTexture(new Rect(screenPos.x - size/2, screenPos.y - size/2, size, size), image);
    //}

    internal void SetRotation(float v)
    {
        RotationX += v;
    }
    [SerializeField] float speed;

    public Transform Reticle;
    Transform crossTop;
    Transform crossBottom;
    Transform crossLeft;
    Transform crossRight;

    float reticleStartPoint;

    private void Start()
    {
        crossTop = Reticle.Find("top").transform;
        crossBottom = Reticle.Find("bottom").transform;
        crossLeft = Reticle.Find("left").transform;
        crossRight = Reticle.Find("right").transform;
        reticleStartPoint = crossTop.localPosition.y;
    }

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Reticle.transform.position = Vector3.Lerp(Reticle.transform.position, screenPos, speed * Time.deltaTime);
    }

    public void ApplyScale(float scale)
    {
        crossTop.localPosition = new Vector3(0, reticleStartPoint + scale, 0);
        crossBottom.localPosition = new Vector3(0, -reticleStartPoint - scale, 0);
        crossLeft.localPosition = new Vector3(-reticleStartPoint - scale, 0, 0);
        crossRight.localPosition = new Vector3(reticleStartPoint + scale, 0, 0);
    }

    void SetVisibility(bool value)
    {
        Reticle.gameObject.SetActive(value);
    }
}
