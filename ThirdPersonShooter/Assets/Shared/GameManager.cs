using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public event System.Action<Player> OnLocalPlayerJoined;
    public bool IsPause;
    GameObject gameObject;
    static GameManager manager;
    InputController inputController;
    Player player;
    Timer timer;
    Respawner respawner;
    EventBus eventBus;

    public static GameManager Instance
    {
        get
        {
            if (manager == null)
            {
                manager = new GameManager();
                manager.gameObject = new GameObject("_gameManager");
                manager.gameObject.AddComponent<InputController>();
                manager.gameObject.AddComponent<Timer>();
                manager.gameObject.AddComponent<Respawner>();
            }
            return manager;
        }
    }
   
    public InputController InputController
    {
        get
        {
            if (inputController == null)
            {
                inputController = gameObject.GetComponent<InputController>();
            }

            return inputController;
        }
    }

    public Timer Timer
    {
        get
        {
            if (timer == null)
                timer = gameObject.GetComponent<Timer>();
            return timer;
        }
    }

    public EventBus EventBus
    {
        get
        {
            if (eventBus == null)
                eventBus = new EventBus();
            return eventBus;
        }
    }
    public Respawner Respawner
    {
        get
        {
            if (respawner == null)
                respawner = gameObject.GetComponent<Respawner>();
            return respawner;
        }
    }
    public Player LocalPlayer
    {
        get
        {
            return player;
        }
        set
        {
            player = value;
            if (OnLocalPlayerJoined != null)
            {
                OnLocalPlayerJoined(player);
            }
        }
    }
}
