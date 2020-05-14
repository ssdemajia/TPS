using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;

namespace Shaoshuai.Core
{
    public class GameManager: BaseManager
    {
        public Transform reticle; // 准星
        public event System.Action<Transform> OnLocalPlayerJoined; 
        public bool IsPause = false;

        public MonoTimer Timer;
        public static GameManager Instance { get;private set;} // 只读
        public InputController inputController;

        Shaoshuai.Message.Player _currentPlayer;
        public Shaoshuai.Message.Player CurrentPlayer
        {
            get
            {
                if (_currentPlayer == null)
                {
                    _currentPlayer = new Shaoshuai.Message.Player() // 初始角色信息
                    {
                        player_name = "ssss",
                        exp = 0,
                        level = 1,
                        ammo = 200,
                        hp = 100
                    };
                }
                return _currentPlayer;
            }
            set
            {
                _currentPlayer = value;
            }
        }
        Player _localPlayer;
        public Player LocalPlayer
        {
            set
            {
                if (_localPlayer == null)
                {
                    _localPlayer = value;
                }
                if (OnLocalPlayerJoined != null)
                    OnLocalPlayerJoined(_localPlayer.transform);
            }
            get
            {
                return _localPlayer;
            }
        }
        private void Awake()
        {
            if (Usersession.Instance != null)
            {
                CurrentPlayer = Usersession.Instance.player;
            }
            Timer = gameObject.AddComponent<MonoTimer>();
            Screen.SetResolution(800, 600, false);
            inputController = gameObject.AddComponent<InputController>();
            Instance = this;
        }

        public static HttpContent ObjToHttpContent<T>(T obj)
        {
            var value = JsonUtility.ToJson(obj);
            HttpContent content = new StringContent(value, Encoding.UTF8, "application/json");
            return content;
        }
    }

}
