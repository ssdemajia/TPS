# coding:utf-8
from server.common.message import (
    MessageStartGame,
    MessageFrameInput,
    FrameInput
)
from server.common.packet import Packet


class Room:
    max_count = 1

    def __init__(self):
        self.player_infos = []
        self.clients = []
        self.player_inputs = []
        self.current_tick = 0
        self.recv_count = 0
        self.id2player_input = {}  # 玩家id与玩家输入的映射
        self.tick2player_inputs = []  # 每帧对应的所有玩家输入

    def add_player(self, player_info, client):
        self.player_infos.append(player_info)
        self.clients.append(client)

    def update(self):
        """
        每隔一段时间调用
        :return:
        """
        self.check_inputs()

    def check_inputs(self):
        if self.recv_count < self.max_count:
            return
        self.inputs_broadcast(self.current_tick)
        self.current_tick += 1

    def inputs_broadcast(self, tick):
        """
        广播玩家的输入
        :param tick: 当前tick
        :return:
        """
        msg = MessageFrameInput(FrameInput(tick, self.player_inputs))
        p = Packet()
        msg.serialize(p)
        for i in range(self.max_count):
            client = self.clients[i]
            client.send(p.byte_list)

    def start_game(self):
        print '房间开始游戏'
        p = Packet()
        msg = MessageStartGame(map_id=0, player_infos=self.player_infos)
        msg.serialize(p)
        for i in range(self.max_count):
            client = self.clients[i]
            msg.local_player_id = i
            client.send(p.byte_list)

    def set_input(self, player_info, message):
        """
        保存玩家发来的输入
        :param player_info:
        :param message:
        :return:
        """
        print '接收玩家输入id:%d' % player_info.local_id
        local_id = player_info.local_id
        if len(self.tick2player_inputs) >= self.current_tick:
            inputs = [None for i in range(self.max_count)]
            self.tick2player_inputs.append(inputs)
        else:
            inputs = self.tick2player_inputs[self.current_tick]
        inputs[local_id] = message.player_input
        self.check_inputs()

