using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoleSelect : MonoBehaviour
{
    public Button StartGameButton;
    public Button QuitGameButton;
    public Button RegisterButton;
    public string levelName;

    public void Start()
    {
        StartGameButton.onClick.AddListener(() => { 
            StartGame(levelName); 
        });
        QuitGameButton.onClick.AddListener(QuitGame);
    }
    public void StartGame(string levelName)
    {
        print(levelName);
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
