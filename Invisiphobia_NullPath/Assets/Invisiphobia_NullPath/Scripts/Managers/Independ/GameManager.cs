using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void GameClear()
    {
        Managers.UI.CreatePopup<GameClearPopup>();
    }

    public void GameOver()
    {
        Managers.UI.CreatePopup<GameOverPopup>();
    }
}
