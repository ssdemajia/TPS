# -*- coding: GBK -*-

import time
from server.common import conf
import socket
from netStream import NetStream


class SimpleHost(object):

    def __init__(self, timeout=conf.NET_HOST_DEFAULT_TIMEOUT):
        super(SimpleHost, self).__init__()

        self.host = 0
        self.state = conf.NET_STATE_STOP
        self.clients = []
        self.index = 1
        self.queue = []
        self.count = 0
        self.sock = None
        self.port = 0
        self.timeout = timeout

        return

    def generate_id(self):
        pos = -1
        for i in xrange(len(self.clients)):
            if self.clients[i] is None:
                pos = i
                break
        if pos < 0:
            pos = len(self.clients)
            self.clients.append(None)

        hid = (pos & conf.MAX_HOST_CLIENTS_INDEX) | (self.index << conf.MAX_HOST_CLIENTS_BYTES)

        self.index += 1
        if self.index >= conf.MAX_HOST_CLIENTS_INDEX:
            self.index = 1

        return hid, pos

    # read event
    def read(self):
        if len(self.queue) == 0:
            return -1, 0, ''  # type, hid, data
        event = self.queue[0]
        self.queue = self.queue[1:]
        return event

    # shutdown service
    def shutdown(self):
        if self.sock:
            try:
                self.sock.close()
            except:
                pass  # should logging here
        self.sock = None

        self.index = 1
        for client in self.clients:
            if not client:
                continue
            try:
                client.close()
            except:
                pass  # should logging here

        self.clients = []

        self.queue = []
        self.state = conf.NET_STATE_STOP
        self.count = 0

        return

    # 启动监听
    def startup(self, port=0):
        self.shutdown()

        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        try:
            self.sock.bind(('0.0.0.0', port))
        except:
            try:
                self.sock.close()
            except:
                pass  # should logging here
            return -1

        self.sock.listen(conf.MAX_HOST_CLIENTS_INDEX + 1)
        self.sock.setblocking(False)
        self.port = self.sock.getsockname()[1]
        self.state = conf.NET_STATE_ESTABLISHED
        print(u'服务器启动，监听端口%d' % self.port)
        return 0

    def get_client(self, hid):
        pos = hid & conf.MAX_HOST_CLIENTS_INDEX
        if (pos < 0) or (pos >= len(self.clients)):
            return -1, None

        client = self.clients[pos]
        if client is None:
            return -2, None
        if client.hid != hid:
            return -3, None

        return 0, client

    # close client
    def close_client(self, hid):
        code, client = self.get_client(hid)
        if code < 0:
            return code

        client.close()
        return 0

    # send data to a certain client
    def send_client(self, hid, data):
        code, client = self.get_client(hid)
        if code < 0:
            return code

        client.send(data)
        client.process()
        return 0

    def client_nodelay(self, hid, nodelay=0):
        code, client = self.get_client(hid)
        if code < 0:
            return code

        return client.nodelay(nodelay)

    def handle_new_client(self, current):
        """

        :param current: 当前时间
        :return:
        """
        sock = None
        try:
            sock, remote = self.sock.accept()
            sock.setblocking(0)
        except:
            pass  # log something

        if self.count > conf.MAX_HOST_CLIENTS_INDEX:
            try:
                sock.close()
            except:
                pass  # log something
            sock = None

        if not sock:
            return

        hid, pos = self.generate_id()
        client = NetStream()
        client.assign(sock)
        client.hid = hid
        client.active = current
        client.peername = sock.getpeername()
        self.clients[pos] = client
        self.count += 1
        self.queue.append((conf.NET_CONNECTION_NEW, hid, repr(client.peername)))

        return

    def update_clients(self, current):
        for pos in xrange(len(self.clients)):
            client = self.clients[pos]
            if not client:
                continue

            client.process()
            while client.status() == conf.NET_STATE_ESTABLISHED:  # 接收数据
                data = client.recv()
                if data == '':
                    break
                self.queue.append((conf.NET_CONNECTION_DATA, client.hid, data))
                client.active = current  # 更新时间

            timeout = current - client.active
            if (client.status() == conf.NET_STATE_STOP) or (timeout >= self.timeout):
                self.queue.append((conf.NET_CONNECTION_LEAVE, client.hid, ''))
                self.clients[pos] = None
                client.close()
                del client
                self.count -= 1

        return

    # 处理客户端信息以及接收新的连接请求
    def process(self):
        current = time.time()
        if self.state != conf.NET_STATE_ESTABLISHED:
            return 0

        self.handle_new_client(current)
        self.update_clients(current)

        return 0
