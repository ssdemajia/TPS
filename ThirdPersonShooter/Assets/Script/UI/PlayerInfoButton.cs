using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Shaoshuai.Message;
using System;

public class PlayerInfoButton : MonoBehaviour
{
    Button button;
    [SerializeField]Text info;
    Shaoshuai.Message.Player player;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            MainMenu.Instance.CurrentPlayer = player;
            SceneManager.LoadScene("SampleScene");  // 进入游戏模式
        });
    }
    public void setText(Shaoshuai.Message.Player player)
    {
        this.player = player;
        info.text = $"角色名称:{player.player_name} 等级:{player.level}";
    }
}
