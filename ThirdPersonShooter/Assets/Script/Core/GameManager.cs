using Shaoshuai.Message;
using Shaoshuai.Network;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;

namespace Shaoshuai.Core
{
    public class GameManager: BaseManager
    {
        [Header("ClientMode")]
        public PlayerServerInfo clientModeInfo; // 客户端模式中，玩家的信息
        public bool IsClientMode = false;
        public string serverIp = "127.0.0.1";
        public int serverPort = 9999;

        [HideInInspector] public int currentMapId = 0;
        [HideInInspector] public int localPlayerId = 0;
        [HideInInspector] public int playerCount = 1;
        [HideInInspector] public PlayerServerInfo[] playerInfos;
        int frameIndex = 0;
        [HideInInspector] public FrameInput currentFrame;
        [HideInInspector] public List<FrameInput> frames = new List<FrameInput>();  // 累计的帧信息
        [HideInInspector] public float remainTime; // 距离下一次更新的时间累计
        [HideInInspector] public int predictTickCount = 3;
        public int tick = 0;
        [HideInInspector] public static int maxServerTick = 0;


        [Header("Player")]
        public static GameObject localPlayer;
        public static Transform localPlayerTrans;

        public Transform reticle; // 准星
        public event System.Action<Transform> OnLocalPlayerJoined; 
        public bool IsPause = false;
        private List<BaseManager> managers = new List<BaseManager>();

        NetworkClient client;
        bool hasStart = false;

        public static GameManager Instance { get;private set;} // 只读
        public static PlayerInput CurrentInput = new PlayerInput(); // 当前用户输入
        public InputController inputController;

        private void Awake()
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!"+MainMenu.Instance.CurrentPlayer);
            Screen.SetResolution(800, 600, false);
            inputController = gameObject.AddComponent<InputController>();
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

        private void Start()
        {
            DoStart();
            foreach (var manager in managers)
            {
                manager.DoStart();
            }

            if (IsClientMode)
            {
                clientModeInfo = new PlayerServerInfo()
                {
                    initPos = new FixedVec3(0, 8, 0)
                };
                playerCount = 1;
                localPlayerId = 0;
                playerInfos = new PlayerServerInfo[] { clientModeInfo };
                frames = new List<FrameInput>();
                StartGame(0, playerInfos, localPlayerId);
            }
            else
            {
                client = new NetworkClient();
                client.Connect(serverIp, serverPort);
                MessageJoinRoom msg = new MessageJoinRoom { name = Application.dataPath };
                Packet p = new Packet();
                p.push((Int16)msg.opcode);
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
            playerCount = infos.Length;  // 更新玩家数量
            for (int i = 0; i < playerCount; i++)
            {
                var info = playerInfos[i];
                var localId = info.localId;
                if (localPlayerId == localId)
                {
                    localPlayer = PlayerManager.InstantiateLocalPlayer(info.PrefabId, info.initPos);
                    localPlayerTrans = localPlayer.transform;
                }
                
            }
            OnLocalPlayerJoined?.Invoke(localPlayerTrans);
        }

        private void Update()
        {
            if (client != null)
                client.ParseMassage(); // 解析收到的数据包
            if (!hasStart)
                return;
            //remainTime += Time.deltaTime;
            //while (remainTime >= 0.017f)
            //{
            //    remainTime -= 0.017f;
            //    // 发送玩家输入
            //    SendInput();
            //    if (GetFrame(frameIndex) == null)  // 如果还没有接收到帧信息
            //        return;
            //    Step();
            //}
        }

        //public void SendInput()
        //{
           
        //    //Debug.Log("发送输入信息,id:" + localPlayerId);
        //    MessagePlayerInput msg = new MessagePlayerInput() // 用户输入信息打包
        //    {
        //        input = CurrentInput,
        //        tick = this.tick
        //    };
        //    Packet p = new Packet();
        //    p.push((Int16)MessageType.PlayerInput);
        //    msg.Serialize(p);
        //    client.Send(p.GetByteStream());
        //    tick++;
        //}

        /// <summary>
        /// 获取tick对应的帧信息
        /// </summary>
        /// <param name="tick">第tick个帧</param>
        /// <returns>帧信息</returns>
        //public FrameInput GetFrame(int tick)
        //{
        //    if (frames.Count > tick)
        //    {
        //        FrameInput frame = frames[tick];
        //        if (frame != null && frame.tick == tick)
        //            return frame;
        //    }
        //    return null;
        //}

        /// <summary>
        /// 获得输入帧
        /// </summary>
        /// <param name="input"></param>
        //public static void PushInputFrame(FrameInput input)
        //{
        //    var frames = Instance.frames;

        //    // 补上本地缺少的帧信息
        //    for (int i = frames.Count; i <= input.tick; i++)
        //    {
        //        frames.Add(new FrameInput());
        //    }
        //    maxServerTick = Mathf.Max(maxServerTick, input.tick);
        //    frames[input.tick] = input;
        //}

        // 更新各个玩家信息
        //void Step()
        //{
        //    frameIndex++;
        //    //Debug.Log("frame index:" + frameIndex);
        //    UpdateManagers();
        //}

        private void UpdateManagers()  // 更新每一个管理器
        {
            var deltaTime = new FixedVec1(0.3f);
            foreach(var manager in managers)
            {
                manager.DoUpdate(deltaTime);
            }
        }

        private void OnDestroy()
        {
            if (client != null)
            {
                MessageQuitRoom msg = new MessageQuitRoom();
                Packet p = new Packet();
                msg.Serialize(p);
                client.Send(p.GetByteStream());
            }
           
            foreach (var manager in managers)
            {
                manager.DoDestroy();
            }
        }

        public void RegisterManager(BaseManager mgr)
        {
            managers.Add(mgr);
        }
        public static HttpContent ObjToHttpContent<T>(T obj)
        {
            var value = JsonUtility.ToJson(obj);
            HttpContent content = new StringContent(value, Encoding.UTF8, "application/json");
            return content;
        }
    }

}
