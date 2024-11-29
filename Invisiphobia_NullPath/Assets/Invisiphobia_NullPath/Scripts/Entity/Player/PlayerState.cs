using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    /// <summary>
    /// 플레이어 죽는 조건 함수
    /// </summary>
    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("몬스터와 충돌해서 플레이어가 천국으로 갔습니다");
            //여기에 효과음 및 죽는 애니메이션, 기타 죽었을 경우
            //띄워줄 UI 삽입
        }
    }


}
