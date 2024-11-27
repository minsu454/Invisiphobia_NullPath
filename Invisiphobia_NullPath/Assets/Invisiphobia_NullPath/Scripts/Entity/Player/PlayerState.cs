using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : Entity
{
    [SerializeField]
    GameObject stamina;

    [SerializeField]
    Slider staminaSlider;

    private Coroutine sliderCoroutine;

    private bool isShiftPressed = false;

    private void Start()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = 1f; // 초기값 설정
        }
    }

    private void Update()
    {
        
    }

    private void PlayerStamina()
    {
        // Shift 키 눌림 감지
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (!isShiftPressed)
            {
                isShiftPressed = true;
                StartSliderCoroutine(-0.1f); // 감소 코루틴 시작
            }
        }

        // Shift 키 떼는 동작 감지
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            if (isShiftPressed)
            {
                isShiftPressed = false;
                StartSliderCoroutine(0.1f); // 증가 코루틴 시작
                if(staminaSlider.value == 1f)
                {

                }
            }
        }
    }

    private void StartSliderCoroutine(float changeRate)
    {
        if (sliderCoroutine != null)
        {
            StopCoroutine(sliderCoroutine); // 기존 코루틴 중지
        }
        sliderCoroutine = StartCoroutine(UpdateSliderValue(changeRate)); // 새로운 코루틴 시작
    }

    private IEnumerator UpdateSliderValue(float changeRate)
    {
        while (true)
        {
            if (staminaSlider != null)
            {
                staminaSlider.value = Mathf.Clamp(staminaSlider.value + changeRate * Time.deltaTime, 0f, 1f);
            }
            yield return null; // 다음 프레임까지 대기
        }
    }


    private void PlayerDie()
    {

    }
}
