using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Test
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public void GameClear()
    {

    }

    public void GameOver()
    {

        Managers.UI.CreatePopup<GameOverPopup>();
    }
}
