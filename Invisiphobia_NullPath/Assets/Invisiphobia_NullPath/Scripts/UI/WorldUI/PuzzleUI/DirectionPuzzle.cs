using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Common.Yield;

public class DirectionPuzzle : PuzzleUI
{
    public Image[] directionImages; // 방향 이미지 슬롯 (6개)
    public Sprite[] directionSprites;    // 방향 이미지 스프라이트 (왼쪽, 오른쪽, 위쪽, 아래쪽)
    public Image roundBar;
    public Image currentInputImage;

    [SerializeField] AudioClip fillClip;
    [SerializeField] AudioClip clearClip;

    [SerializeField] private int clearCount = 10;
    
    private int currentDirectionIndex = 0; // 현재 확인 중인 방향 인덱스
    private int[] generatedDirections;  // 생성된 방향 배열
    private bool isGameActive = false;   // 게임 상태


    public override void Init(IActiveStatable<TabletStateType> subject)
    {
        StartNewGame();
        roundBar.fillAmount = 0;
    }

    void Update()
    {
        if (isGameActive)
        {
            HandleInput();
        }
    }

    public override void Subscribe(IActiveStatable<TabletStateType> subject)
    {
        gameObject.SetActive(true);
    }

    public override void Unsubscribe(IActiveStatable<TabletStateType> subject)
    {
        gameObject.SetActive(false);
    }

    void StartNewGame()
    {
        isGameActive = true;
        StartNewRound();
    }

    void StartNewRound()
    {
        roundBar.fillAmount += 1f / clearCount;
        Managers.Sound.SFX2DPlay(fillClip);

        currentDirectionIndex = 0;
        generatedDirections = new int[directionImages.Length];

        // 방향 랜덤 생성
        for (int i = 0; i < directionImages.Length; i++)
        {
            // 랜덤 방향 선택 (0: 왼쪽, 1: 오른쪽, 2: 위, 3: 아래)
            int randomDirection = Random.Range(0, directionSprites.Length); // 0~3
            generatedDirections[i] = randomDirection;

            // 해당 이미지에 랜덤으로 방향 이미지 배치
            directionImages[i].sprite = directionSprites[randomDirection];
        }
        
        if(roundBar.fillAmount >= 1f)
        {
            Managers.Sound.SFX2DPlay(clearClip);
            GameOver(true);
        }
    }

    void HandleInput()
    {
        // 각 방향에 맞는 입력 처리
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentInputImage.sprite = directionSprites[0];
            if (generatedDirections[currentDirectionIndex] == 0)
            {
                ProcessCorrectInput(currentDirectionIndex);
            }
            else if (Input.anyKeyDown) { ProcessWrongInput(); }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentInputImage.sprite = directionSprites[1];
            if (generatedDirections[currentDirectionIndex] == 1)
            {
                ProcessCorrectInput(currentDirectionIndex);
            }
            else if (Input.anyKeyDown) { ProcessWrongInput(); }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentInputImage.sprite = directionSprites[2];
            if (generatedDirections[currentDirectionIndex] == 2)
            {
                ProcessCorrectInput(currentDirectionIndex);
            }
            else if (Input.anyKeyDown) { ProcessWrongInput(); }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentInputImage.sprite = directionSprites[3];
            if (generatedDirections[currentDirectionIndex] == 3)
            {
                ProcessCorrectInput(currentDirectionIndex);
            }
            else if (Input.anyKeyDown) { ProcessWrongInput(); }
        }

    }


    void ProcessCorrectInput(int index)
    {
        // 올바른 입력이 들어오면 해당 방향 이미지를 비우기
        directionImages[index].sprite = null; // 스프라이트를 null로 비워서 없앰

        // 올바른 입력 후 라운드 진행
        currentDirectionIndex++;
        if (currentDirectionIndex >= directionImages.Length)
        {
            StartNewRound(); // 라운드가 끝난 경우 새로운 라운드 시작
        }
    }

    void ProcessWrongInput()
    {
        // 틀린 입력 처리 시 진행 상태 감소
        if (roundBar.fillAmount > 0)
        {
            roundBar.fillAmount -= 1 / clearCount;
        }
    }

    void GameOver(bool isWin)
    {
        isGameActive = false;
        OnComplete();
    }

    public void RetryBtn()
    {
        StartNewGame();
        roundBar.fillAmount = 0f;
    }
}
