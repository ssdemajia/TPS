# -*- coding: GBK -*-

from server.simpleServer import SimpleServer
from server.common import conf


server = SimpleServer()
server.host.startup(9999)

while True:
    server.tick()
    server.update()
    event, wparam, data = server.host.read()

    if event < 0:
        continue
    # print '-------------------', event, wparam, data
    if event == conf.NET_CONNECTION_DATA:
        status, client = server.host.get_client(wparam)
        server.dispatch(data, client)
    elif event == conf.NET_CONNECTION_NEW:
        # print(data)
        pass
    elif event == conf.NET_CONNECTION_LEAVE:
        player_info = wparam
