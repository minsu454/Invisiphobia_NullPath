using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void GameClear()
    {

    }

    public void GameOver()
    {
        //Managers.UI.CreatePopup<>
    }
}
