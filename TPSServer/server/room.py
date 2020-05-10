# coding:utf-8
from server.common.message import (
    MessageStartGame,
    MessageFrameInput,
    FrameInput
)
from server.common.packet import Packet


class Room:
    max_count = 2

    def __init__(self):
        self.counter = 0
        self.player_infos = []
        self.clients = []
        self.player_inputs = []
        self.current_tick = 0
        self.recv_count = 0
        self.id2player_input = {}  # 玩家id与玩家输入的映射
        self.tick2player_inputs = []  # 每帧对应的所有玩家输入

    def add_player(self, player_info, client):
        player_info.local_id = self.counter
        self.counter += 1
        self.player_infos.append(player_info)
        self.clients.append(client)

    def update(self):
        """
        每隔一段时间调用
        :return:
        """
        self.check_inputs()

    def check_inputs(self):
        if self.current_tick >= len(self.tick2player_inputs):
            return
        is_full_input = True
        # 当玩家的输入在current_tick当前帧满足时，进行广播
        for inp in self.tick2player_inputs[self.current_tick]:
            if inp is None:
                is_full_input = False
                break
        if is_full_input:
            print 'is full'
            inputs = self.tick2player_inputs[self.current_tick]
            self.inputs_broadcast(inputs)

    def inputs_broadcast(self, inputs):
        """
        广播玩家的输入
        :param tick: 当前tick
        :return:
        """
        print u'广播:服务器 tick:%d, len:%d' % \
              (self.current_tick, len(self.tick2player_inputs))
        msg = MessageFrameInput(FrameInput(self.current_tick, inputs))
        p = Packet()
        msg.serialize(p)
        for i in range(self.max_count):
            client = self.clients[i]
            client.send(p.byte_list)
        self.current_tick += 1

    def start_game(self):
        print u'房间开始游戏'

        for i in range(self.max_count):
            p = Packet()
            client = self.clients[i]

            msg = MessageStartGame(map_id=0, player_infos=self.player_infos)
            msg.local_player_id = client.player_info.id  # 标识本地用户
            msg.serialize(p)
            client.send(p.byte_list)

    def set_input(self, player_info, message):
        """
        保存玩家发来的输入
        :param player_info:
        :param message:
        :return:
        """
        print u'玩家输入:tick:%d local_id:%d, id:%d， pos:%f,%f' % (
            message.tick, player_info.local_id, player_info.id,
            message.player_input.mouse_pos[0],
            message.player_input.mouse_pos[1])
        local_id = player_info.local_id

        # 存放对应的玩家输入
        if len(self.tick2player_inputs) <= message.tick:
            inputs = [None for i in range(self.max_count)]
            self.tick2player_inputs.append(inputs)

        self.tick2player_inputs[message.tick][local_id] = \
            message.player_input
        self.check_inputs()

