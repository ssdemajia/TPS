using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NInputController
{
    System.Timers.Timer _timer;
    FixedVec1 _time; // 当前游戏帧次数* 每帧时间
    public FixedVec1 Time
    {
        get { return _time; }
    }
    NetworkClient _client;
    NKeyboards sendKB; // 需要发送的按键指令
    public Action frameUpdate; // 帧同步每帧调用
    int _serverFrame;  // 服务器帧号
    int _clientFrame;  // 客户端帧号

    NClientInput[] _inputs;

    public NInputController(int maxClient)
    {
        _inputs = new NClientInput[maxClient];
        _timer = new System.Timers.Timer(NetworkClient.deltaTime.ToFloat() * 1000);
        _timer.Elapsed += SendFrame;
        _serverFrame = 0;
        _clientFrame = 0;
        sendKB = new NKeyboards();

        for (int i = 0; i <= maxClient; i++)
        {
            _inputs[i] = new NClientInput(this);
        }
    }

    public void Init(NetworkClient client)
    {
        _client = client;
        _timer.Enabled = true;
    }

    public void ReceiveProtocol(Protocol protocol)
    {
        _serverFrame++;
        int playerCount = protocol.getByte();
        for (int i = 0; i < playerCount; i++)
        {
            _inputs[i].ReceiveFrame(protocol); // 解析所有玩家输入
        }

        for (; _clientFrame < _serverFrame; _clientFrame++)
        {
            if (frameUpdate != null)
            {
                frameUpdate();
                //_client.physics
            }
            this._time += NetworkClient.deltaTime;
        }
    }

    void SendFrame(object sender, System.Timers.ElapsedEventArgs args)
    {
        if (_client.conn.clientID < 0)
            return;

        Protocol protocol = new Protocol();
        protocol.push((byte)MessageType.Frame);
        protocol.push((byte)_client.conn.clientID);
        protocol.push(sendKB.GetByte());

        _client.Send(protocol.GetByteStream());
    }

    public void SetKey(bool down, NKeyboards.KeyNum key)
    {
        sendKB.SetKey(down, key);
    }
}
