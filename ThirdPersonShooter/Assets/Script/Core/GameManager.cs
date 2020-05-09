using System.Collections.Generic;
using UnityEngine;
using Shaoshuai.Message;
using Shaoshuai.Network;
using Shaoshuai.Entity;
using Shaoshuai.View;

namespace Shaoshuai.Core
{
    public class GameManager: BaseManager
    {
        [Header("ClientMode")]  
        public bool IsClientMode = false;
        public string serverIp = "127.0.0.1";
        public int serverPort = 9999;
        public int maxCount = 2;

        [HideInInspector] public int currentMapId = 0;
        [HideInInspector] public int localPlayerId = 0;
        [HideInInspector] public int playerCount = 1;
        [HideInInspector] public PlayerServerInfo[] playerInfos;
        int frameIndex = 0;
        [HideInInspector] public FrameInput currentFrame;
        [HideInInspector] public List<FrameInput> frames = new List<FrameInput>();  // 累计的帧信息
        [HideInInspector] public float remainTime; // 距离下一次更新的时间累计
        [HideInInspector] public int predictTickCount = 2;
        [HideInInspector] public int tick = 0;
        [HideInInspector] public static int maxServerTick = 0;
        

        [Header("Player")]
        public static List<PlayerEntity> allPlayers = new List<PlayerEntity>();
        public static PlayerEntity localPlayer;
        public static PlayerView localPlayerView;
        public static Transform localPlayerTrans;

        public event System.Action<Transform> OnLocalPlayerJoined; 
        public bool IsPause;
        private List<BaseManager> managers = new List<BaseManager>();

        NetworkClient client;
        bool hasStart = false;

        public static GameManager Instance { get;private set;} // 只读
        public static PlayerInput CurrentInput = new PlayerInput(); // 当前用户输入

        private void Awake()
        {
            Screen.SetResolution(1024, 768, false);
            gameObject.AddComponent<InputController>();
            DoAwake();
            foreach (var manager in managers)
            {
                manager.DoAwake();
            }
        }

        public static void StartGame(MessageStartGame startMsg)
        {
            Instance.StartGame(startMsg.mapId, startMsg.playerInfo, startMsg.localPlayerId);
        }

        public override void DoAwake()
        {
            Instance = this;
            var mgrInThis = GetComponents<BaseManager>();
            foreach (var mgr in mgrInThis)
            {
                if (mgr == this) continue;

                RegisterManager(mgr);
            }
        }

        public void RegisterManager(BaseManager mgr)
        {
            managers.Add(mgr);
        }

        private void Start()
        {
            DoStart();
            foreach (var manager in managers)
            {
                manager.DoStart();
            }

            if (IsClientMode)
            {
                
            }
            else
            {
                client = new NetworkClient(maxCount);
                client.Connect(serverIp, serverPort);
                MessageJoinRoom msg = new MessageJoinRoom { name = Application.dataPath };
                Packet p = new Packet();
                p.push(msg.opcode);
                msg.Serialize(p);
                client.Send(p.GetByteStream());
            }
        }

        public void StartGame(int mapId, PlayerServerInfo[] infos, int localPlayerId)
        {
            Debug.Log("游戏开始！");
            // 初始化各个玩家
            hasStart = true;
            currentMapId = mapId;
            playerInfos = infos;
            this.localPlayerId = localPlayerId;
            allPlayers.Clear();
            
            for (int i = 0; i < playerCount; i++)
            {
                allPlayers.Add(new PlayerEntity() { localPlayerId = i });
                var info = playerInfos[i];
                var instance = PlayerManager.InstantiatePlayer(allPlayers[i], info.PrefabId, info.initPos);
                if (localPlayerId == info.id)
                    localPlayerTrans = instance.transform;
            }
            localPlayer = allPlayers[localPlayerId];
            if (OnLocalPlayerJoined != null)
                OnLocalPlayerJoined(localPlayerTrans);
        }

        private void Update()
        {
            client.ParseMassage(); // 解析收到的数据包
            if (!hasStart)
                return;
            remainTime += Time.deltaTime;
            while (remainTime >= 0.03f)
            {
                remainTime -= 0.03f;
                // 发送玩家输入
                SendInput();
                if (GetFrame(frameIndex) == null)  // 如果还没有接收到帧信息
                    return;
                Step();
            }
        }

        public void SendInput()
        {
            if (IsClientMode)
            {

            }

            if (tick > predictTickCount + maxServerTick)  // 当前客户端超过服务端的帧数了，需要等待
            {
                return;
            }

            MessagePlayerInput msg = new MessagePlayerInput() // 用户输入信息打包
            {
                input = CurrentInput,
                tick = this.tick
            };
            Packet p = new Packet();
            msg.Serialize(p);
            client.Send(p.GetByteStream());
            tick++;
        }

        /// <summary>
        /// 获取tick对应的帧信息
        /// </summary>
        /// <param name="tick">第tick个帧</param>
        /// <returns>帧信息</returns>
        public FrameInput GetFrame(int tick)
        {
            if (frames.Count > tick)
            {
                FrameInput frame = frames[tick];
                if (frame != null && frame.tick == tick)
                    return frame;
            }
            return null;
        }

        /// <summary>
        /// 获得输入帧
        /// </summary>
        /// <param name="input"></param>
        public static void PushInputFrame(FrameInput input)
        {
            var frames = Instance.frames;

            // 补上本地缺少的帧信息
            for (int i = frames.Count; i <= input.tick; i++)
            {
                frames.Add(new FrameInput());
            }
            maxServerTick = Mathf.Max(maxServerTick, input.tick);

            frames[input.tick] = input;
            
        }
        void Step()
        {
            currentFrame = GetFrame(frameIndex); // 当前帧对应信息
            for (var i = 0; i < playerCount; i++)
            {
                allPlayers[i].input = currentFrame.inputs[i];
            }
            frameIndex++;
        }

        private void OnDestroy()
        {
            MessageQuitRoom msg = new MessageQuitRoom();
            Packet p = new Packet();
            msg.Serialize(p);
            client.Send(p.GetByteStream());
            foreach (var manager in managers)
            {
                manager.DoDestroy();
            }
        }
    }

}
