using Shaoshuai.Core;
using Shaoshuai.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

namespace Shaoshuai.Network
{
    public class NetworkClient
    {
        /// <summary>
        /// 帧同步时间间隔
        /// </summary>
        public readonly static FixedVec1 deltaTime = new FixedVec1(0.1f);
        public Connection conn;
        public Queue<Packet> MessageQ;

        public NetworkClient(int maxClient)
        {
            conn = new Connection();
            conn.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            conn.socket.NoDelay = true;
            MessageQ = new Queue<Packet>();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">对应端口</param>
        public void Connect(string ip, int port)
        {
            conn.socket.Connect(ip, port);
            // 开始接收服务端发送的数据包
            conn.socket.BeginReceive(conn.readBuff, 0, conn.BufferRemain, SocketFlags.None, RecvCallback, conn);
        }

        void RecvCallback(IAsyncResult ar)
        {
            Connection conn = (Connection)ar.AsyncState; // 接收服务器发来信息
            int length = conn.socket.EndReceive(ar);
            //Debug.Log("Receive:" + length);
            conn.recvLen += length;
            RecvToProto(conn);
            conn.socket.BeginReceive(conn.readBuff, conn.recvLen, conn.BufferRemain, SocketFlags.None, RecvCallback, conn);
        }

        void RecvToProto(Connection conn)
        {
            Array.Copy(conn.readBuff, conn.lenBytes, sizeof(Int32)); // 读取4字节头部长度
            
            conn.msgLen = BitConverter.ToInt32(conn.lenBytes, 0);
            Debug.Log("接收msg的长度:" + conn.msgLen);
            if (conn.msgLen > conn.recvLen)
            {
                Debug.Log("数据包长度不一致");
                return;
            }
            Packet p = new Packet();
            p.InitMsg(conn.RecvBytes);
            MessageQ.Enqueue(p);
            int count = conn.recvLen - conn.msgLen; // 剩余内容长度
            Array.Copy(conn.readBuff, conn.msgLen, conn.readBuff, 0, count);
            conn.recvLen = count;

            // 当buff中还有没有读完的数据时  
            if (count > 0)
            {
                RecvToProto(conn);
            }
        }

        public void Send(byte[] bytes)
        {
            byte[] length = BitConverter.GetBytes(bytes.Length);
            byte[] buffer = length.Concat(bytes).ToArray();
            conn.socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, null, null);
        }

        /// <summary>
        /// 解析接收到的消息包
        /// </summary>
        public void ParseMassage()
        {
            if (this.MessageQ.Count == 0)
                return;
            Packet packet = this.MessageQ.Dequeue();
            MessageType msgType = (MessageType)packet.getInt16();
            Debug.Log("接收消息包类型:" + msgType);
            switch (msgType)
            {
                case MessageType.FrameInput:
                    var inputMsg = new MessageFrameInput();
                    inputMsg.Deserialize(packet);
                    GameManager.PushInputFrame(inputMsg.input);
                    break;
                case MessageType.StartGame:
                    var startMsg = new MessageStartGame();
                    startMsg.Deserialize(packet);
                    GameManager.StartGame(startMsg);
                    break;
                default:
                    Debug.LogError("无法解析的消息类型:" + msgType);
                    return;
            }
        }

        public void Destroy()
        {
            conn.Destroy();
        }
    }

}
