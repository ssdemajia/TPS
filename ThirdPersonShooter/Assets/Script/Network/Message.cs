using Shaoshuai;
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

    public enum MessageType:ushort
    {
        JoinRoom=10,
        QuitRoom,
        PlayerInput,
        StartGame,
        FrameInput,
        HashCode
    }
    public interface IMessage
    {
        ushort opcode { get; set; }
    }
    public interface ISerializable
    {
        void Serialize(Packet p);
        void Deserialize(Packet p);
    }

    public class BaseFormat
    {
        public virtual void Deserialize(Packet p) { }
        public virtual void Serialize(Packet p) { }
    }

    public class MessageHashCode : BaseFormat
    {
        public ushort opcode { get; set; } = (ushort)MessageType.HashCode;
        public int tick;
        public int hash;

        public override void Serialize(Packet p)
        {
            p.push(tick);
            p.push(hash);
        }

        public override void Deserialize(Packet p)
        {
            tick = p.getInt32();
            hash = p.getInt32();
        }
    }

    public class MessageJoinRoom: BaseFormat
    {
        public ushort opcode { get; set; } = (ushort)MessageType.JoinRoom;
        public string name;

        public override void Serialize(Packet p)
        {
            p.push(name);
        }

        public override void Deserialize(Packet p)
        {
            name = p.getString();
        }
    }

    public class MessageQuitRoom: BaseFormat
    {
        public ushort opcode { get; set; } = (ushort)MessageType.QuitRoom;
        public int val;

        public override void Serialize(Packet p)
        {
            p.push(val);
        }

        public override void Deserialize(Packet p)
        {
            val = p.getInt32();
        }
    }

    public class MessagePlayerInput: BaseFormat
    {
        public ushort opcode { get; set; } = (ushort)MessageType.PlayerInput;
        public int tick;
        public PlayerInput input;

        public override void Serialize(Packet p)
        {
            p.push(tick);
            p.push(input == null);
            input.Serialize(p);
        }

        public override void Deserialize(Packet p)
        {
            tick = p.getInt32();
            if (p.getBool())
                return;
            input.Deserialize(p);
        }
    }

    public class MessageStartGame: BaseFormat
    {
        public ushort opcode { get; set; } = (ushort)MessageType.StartGame;
        public int mapId;
        public int localPlayerId;
        public PlayerServerInfo[] playerInfo;

        public override void Serialize(Packet p)
        {
            p.push(mapId);
            p.push(localPlayerId);
            p.push(playerInfo);
        }

        public override void Deserialize(Packet p)
        {
            mapId = p.getInt32();
            localPlayerId = p.getInt32();
            playerInfo = p.getArray(playerInfo);
        }
    }

    public class MessageFrameInput : BaseFormat
    {
        /// <summary>
        /// 包含帧信息的消息包
        /// </summary>
        public ushort opcode { get; set; } = (ushort)MessageType.FrameInput;
        public FrameInput input = new FrameInput();

        public override void Serialize(Packet p)
        {
            input.Serialize(p);
        }

        public override void Deserialize(Packet p)
        {
            input.Deserialize(p);
        }
    }

    [Serializable]
    public class PlayerInput : BaseFormat
    {
        /// <summary>
        /// 玩家输入
        /// </summary>
        public FixedVec2 mousePos;
        public FixedVec2 inputHV;   // horizontal, vertical
        public bool fire1;
        public bool reload;
        public bool isJump;

        public override void Deserialize(Packet p)
        {
            mousePos = p.getFixedVec2();
            inputHV = p.getFixedVec2();
            fire1 = p.getBool();
            reload = p.getBool();
            isJump = p.getBool();
        }

        public override void Serialize(Packet p)
        {
            p.push(mousePos);
            p.push(inputHV);
            p.push(fire1);
            p.push(reload);
            p.push(isJump);
        }
    }

    [Serializable]
    public class PlayerServerInfo: BaseFormat
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public string name;
        public int id;
        public int localId;
        public FixedVec3 initPos;
        public FixedVec3 initDeg;
        public int PrefabId;


        public override void Serialize(Packet p)
        {
            p.push(name);
            p.push(id);
            p.push(localId);
            p.push(initPos);
            p.push(initDeg);
            p.push(PrefabId);
        }

        public override void Deserialize(Packet p)
        {
            name = p.getString();
            id = p.getInt32();
            localId = p.getInt32();
            initPos = p.getFixedVec3();
            initDeg = p.getFixedVec3();
            PrefabId = p.getInt32();
        }
    }

    [Serializable]
    public class FrameInput: BaseFormat
    {
        /// <summary>
        /// 帧信息
        /// </summary>
        public int tick;
        public PlayerInput[] inputs;

        public override void Deserialize(Packet p)
        {
            tick = p.getInt32();
            inputs = p.getArray(inputs);
        }

        public override void Serialize(Packet p)
        {
            p.push(tick);
            p.push(inputs);
        }
    }
}
