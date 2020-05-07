# -*- coding: GBK -*-

from server.simpleServer import SimpleServer

server = SimpleServer()
server.host.startup(9999)
while True:
