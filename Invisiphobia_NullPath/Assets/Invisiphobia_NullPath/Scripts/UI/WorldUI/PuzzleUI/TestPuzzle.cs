using Common.Yield;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TestPuzzle : PuzzleUI
{
    public TMP_InputField playerInput;    // 현재 값 입력창 (수정 불가능)
    public TMP_InputField recentCode;   // 최근 제출 값 입력창 (수정 불가능)
    public UnityEngine.UI.Button[] modifyButtons;  // +1, +10, +100, -1, -10, -100 버튼
    public UnityEngine.UI.Button submitButton;     // 제출 버튼
    public Image upImage;           // Up 상태 이미지
    public Image downImage;         // Down 상태 이미지

    public int targetNumber;       // 게임 시작 시 정해진 번호
    private int currentInputValue;  // 현재 입력 값

    private bool isColorChanged = false;
    public override void Init(IActiveStatable<TabletStateType> subject)
    {
        targetNumber = Random.Range(1, 1000); // 1~999 사이 랜덤 값
        currentInputValue = 0;
        playerInput.text = currentInputValue.ToString();
        recentCode.text = "0";

        // InputField 직접 수정 방지
        playerInput.readOnly = true;
        recentCode.readOnly = true;
        Debug.Log("Target Number: " + targetNumber); // 디버그 용
    }

    public override void Subscribe(IActiveStatable<TabletStateType> subject)
    {
        gameObject.SetActive(true);
    }

    public override void Unsubscribe(IActiveStatable<TabletStateType> subject)
    {
        gameObject.SetActive(false);
    }

    public void ModifyInputValue(int value)
    {
        currentInputValue += value;
        // 값 범위 제한
        currentInputValue = Mathf.Clamp(currentInputValue, 0, 999);
        // 업데이트된 값 반영
        playerInput.text = currentInputValue.ToString();
    }

    // 제출 버튼 처리
    public void SubmitValue()
    {
        int submittedValue = currentInputValue;

        // RecentCode 업데이트
        recentCode.text = submittedValue.ToString();

        // 숫자 비교 및 상태 이미지 업데이트
        if (submittedValue < targetNumber && !isColorChanged)
        {
            StartCoroutine(ChangeImageColor(upImage, Color.green));
        }
        else if (submittedValue > targetNumber && !isColorChanged)
        {
            StartCoroutine(ChangeImageColor(downImage, Color.red));
        }
        else if((submittedValue == targetNumber))
        {
            Debug.Log("정답입니다!");
        }
    }

    // 이미지 색상 변경 후 복원
    IEnumerator ChangeImageColor(Image image, Color targetColor)
    {
        isColorChanged = true;
        Color originalColor = image.color;
        image.color = targetColor;
        yield return YieldCache.WaitForSeconds(1.5f);
        image.color = originalColor;
        isColorChanged = false;
    }

}
