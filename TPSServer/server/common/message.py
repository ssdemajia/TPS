# -*- coding: GBK -*-
from server.common import conf


class PlayerInput:
    # 包含用户输入
    def __init__(self):
        self.mouse_pos = [0, 0]
        self.hv = [0, 0]
        self.fire1 = False
        self.reload = False
        self.is_jump = False

    def serialize(self, p):
        p.push_vec2(self.mouse_pos)
        p.push_vec2(self.hv)
        p.push_bool(self.fire1)
        p.push_bool(self.reload)
        p.push_bool(self.is_jump)

    def deserialize(self, p):
        self.mouse_pos = p.get_vec2()
        self.hv = p.get_vec2()
        self.fire1 = p.get_bool()
        self.reload = p.get_bool()
        self.is_jump = p.get_bool()


class PlayerInfo:
    def __init__(self, name='', player_id=0, local_id=0):
        self.name = name
        self.id = player_id  # 全局玩家id
        self.local_id = local_id  # 房间中的id
        self.init_pos = [0, 0, 0]
        self.init_deg = [0, 0, 0]
        self.prefab_id = 0

    def serialize(self, p):
        p.push_string(self.name)
        p.push_int32(self.id)
        p.push_int32(self.local_id)
        p.push_vec3(self.init_deg)
        p.push_vec3(self.init_deg)
        p.push_int32(self.prefab_id)

    def deserialize(self, p):
        self.name = p.get_string()
        self.id = p.get_int32()
        self.local_id = p.get_int32()
        self.init_pos = p.get_vec3()
        self.init_deg = p.get_vec3()
        self.prefab_id = p.get_int32()


class FrameInput:
    def __init__(self, tick=0, player_inputs=None):
        self.tick = tick
        self.player_inputs = player_inputs

    def serialize(self, p):
        p.push_int32(self.tick)
        p.push_player_input_arr(self.player_inputs)

    def deserialize(self, p):
        self.tick = p.get_int32()
        self.player_inputs = p.get_player_input_arr()


class MessageJoinRoom:
    def __init__(self, name=''):
        self.name = name
        self.opcode = conf.MSG_JOIN_ROOM

    def serialize(self, packet):
        packet.push_int16(conf.MSG_JOIN_ROOM)
        packet.push_string(self.name)

    def deserialize(self, packet):
        self.name = packet.get_string()


class MessageQuitRoom:
    def __init__(self, value=0):
        self.value = value
        self.opcode = conf.MSG_QUIT_ROOM

    def serialize(self, packet):
        packet.push_int16(conf.MSG_QUIT_ROOM)
        packet.push_int32(self.value)

    def deserialize(self, packet):
        self.value = packet.get_int32()


class MessagePlayerInput:
    def __init__(self, tick=0, player_input=None):
        self.opcode = conf.MSG_PLAYER_INPUT
        self.tick = tick
        if player_input is None:
            self.player_input = PlayerInput()
        else:
            self.player_input = player_input

    def serialize(self, packet):
        packet.push_int16(conf.MSG_PLAYER_INPUT)
        packet.push_int32(self.tick)
        packet.push_bool(self.player_input is None)
        self.player_input.serialize(packet)

    def deserialize(self, packet):
        self.tick = packet.get_int32()
        if packet.get_bool():
            return
        self.player_input.deserialize(packet)


class MessageStartGame:
    def __init__(self, map_id=0, local_player_id=0, player_infos=None):
        self.opcode = conf.MSG_START_GAME
        self.map_id = map_id
        self.local_player_id = local_player_id
        self.player_infos = player_infos

    def serialize(self, p):
        p.push_int16(conf.MSG_START_GAME)
        p.push_int32(self.map_id)
        p.push_int32(self.local_player_id)
        p.push_player_input_arr(self.player_infos)

    def deserialize(self, p):
        self.map_id = p.get_int32()
        self.local_player_id = p.get_int32()
        self.player_infos = p.get_player_input_arr()


class MessageFrameInput:
    def __init__(self, frame_input=None):
        self.opcode = conf.MSG_FRAME_INPUT
        self.frame_input = frame_input

    def serialize(self, p):
        p.push_int16(conf.MSG_FRAME_INPUT)
        self.frame_input.serialize(p)

    def deserialize(self, p):
        self.frame_input.deserialize(p)
