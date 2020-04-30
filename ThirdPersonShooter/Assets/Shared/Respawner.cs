using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    /* 去除当前物体，然后几秒后重新生成 */
    public void Despawn(GameObject game, float inSeconds)
    {
        game.SetActive(false);
        GameManager.Instance.Timer.Add(() =>
        {
            game.SetActive(true);
        }, inSeconds);
    }
}
