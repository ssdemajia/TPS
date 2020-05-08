# -*- coding: GBK -*-
MSG_JOIN_ROOM = 0
MSG_QUIT_ROOM = 1
MSG_PLAYER_INPUT = 2
MSG_START_GAME = 3
MSG_FRAME_INPUT = 4


NET_STATE_STOP = 0  # state: init value  连接已经关闭
NET_STATE_CONNECTING = 1  # state: connecting  正在通信
NET_STATE_ESTABLISHED = 2  # state: connected  连接建立

NET_HEAD_LENGTH_SIZE = 4  # 4 bytes little endian (x86)
NET_HEAD_LENGTH_FORMAT = '<I'

NET_CONNECTION_NEW = 0  # new connection
NET_CONNECTION_LEAVE = 1  # lost connection
NET_CONNECTION_DATA = 2  # data comming

NET_HOST_DEFAULT_TIMEOUT = 70

MAX_HOST_CLIENTS_INDEX = 0xffff  # 最大连接数
MAX_HOST_CLIENTS_BYTES = 16
