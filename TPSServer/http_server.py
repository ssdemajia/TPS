# coding=utf-8
import json
import BaseHTTPServer
import sqlite3
PORT = 9000
conn = sqlite3.connect(':memory:')  # 目前只是简单应用

"""
code:
    100，登陆成功
    101，用户不存在
    102，密码错误
    103, 用户存在
    104, 用户创建成功
    105, 角色创建成功
    106, 角色名重复
"""


class User:
    def __init__(self):
        self.user_id = 0
        self.username = ''
        self.password = ''
        self.players = []

    @staticmethod
    def create_user(username, password):
        cursor = conn.cursor()
        cursor.execute('INSERT INTO user VALUES(?, ?)',
                       (username, password))
        conn.commit()

    @staticmethod
    def create(name, password):
        cursor = conn.cursor()
        cursor.execute('SELECT * FROM user WHERE username = ?', (name,))
        res = cursor.fetchone()
        if res is None:
            User.create_user(name, password)
            return {'code': 104}
        return {
            'code': 103
        }

    @staticmethod
    def create_table():
        cursor = conn.cursor()
        try:
            cursor.execute('''CREATE TABLE user
                (username text primary key, password text)''')
        except sqlite3.OperationalError as e:
            print(e)

    @staticmethod
    def load_info(name, password):
        cursor = conn.cursor()
        cursor.execute('SELECT * FROM user WHERE username = ?', (name,))
        res = cursor.fetchone()
        if res is None:
            return {'code': 101, 'players': []}
        elif res[1] != password:
            return {'code': 102, 'players': []}
        players = Player.get_players(name)
        return {
            'code': 100,
            'players': players
        }

    @staticmethod
    def has_user(username):
        """
        是否存在用户
        :param username: 用户名
        :return: true存在；false不存在
        """
        cursor = conn.cursor()
        cursor.execute('SELECT username FROM user WHERE username = ?', (username,))
        res = cursor.fetchone()
        return res is not None


class Player:

    def __init__(self, player_name='', username=''):
        self.player_name = player_name
        self.username = username
        self.exp = 0
        self.level = 1
        self.ammo = 200
        self.hp = Player.get_hp(1)

    @staticmethod
    def get_hp(level):
        # 血量随着等级提高增加
        return level*100

    @staticmethod
    def create_player(username, player_name):
        if not User.has_user(username):
            return {'code': 101}
        cursor = conn.cursor()
        cursor.execute('SELECT player_name FROM player WHERE player_name = ?', (player_name,))
        res = cursor.fetchone()
        if res is not None:
            return {
                'code': 106
            }
        player = Player(player_name, username)
        cursor = conn.cursor()
        cursor.execute('INSERT INTO player VALUES(?, ?, ?, ?, ?, ?)',
                       (player.username, player.player_name, player.exp,
                        player.level, player.ammo, player.hp))
        conn.commit()
        res = {
            'code': 105,
            'player': player.__dict__
        }
        return res

    @staticmethod
    def save(player):
        import time
        time.sleep(1)
        cursor = conn.cursor()
        cursor.execute('UPDATE player SET exp = ?, hp=?, level=?, ammo=? WHERE player_name = ?',
                       (player.exp, player.hp,
                        player.level, player.ammo, player.player_name))
        conn.commit()
        res = {
            'code': 107,
        }
        return res

    @staticmethod
    def create_table():
        cursor = conn.cursor()
        try:
            cursor.execute('''CREATE TABLE player
             (username text, player_name text, exp int, level int, ammo int, hp int)
             ''')
        except sqlite3.OperationalError as e:
            print(e)

    @staticmethod
    def get_players(username):
        cursor = conn.cursor()
        cursor.execute('SELECT * FROM player WHERE username = ?', (username,))
        res = cursor.fetchall()
        players = []
        for item in res:
            players.append({
                'player_name': item[1],
                'exp': item[2],
                'level': item[3],
                'ammo': item[4],
                'hp': item[5]
            })
        return players

    def update_level(self):
        pass


class RequestHandler(BaseHTTPServer.BaseHTTPRequestHandler):
    def do_GET(self):
        req = self.request
        path = self.path
        self.send_header('Content-Type', 'application/json')
        self.send_response(200)
        self.end_headers()
        self.wfile.write("test")

    def do_POST(self):
        path = self.path
        res = ''
        data_string = self.rfile.read(int(self.headers['Content-Length']))
        data = json.loads(data_string)
        if path == '/login':
            res = self.login(data)
        elif path == '/register':
            res = self.register(data)
        elif path == '/create':
            res = self.create(data)
        elif path == '/save':
            res = self.save(data)
        res = json.dumps(res)
        self.send_response(200)
        self.send_header('Content-Type', 'application/json')
        self.end_headers()
        self.wfile.write(res)

    def login(self, data):
        res = User.load_info(data['username'], data['password'])
        return res

    def register(self, data):
        res = User.create(data['username'], data['password'])
        return res

    def create(self, data):
        res = Player.create_player(data['username'], data['player_name'])
        return res

    def save(self, data):
        player = Player(player_name=data[u'player_name'])
        player.level = data[u'level']
        player.hp = data[u'hp']
        player.ammo = data[u'ammo']
        player.exp = data[u'exp']
        res = Player.save(player)
        return res


User.create_table()
Player.create_table()
name_pass = [["netease1", 123], ["netease2", 123], ["netease3", 123]]
for username, password in name_pass:
    User.create_user(username, password)
    Player.create_player(username, 'test1')
    Player.create_player(username, 'test2')
http_server = BaseHTTPServer.HTTPServer(('', PORT), RequestHandler)
http_server.serve_forever()

