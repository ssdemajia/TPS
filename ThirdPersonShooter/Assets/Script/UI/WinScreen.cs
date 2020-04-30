using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] GameObject WinPanel;
    [SerializeField] Button backtoMenu;

    public void Start()
    {
        WinPanel.SetActive(false);
        backtoMenu.onClick.AddListener(ClickQuitToMenu);

        GameManager.Instance.EventBus.AddListener("OnWin", () =>
        {
            GameManager.Instance.Timer.Add(() =>
            {
                WinPanel.SetActive(true);
                GameManager.Instance.IsPause = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }, 2);
            
        });
    }

    public void ClickQuitToMenu()
    {
        SceneManager.LoadScene("UI");
    }

    
}
