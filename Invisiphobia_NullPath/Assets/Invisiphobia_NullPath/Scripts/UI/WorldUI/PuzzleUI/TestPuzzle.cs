
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TestPuzzle : PuzzleUI
{
    public InputField playerCode;
    public UnityEngine.UI.Button submitBtn;
    public bool isCorrect = false;

    private int[] answerCode;
    private int attemptCount;
    public override void Init(IActiveStatable<TabletStateType> subject)
    {
        GenerateRandomAnswer();
        playerCode.onValueChanged.AddListener(OnValueChanged);
    }

    public override void Subscribe(IActiveStatable<TabletStateType> subject)
    {

    }

    public override void Unsubscribe(IActiveStatable<TabletStateType> subject)
    {

    }

    void GenerateRandomAnswer()
    {
        answerCode = new int[4];
        for (int i = 0; i < answerCode.Length; i++)
        {
            answerCode[i] = Random.Range(0, 10); // 각 자리수 0~9
        }
        Debug.Log("퍼즐 정답: " + string.Join("", answerCode));
    }

    void PlayerAnswer()
    {
        string playerInput = playerCode.text;
        int[] playerNumbers = new int[4];

        for (int i = 0; i < playerInput.Length; i++)
        {
            playerNumbers[i] = int.Parse(playerInput[i].ToString());
        }

        for (int i = 0; i < answerCode.Length; i++)
        {
            if (playerNumbers[i] < answerCode[i])
            {
                isCorrect = false;
            }
            else if (playerNumbers[i] > answerCode[i])
            {
                isCorrect = false;
            }

            else
            {
                isCorrect = true;
            }
        }

        attemptCount++;

        if (isCorrect)
        {
            
        }
}
    void OnValueChanged(string input)
    {
        // 숫자 4개 이상 입력 시 자르기
        if (input.Length > 4)
        {
            playerCode.text = input.Substring(0, 4);
        }
    }
}
