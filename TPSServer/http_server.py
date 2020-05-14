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
    basic_damage = 10
    basic_hp = 100

    def __init__(self, player_name='', username=''):
        self.player_name = player_name
        self.username = username
        self.exp = 0
        self.level = 1
        self.ammo = 200

    @staticmethod
    def get_damage(level):
        # 每一枪的伤害随着等级提高而增加
        return Player.basic_damage + level

    @staticmethod
    def get_hp(level):
        # 血量随着等级提高增加
        return Player.basic_hp + level*10

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
        cursor.execute('INSERT INTO player VALUES(?, ?, ?, ?, ?)',
                       (player.username, player.player_name, player.exp,
                        player.level, player.ammo))
        conn.commit()
        res = {
            'code': 105,
            'player': player.__dict__
        }
        res['player']['hp'] = Player.get_hp(player.level)
        res['player']['damage'] = Player.get_damage(player.level),
        return res
    @staticmethod
    def save_player(player):

        cursor = conn.cursor()
        cursor.execute('INSERT INTO player VALUES(?, ?, ?, ?, ?)',
                       (player.username, player.player_name, player.exp,
                        player.level, player.ammo))
        conn.commit()
        res = {
            'code': 105,
            'player': player.__dict__
        }
        return res

    @staticmethod
    def create_table():
        cursor = conn.cursor()
        try:
            cursor.execute('''CREATE TABLE player
             (username text, player_name text, exp int, level int, ammo int)
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
                'damage': Player.get_damage(item[3]),
                'hp': Player.get_hp(item[3])
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


User.create_table()
Player.create_table()
User.create_user('ss', '123')
Player.create_player('ss', 'test1')
Player.create_player('ss', 'test2')
http_server = BaseHTTPServer.HTTPServer(('', PORT), RequestHandler)
http_server.serve_forever()

