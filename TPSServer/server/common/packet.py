# coding:utf-8
import struct
from message import PlayerInput

class Packet:
    """ 序列化消息 """
    def __init__(self, byte_list=''):
        self.byte_list = byte_list
        self.index = 0
        self.last_length = 0

    def push_byte(self, b):
        self.byte_list += struct.pack('b', b)

    def push_int16(self, num):
        self.byte_list += struct.pack('h', num)

    def push_int32(self, num):
        self.byte_list += struct.pack('i', num)

    def push_int64(self, num):
        self.byte_list += struct.pack('q', num)

    def push_bool(self, flag):
        num = 0
        if flag:
            num = 1
        self.push_byte(num)

    def push_bytes(self, bs):
        self.byte_list += bs

    def push_string(self, string):
        str_len = len(string)
        self.push_int16(str_len)
        self.byte_list += string

    def push_vec2(self, vec2):
        for v in vec2:
            self.push_int64(v)

    def push_vec3(self, vec3):
        for v in vec3:
            self.push_int64(v)

    def push_player_input_arr(self, arr):
        length = len(arr)
        self.push_int16(length)
        for player_input in arr:
            flag = player_input is None
            self.push_bool(flag)
            if flag:
                continue
            player_input.serialize(self)

    def get_byte(self):
        self.index += self.last_length
        self.last_length = 1
        return struct.unpack('b', self.byte_list[self.index])[0]

    def get_bool(self):
        val = self.get_byte()
        return val == 1

    def get_int16(self):
        self.index += self.last_length
        self.last_length = 2
        sub_str = self.byte_list[self.index:self.index+2]
        return struct.unpack('<H', sub_str)[0]

    def get_int32(self):
        self.index += self.last_length
        self.last_length = 4
        return struct.unpack('i', self.byte_list[self.index:self.index+4])[0]

    def get_int64(self):
        self.index += self.last_length
        self.last_length = 8
        return struct.unpack('q', self.byte_list[self.index:self.index+8])[0]

    def get_string(self):
        str_len = self.get_int16()
        self.index += self.last_length
        self.last_length = str_len
        return self.byte_list[self.index: self.index+str_len]

    def get_remain(self):
        self.index += self.last_length
        return self.byte_list[self.index:]

    def get_vec2(self):
        res = [0, 0]
        res[0] = self.get_int64()
        res[1] = self.get_int64()
        return res

    def get_vec3(self):
        res = [0, 0, 0]
        res[0] = self.get_int64()
        res[1] = self.get_int64()
        res[2] = self.get_int64()
        return res

    def get_player_input_arr(self):
        arr_len = self.get_int16()
        # print u'获得用户input长度：%d' % arr_len
        if arr_len == 0:
            return None
        res = []
        for i in range(arr_len):
            if self.get_bool():
                res.append(None)
                continue
            player = PlayerInput()
            player.deserialize(self)
            # print u"获得用户输入:%f, %f" % (player.mouse_pos[0], player.mouse_pos[1])
            res.append(player)
        return res
