using System;
using System.Net.Sockets;

public class Connection
{
    public int clientID = -1;
    public readonly static int BUFFER_SIZE = 1024;
    public byte[] readBuff = new byte[BUFFER_SIZE];
    public byte[] lenBytes = new byte[4];
    public int msgLen = 0; // 消息长度
    public Socket socket;
    public int recvLen = 0;
    byte[] buffer; // 接收缓存
    public int BufferRemain
    {
        get
        {
            return BUFFER_SIZE - recvLen;
        }
    }
    public byte[] RecvBytes
    {
        get
        {
            buffer = new byte[msgLen];
            Array.Copy(readBuff, buffer, msgLen);
            return buffer;
        }
    }
}
