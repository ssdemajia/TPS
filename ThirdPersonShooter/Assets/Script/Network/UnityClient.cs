using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    public NetworkClient _client;
    public string ip;
    public int port;

    InputController input;
    InputController InputController
    {
        get
        {
            if (input == null)
                input = GameManager.Instance.InputController;
            return input;
        }
    }
    private void Awake()
    {
        _client = new NetworkClient();
        _client.Connect(ip, port);
    }

    private void Update()
    {
        _client.ParseMassage();
        CommitKey();
    }

    private void CommitKey()
    {
        _client.inputController.SetKey(Input.GetKey(KeyCode.W), NKeyboards.KeyNum.Up);
        _client.inputController.SetKey(Input.GetKey(KeyCode.S), NKeyboards.KeyNum.Down);
        _client.inputController.SetKey(Input.GetKey(KeyCode.A), NKeyboards.KeyNum.Left);
        _client.inputController.SetKey(Input.GetKey(KeyCode.D), NKeyboards.KeyNum.Right);
    }
}
