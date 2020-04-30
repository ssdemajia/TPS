using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] GameObject EscapeMenuPanel;
    [SerializeField] Button YesMenu;
    [SerializeField] Button NoMenu;

    public string levelName;

    public void Start()
    {
        EscapeMenuPanel.SetActive(false);
        YesMenu.onClick.AddListener(ClickQuitToMenu);
        NoMenu.onClick.AddListener(ClickClose);
    }

    public void ClickClose()
    {
        EscapeMenuPanel.SetActive(false);
        GameManager.Instance.IsPause = false;
    }
    public void ClickQuitToMenu()
    {
        SceneManager.LoadScene("UI");
    }

    private void Update()
    {
        if (EscapeMenuPanel.activeSelf)
            return;

        if (GameManager.Instance.InputController.Escape)
        {
            EscapeMenuPanel.SetActive(true);
            GameManager.Instance.IsPause = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
