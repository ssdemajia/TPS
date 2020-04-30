using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Linq;

public class NetworkClient
{
    /// <summary>
    /// 帧同步时间间隔
    /// </summary>
    public readonly static FixedVec1 deltaTime = new FixedVec1(0.1f);
    public Connection conn;
    public NInputController inputController;
    public Queue<Protocol> MessageQ;

    public NetworkClient()
    {
        conn = new Connection();
        conn.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        conn.socket.NoDelay = true;

        //inputController = new NInputController();
    }
    public void Connect(string ip, int port)
    {
        conn.socket.Connect(ip, port);
        conn.socket.BeginReceive(conn.readBuff, 0, conn.BufferRemain, SocketFlags.None, RecvCallback, conn);
    }

    void RecvCallback(IAsyncResult ar)
    {
        Connection conn = (Connection)ar.AsyncState;
        int length = conn.socket.EndReceive(ar);
        Debug.Log("Receive:" + length);
        conn.recvLen += length;
        RecvToProto(conn);
        conn.socket.BeginReceive(conn.readBuff, 0, conn.BufferRemain, SocketFlags.None, RecvCallback, conn);
    }

    void RecvToProto(Connection conn)
    {
        Array.Copy(conn.readBuff, conn.lenBytes, sizeof(Int32));
        conn.msgLen = BitConverter.ToInt32(conn.lenBytes, 0);
        if (conn.msgLen + sizeof(Int32) > conn.recvLen)
        {
            Debug.Log("数据包长度不一致");
            return;
        }
        Protocol protocol = new Protocol();
        protocol.InitMsg(conn.RecvBytes);
        MessageQ.Enqueue(protocol);
        int count = conn.recvLen - conn.msgLen - sizeof(Int32);
        Debug.Log("RecvTo Proto:" + count);
    }

    public void ParseProto(Protocol protocol)
    {
        MessageType msgType = (MessageType)protocol.getByte();
        switch(msgType)
        {
            case MessageType.Init:
                conn.clientID = protocol.getByte();
                break;
            case MessageType.Frame:
                inputController.ReceiveProtocol(protocol);
                break;
            default:
                Debug.LogError("无法解析的消息类型:" + msgType);
                return;
        }
    }

    public void Send(byte[] bytes)
    {
        byte[] length = BitConverter.GetBytes(bytes.Length);
        byte[] buffer = length.Concat(bytes).ToArray();
        conn.socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, null, null);
    }

    public void ParseMassage()
    {

    }

    public bool IsLocalId(int id)
    {
        return id == conn.clientID;
    }
}
