using System;

namespace Shaoshuai.Message {
    [Serializable]
    public class Player
    {
        public string player_name;
        public int exp;
        public int level;
        public int ammo;
        public int damage;
        public int hp;
    }
    [Serializable]
    public class Request
    {
        
    }

    [Serializable]
    public class Response
    {
        /// <summary>
        /// status:
        ///     100，登陆成功
        ///     101，用户不存在
        ///     102，密码错误
        ///     103, 用户存在
        ///     104, 用户创建成功
        ///     105, 角色创建成功
        ///     106, 角色名重复
        ///     107, 保存成功
        /// </summary>
        public int code;
    }

    [Serializable]
    public class LoginResponse
    {
        public int code;
        public Player[] players;
    }

    [Serializable]
    public class LoginRequest: Request
    {
        public LoginRequest(string name, string password)
        {
            this.username = name;
            this.password = password;
        }

        public string username;
        public string password;
    }

    [Serializable]
    public class CreateResponse
    {
        public int code;
        public Player player;
    }

    [Serializable]
    public class CreateRequest: Request
    {
        public CreateRequest(string username, string player_name)
        {
            this.username = username;
            this.player_name = player_name;
        }
        public string username;
        public string player_name;
    }

}
