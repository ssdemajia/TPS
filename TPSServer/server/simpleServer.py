# -*- coding: GBK -*-

import sys
from server.common import conf
from network.simpleHost import SimpleHost
from dispatcher import Dispatcher
from server.common.packet import Packet
from room import Room
from datetime import datetime
from server.common.message import (
    MessageJoinRoom,
    MessagePlayerInput,
    MessageQuitRoom,
    PlayerInfo
)


class SimpleServer(object):
    def __init__(self):
        super(SimpleServer, self).__init__()
        self.id_counter = 0
        self.last_time = datetime.now()
        self.player_count = 0  # 当前连入玩家数
        self.host = SimpleHost()
        self.dispatcher = Dispatcher()
        self.room = None
        self.id2player_info = {}
        self.id2client = {}

    def tick(self):
        self.host.process()
        return

    def dispatch(self, data, owner):
        """
        消息派发
        :param data: 原始数据
        :param owner: 接收的客户端
        """
        # 当用户异常退出时发送FIN，data的长度为0
        if len(data) == 0:
            return
        p = Packet(data)
        opcode = p.get_int16()

        def str_to_hex(s):
            return ":".join("{:02x}".format(ord(c)) for c in s)
        print(str_to_hex(data))
        # print u'获得opcode:%d' % opcode
        if opcode == conf.MSG_JOIN_ROOM:
            self.player_join_room(opcode, p, owner)
        elif opcode == conf.MSG_PLAYER_INPUT:
            self.player_input(opcode, p, owner)
        elif opcode == conf.MSG_QUIT_ROOM:
            self.player_input(opcode, p, owner)
        else:
            raise Exception(u'bad opcode %d' % opcode)

    def player_join_room(self, opcode, packet, client):
        # Todo 从数据库读取用户信息
        msg = MessageJoinRoom()
        msg.deserialize(packet)
        player_name = msg.name + str(self.id_counter)
        info = PlayerInfo(player_name, self.id_counter, self.id_counter)
        print u'玩家：%s连入' % info.name
        self.id_counter += 1
        self.player_count += 1
        self.id2player_info[info.id] = info
        self.id2client[info.id] = client

        client.player_info = info
        if self.player_count >= Room.max_count:
            self.room = Room()
            for info in self.id2player_info.values():
                client = self.id2client[info.id]
                self.room.add_player(info, client)
            self.room.start_game()

    def player_quit_room(self, opcode, packet, client):
        player_info = client.player_info
        if player_info is None:
            return
        self.player_count -= 1
        player_id = player_info.id
        del self.id2client[player_id]
        del self.id2player_info[player_id]
        if self.player_count == 0:
            self.room = None

    def player_input(self, opcode, packet, client):
        if self.room is None:
            return
        msg = MessagePlayerInput()
        msg.deserialize(packet)
        player_info = client.player_info
        self.room.set_input(player_info, msg)

    def update(self):
        """
        每30ms更新一次，主要用于帧同步
        """
        now = datetime.now()
        delta_time = (now - self.last_time).total_seconds()
        if delta_time < 0.03:
            return

        self.last_time = now

        if self.room is None:
            return
        self.room.update()
