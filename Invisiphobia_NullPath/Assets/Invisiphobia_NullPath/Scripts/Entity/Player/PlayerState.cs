using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float playerHealth { get; private set; } = 100f;

    /// <summary>
    /// 플레이어 죽는 조건 함수
    /// </summary>
    private void PlayerDie()
    {
        if(playerHealth == 0f)
        {

        }
    }

}
