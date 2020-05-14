using Shaoshuai.Core;
using Shaoshuai.Message;
using System;
using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }
    GameObject LoginPanel; // 登陆注册
    InputField usernameInput;
    InputField passwordInput;
    Text infoLabel;
    public Button LoginButton;
    public Button QuitGameButton;
    public Button RegisterButton;

    GameObject PlayerPanel;// 展示角色
    ScrollRect PlayerScroll;
    [SerializeField]private GameObject ScrollContent;
    InputField playerNameInput;
    public Button BackButton;
    public Button CreateButton;
    Text createLabel;

    GameObject LoadingPanel;// 等待画面

    MonoTimer timer;
    Shaoshuai.Message.Player _currentPlayer;
    public Shaoshuai.Message.Player CurrentPlayer
    {
        get
        {
            if (_currentPlayer == null)
            {
                _currentPlayer = new Shaoshuai.Message.Player() // 初始角色信息
                {
                    player_name = "ssss",
                    exp = 0,
                    level = 1,
                    ammo = 200,
                    hp = 100
                };
            }
            return _currentPlayer;
        }

        // 在PlayerInfoButton中点击时设置当前玩家角色信息
        set
        {
            _currentPlayer = value;
        }
    } 

    GameObject prefabs;
    HttpClient client;
    public void Start()
    {
        Instance = this;
        timer = gameObject.GetComponent<MonoTimer>();
        client = new HttpClient();
        LoginPanel = GameObject.Find("LoginPanel");
        usernameInput = GameObject.Find("UserNameInput").GetComponent<InputField>();
        passwordInput = GameObject.Find("PasswordInput").GetComponent<InputField>();
        infoLabel = GameObject.Find("InfoLabel").GetComponent<Text>();

        PlayerPanel = GameObject.Find("PlayerPanel");
        PlayerScroll = GameObject.Find("PlayerScroll").GetComponent<ScrollRect>();
        createLabel = GameObject.Find("CreateLabel").GetComponent<Text>();
        playerNameInput = GameObject.Find("PlayerNameInput").GetComponent<InputField>();
        LoadingPanel = GameObject.Find("LoadingPanel");

        LoginButton.onClick.AddListener(Login);
        RegisterButton.onClick.AddListener(Register);
        QuitGameButton.onClick.AddListener(QuitGame);
        CreateButton.onClick.AddListener(Create);
        BackButton.onClick.AddListener(Back);
        LoginPanel.SetActive(true);
        PlayerPanel.SetActive(false);
        LoadingPanel.SetActive(false);
    }
    public async void Login()
    {
        LoadingPanel.SetActive(true);
        var info = new LoginRequest(usernameInput.text, passwordInput.text);
        
        Usersession.SetUser(info.username, info.password);

        var content = GameManager.ObjToHttpContent(info);
        HttpResponseMessage resp;

        try
        {
            resp = await client.PostAsync("http://127.0.0.1:9000/login", content);
        }
        catch (Exception)
        {
            LoadingPanel.SetActive(false);
            infoLabel.text = "连接服务器错误";
            return;
        }
        
        var result = resp.Content.ReadAsStringAsync().Result;
        var loginResp = JsonUtility.FromJson<LoginResponse>(result);
        LoadingPanel.SetActive(false);

        ScrollContent.transform.DetachChildren(); // 清空子元素

        if (loginResp.code == 100)
        {
            LoginPanel.SetActive(false);
            prefabs = (GameObject)Resources.Load("Prefabs/PlayerInfoButton");
            foreach (var player in loginResp.players)
            {
                GameObject infoButton = GameObject.Instantiate(prefabs) as GameObject;
                var button = infoButton.GetComponent<PlayerInfoButton>();
                button.setText(player);
                infoButton.SetActive(true);
                infoButton.transform.SetParent(ScrollContent.transform, false);
            }
            PlayerPanel.SetActive(true);
        }
        else if (loginResp.code == 101)
        {
            infoLabel.text = "用户不存在";
        }
        else if (loginResp.code == 102)
        {
            infoLabel.text = "密码错误";
        }
        timer.Add(() =>{ infoLabel.text = "";}, 2);
    }
 
    public async void Register()
    {
        var info = new LoginRequest(usernameInput.text, passwordInput.text);
        var content = GameManager.ObjToHttpContent(info);
        var resp = await client.PostAsync("http://127.0.0.1:9000/register", content);
        var result = resp.Content.ReadAsStringAsync().Result;
        var registerResp = JsonUtility.FromJson<Response>(result);
        if (registerResp.code == 103)
        {
            infoLabel.text = "用户名存在";
        }
        else if (registerResp.code == 104)
        {
            infoLabel.text = "创建成功";
        }
        timer.Add(() => { infoLabel.text = ""; }, 2);
    }

    public async void Create()
    {
        var info = new CreateRequest(Usersession.Instance.username, playerNameInput.text);
        var content = GameManager.ObjToHttpContent(info);
        var resp = await client.PostAsync("http://127.0.0.1:9000/create", content);
        var result = resp.Content.ReadAsStringAsync().Result;
        var createResp = JsonUtility.FromJson<CreateResponse>(result);
        if (createResp.code == 105)
        {
            createLabel.text = "创建成功";
            InsertScroll(createResp.player);
        }

        else if (createResp.code == 106)
        {
            createLabel.text = "角色名称重复";
        }
        timer.Add(() => { createLabel.text = ""; }, 2);
    }

    void Back()
    {
        PlayerPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    void InsertScroll(Shaoshuai.Message.Player player)
    {
        GameObject infoButton = GameObject.Instantiate(prefabs) as GameObject;
        infoButton.GetComponent<PlayerInfoButton>().setText(player);
        infoButton.SetActive(true);
        infoButton.transform.SetParent(ScrollContent.transform, false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
