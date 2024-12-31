using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingPuzzle_Collider : MonoBehaviour
{
    [SerializeField] Image targetImage;
    [SerializeField] Image followImage;

    [SerializeField] AudioClip fillClip;
    [SerializeField] AudioClip clearClip;
    [SerializeField] Image myFillAmount;

    public float addValue = 0.1f;
    private float timer = 0f;
    private bool isOverlapping = false;

    public event Action clearActionEvent;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == followImage.gameObject)
        {
            // 타이머 진행
            timer += Time.deltaTime;

            // 2초가 지나면 fillAmount를 증가
            if (timer >= 3f)
            {
                myFillAmount.fillAmount = Mathf.Clamp(myFillAmount.fillAmount + addValue, 0f, 1f);
                Managers.Sound.SFX2DPlay(fillClip);
                timer = 0f;  // 타이머 리셋

                if(myFillAmount.fillAmount >= 1.0f)
                {
                    clearActionEvent.Invoke();
                    Managers.Sound.SFX2DPlay(clearClip);
                }
            }
        }
    }

    // OnTriggerExit2D에서 겹침이 끝나면 타이머 초기화
    private void OnTriggerExit2D(Collider2D other)
    {
        // followImage와의 접촉이 끝났을 때만 실행
        if (other.gameObject == followImage)
        {
            timer = 0f;  // 타이머 초기화
        }
    }
}
