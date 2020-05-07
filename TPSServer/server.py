# coding=utf-8
import socket
import threading
from packet import Packet
import msg_type
import struct


def str_to_hex(s):
    return ":".join("{:02x}".format(ord(c)) for c in s)


class Client:
    def __init__(self, sock, client_id, thread):
        self.name = ''
        self.user_id = 0
        self.initPos = []
        self.initDeg = []
        self.sock = sock
        self.id = client_id  # 战斗id值
        self.thread = thread
        self.msg = ""
        self.frame = ""

    def send_init(self):
        client_id = self.id
        print'玩家[%d]->初始化报文' % client_id
        p = Packet()
        p.push_byte(msg_type.INIT)
        p.push_byte(client_id)
        self.send_frame(p.byte_list)

    def send_frame(self, frame):
        """
        msg = frame_len + frame
        """
        p = Packet()
        p.push_int32(len(frame))
        p.push_bytes(frame)
        # print'发送玩家[%d]->帧，内容：%s' % (self.id, str_to_hex(frame))
        self.sock.send(p.byte_list)

    def recv_frame(self):
        raw_msg = self.sock.recv(4)
        print'<-玩家[%d]帧信息，长度:%s' % (self.id, raw_msg)
        if len(raw_msg) == 0:
            print'玩家[%d]退出' % self.id
            self.sock.close()
            return False
        msg_len = struct.unpack('i', raw_msg)[0]
        self.msg = self.sock.recv(msg_len)
        print'<-玩家[%d]帧信息，内容:%s' % (self.id, self.msg)
        return True


class TPSServer:

    def __init__(self, ip='127.0.0.1', port=9999, timer_interval=0.1):
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.setsockopt(socket.IPPROTO_TCP, socket.TCP_NODELAY, 1)
        self.sock.bind((ip, port))
        self.sock.listen(5)
        self.timer_interval = timer_interval  # 逻辑帧时间间隔
        self.client_id = 0
        self.clients = {}
        self.frames = []

    def start(self):
        print'启动帧同步计时器'
        threading.Timer(self.timer_interval, self.timer_func).start()
        print'服务器开始接收信息'
        while True:
            conn, address = self.sock.accept()

            client_id = self.client_id + 1
            t = threading.Thread(target=self.accept_handle, args=(client_id,))
            client = Client(conn, client_id, t)
            self.clients[client_id] = client
            print'<-新连接：%s，玩家[%d]' % (address, client_id)
            t.start()

    def accept_handle(self, client_id):
        client = self.clients[client_id]
        client.send_init()
        self.send_all_frames(client_id)
        self.recv_handle(client_id)

    def recv_handle(self, client_id):
        client = self.clients[client_id]
        while client.recv_frame():
            self.process_frame(client)
        del self.clients[client_id]

    def process_frame(self, client):
        msg = client.msg
        p = Packet(msg)
        frame_type = p.get_byte()
        frame = p.get_remain()
        if frame_type == msg_type.FRAME:
            client.frame = frame
        elif frame_type == msg_type.LOGIN:
            pass

    def send_all_frames(self, client_id):
        if len(self.frames) == 0:
            return
        print'玩家[%d]中途加入，->所有帧' % client_id
        client = self.clients[client_id]
        for frame in self.frames:
            client.send_frame(frame)

    def timer_func(self):
        """
        定时发送帧信息到客户端
        """
        # print('玩家数[%d],' % len(self.clients))
        p = Packet()
        p.push_byte(msg_type.FRAME)
        length = len(self.clients)
        p.push_byte(length)
        for client_id, client in self.clients.items():
            p.push_byte(client_id)
            p.push_bytes(client.frame)

        frame_to_send = p.byte_list
        self.frames.append(frame_to_send)
        for client in self.clients.values():
            client.send_frame(frame_to_send)

        # print('帧同步[%d]' % len(self.frames))
        threading.Timer(self.timer_interval, self.timer_func).start()


if __name__ == '__main__':
    server = TPSServer()
    server.start()
