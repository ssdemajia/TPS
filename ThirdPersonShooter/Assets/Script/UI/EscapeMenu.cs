using Shaoshuai.Core;
using Shaoshuai.Message;
using System;
using System.Net.Http;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] GameObject EscapeMenuPanel;
    [SerializeField] Button YesMenu;
    [SerializeField] Button NoMenu;
    [SerializeField] Button SaveMenu;
    [SerializeField] Text infoLabel;
    GameObject LoadingPanel;
    HttpClient client;

    public void Start()
    {
        EscapeMenuPanel.SetActive(false);
        client = new HttpClient();
        LoadingPanel = GameObject.Find("LoadingPanel");
        LoadingPanel.SetActive(false);

        YesMenu.onClick.AddListener(ClickQuitToMenu);
        NoMenu.onClick.AddListener(ClickClose);
        SaveMenu.onClick.AddListener(ClickSaveAsync);
    }

    private async void ClickSaveAsync()
    {
        EscapeMenuPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        var info = new SaveRequest(GameManager.Instance.CurrentPlayer);
        var content = GameManager.ObjToHttpContent(info);
        HttpResponseMessage resp;
        resp = await client.PostAsync("http://127.0.0.1:9000/save", content);

        var result = resp.Content.ReadAsStringAsync().Result;
        var saveResp = JsonUtility.FromJson<Response>(result);
        LoadingPanel.SetActive(false);

        if (saveResp.code == 107)
        {
            infoLabel.text = "保存成功";
        }
        else
        {
            infoLabel.text = "保存失败";
        }
        EscapeMenuPanel.SetActive(true);
    }

    public void ClickClose()
    {
        EscapeMenuPanel.SetActive(false);
        GameManager.Instance.IsPause = false;
    }
    public void ClickQuitToMenu()
    {
        SceneManager.LoadScene("LoginAndRegister");
    }

    private void Update()
    {
        if (EscapeMenuPanel.activeSelf)
            return;

        if (GameManager.Instance.inputController.Escape)
        {
            EscapeMenuPanel.SetActive(true);
            GameManager.Instance.IsPause = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
