# coding:utf-8


class Room:
    def __init__(self, max_count=2):
        self.max_count = max_count
        self.player_infos = []
        self.clients = []
        self.current_tick = 0
        self.recv_count = 0

    def add_player(self, player_info, client):
        self.player_infos.append(player_info)
        self.clients.append(client)

    def update(self):

        if self.recv_count < self.max_count:
            return
        self.input_broadcast()

    def input_broadcast(self):
        pass


