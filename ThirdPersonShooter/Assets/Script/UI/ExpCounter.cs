﻿using Shaoshuai.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpCounter : MonoBehaviour
{
    [SerializeField] Text text;
    int exp = 0;
    int maxExp = 0;
    int level = 1;
    private void Start()
    {
        level = GameManager.Instance.CurrentPlayer.level;
        exp = GameManager.Instance.CurrentPlayer.exp;
        maxExp = GameManager.Instance.CurrentPlayer.level * 100;
    }

    public void Kill()
    {
        exp += 20;
        if (exp > maxExp)
        {
            level += 1;
            maxExp = GameManager.Instance.CurrentPlayer.level * 100;
            exp = 0;
        }
    }

    // 显示经验数
    private void LateUpdate()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CurrentPlayer.level = level;
        }
        text.text = $"经验:{exp}/{maxExp} 等级:{level}";
    }
}