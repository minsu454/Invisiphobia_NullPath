using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public float playerHealth { get; private set; } = 100f;


    private void PlayerTired()
    {
        if(Player.Instance.PlayerMovement.staminaSlider.value == 0f)
        {
            Player.Instance.PlayerMovement.moveSpeed *= 0.5f;
        }
    }

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
