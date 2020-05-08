# -*- coding: GBK -*-

import sys
from server.common import conf
from network.simpleHost import SimpleHost
from dispatcher import Dispatcher
from server.common.packet import Packet
from room import Room
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
        self.player_count = 0  # 当前连入玩家数
        self.entities = {}
        self.host = SimpleHost()
        self.dispatcher = Dispatcher()
        self.room = None
        self.id2player_info = {}
        self.id2client = {}

    def generate_entity_id(self):
        raise NotImplementedError

    def register_entity(self, entity):
        eid = self.generate_entity_id
        entity.id = eid
        self.entities[eid] = entity
        return

    def tick(self):
        self.host.process()

        for eid, entity in self.entities.iteritems():
            # Note: you can not delete entity in tick.
            # you may cache delete items and delete in next frame
            # or just use items.
            entity.tick()

        return

    def dispatch(self, data, owner):
        """
        消息派发
        :param data: 原始数据
        :param owner: 接收的客户端
        """
        p = Packet(data)
        opcode = p.get_int16()

        if opcode == conf.MSG_JOIN_ROOM:
            self.player_join_room(opcode, p, owner)
        elif opcode == conf.MSG_PLAYER_INPUT:
            self.player_input(opcode, p, owner)
        elif opcode == conf.MSG_QUIT_ROOM:
            self.player_input(opcode, p, owner)
        else:
            raise Exception('bad opcode %d' % opcode)

    def player_join_room(self, opcode, packet, client):
        # Todo 从数据库读取用户信息
        msg = MessageJoinRoom()
        msg.deserialize(packet)
        player_name = msg.name + str(self.id_counter)
        info = PlayerInfo(player_name, self.id_counter)
        self.id_counter += 1
        self.player_count += 1
        self.id2player_info[info.id] = info
        self.id2client[info.id] = client

        client.player_info = info
        if self.player_count >= 2:
            self.room = Room()
            for info in self.id2player_info.values():
                client = self.id2client[info.id]
                self.room.add_player(info, client)

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
        pass