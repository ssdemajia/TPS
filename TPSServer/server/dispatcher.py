# -*- coding: GBK -*-
from server.common.packet import Packet


class Service(object):
    def __init__(self, sid=0):
        super(Service, self).__init__()

        self.service_id = sid
        self.__command_map = {}

    def handle(self, msg, owner):
        cid = msg.cid
        if cid not in self.__command_map:
            raise Exception('bad command %s' % cid)

        f = self.__command_map[cid]
        return f(msg, owner)

    def register(self, cid, function):
        self.__command_map[cid] = function

    def registers(self, command_dict):
        self.__command_map = {}
        for cid in command_dict:
            self.register(cid, command_dict[cid])


class Dispatcher(object):
    def __init__(self):
        super(Dispatcher, self).__init__()

        self.__service_map = {}

    def dispatch(self, data, owner):
        """
        消息派发
        :param data: 原始数据
        :param owner: 接收的客户端
        """
        p = Packet(data)
        opcode = p.get_int16()
        if opcode not in self.__service_map:
            raise Exception('bad opcode %d' % opcode)

        svc = self.__service_map[opcode]
        return svc.handle(opcode, p, owner)

    def register(self, opecode, handle):
        self.__service_map[opecode] = handle
