using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 保存用户相关信息，用于secene的切换
public class Usersession : MonoBehaviour
{
    public Shaoshuai.Message.Player player;
    public string username;
    public string password;
    public static Usersession Instance { get; set; }
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public static void SetUser(string username, string password)
    {
        Instance.username = username;
        Instance.password = password;
    }

    public static void SetPlayer(Shaoshuai.Message.Player player)
    {
        Instance.player = player;
    }
}
