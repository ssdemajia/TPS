# coding:utf-8
import struct


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

    def get_byte(self):
        self.index += self.last_length
        self.last_length = 1
        return struct.unpack('b', self.byte_list[self.index])[0]

    def get_int16(self):
        self.index += self.last_length
        self.last_length = 2
        return struct.unpack('h', self.byte_list[self.index:2])[0]

    def get_int32(self):
        self.index += self.last_length
        self.last_length = 4
        return struct.unpack('i', self.byte_list[self.index:4])[0]

    def get_int64(self):
        self.index += self.last_length
        self.last_length = 8
        return struct.unpack('q', self.byte_list[self.index:8])[0]

    def get_string(self):
        str_len = self.get_int16()
        self.index += self.last_length
        self.last_length = str_len
        return self.byte_list[self.index: self.index+str_len]

    def get_remain(self):
        self.index += self.last_length
        return self.byte_list[self.index:]
