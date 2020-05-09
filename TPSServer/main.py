# -*- coding: GBK -*-

from server.simpleServer import SimpleServer
from server.common import conf
import time


server = SimpleServer()
server.host.startup(9999)

while True:
    server.tick()
    server.update()
    event, wparam, data = server.host.read()
    if event < 0:
        time.sleep(1)
        continue

    if event == conf.NET_CONNECTION_DATA:
        status, client = server.host.get_client(wparam)
        server.dispatch(data, client)
    elif event == conf.NET_CONNECTION_NEW:
        print(data)
