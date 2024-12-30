using Common.Yield;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MovingPuzzle : PuzzleUI
{
    [SerializeField] Image backGround;
    [SerializeField] Image targetImage;
    [SerializeField] Image followImage;

    [SerializeField] RectTransform board; // Board 영역
    [SerializeField] float ImageMoveSpeed = 2f;
    [SerializeField] Image fillAmount;

    private Vector2 moveDirection;
    private Coroutine moveCoroutine;
    private Coroutine overlapCoroutine;

    private bool isMouseInsideBoard = true;
    private bool isOverlapping = false;

    public override void Init(IActiveStatable<TabletStateType> subject)
    {
        StartRandomMovement();
    }

    public override void Subscribe(IActiveStatable<TabletStateType> subject)
    {
        gameObject.SetActive(true);
    }

    public override void Unsubscribe(IActiveStatable<TabletStateType> subject)
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateFollowImagePosition();
    }

    /// <summary>
    /// followImage의 위치를 업데이트
    /// </summary>
    private void UpdateFollowImagePosition()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            board,
            Input.mousePosition,
            null,
            out mousePosition
        );

        // 마우스가 Board 영역 안에 있는지 확인
        if (board.rect.Contains(mousePosition))
        {
            isMouseInsideBoard = true;
            followImage.rectTransform.anchoredPosition = mousePosition;
        }
        else
        {
            isMouseInsideBoard = false;
            ClampFollowImageToBoard();
        }
    }

    /// <summary>
    /// followImage가 Board 영역 안에 있도록 고정
    /// </summary>
    private void ClampFollowImageToBoard()
    {
        Vector2 clampedPosition = followImage.rectTransform.anchoredPosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, board.rect.xMin, board.rect.xMax);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, board.rect.yMin, board.rect.yMax);
        followImage.rectTransform.anchoredPosition = clampedPosition;
    }

    /// <summary>
    /// 충돌 감지 시작
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == targetImage.gameObject && !isOverlapping)
        {
            isOverlapping = true;
            overlapCoroutine = StartCoroutine(CoHandleOverlap());
        }
    }

    /// <summary>
    /// 충돌 종료
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == targetImage.gameObject)
        {
            isOverlapping = false;
            if (overlapCoroutine != null)
            {
                StopCoroutine(overlapCoroutine);
                overlapCoroutine = null;
            }
        }
    }

    /// <summary>
    /// 2초 이상 겹치면 fillAmount 증가
    /// </summary>
    private IEnumerator CoHandleOverlap()
    {
        yield return new WaitForSeconds(2f);

        if (isOverlapping)
        {
            fillAmount.fillAmount += 0.1f;
            isOverlapping = false;
        }
    }

    /// <summary>
    /// 코루틴으로 이미지 움직임을 시작
    /// </summary>
    public void StartRandomMovement()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(CoRandomMove());
    }

    /// <summary>
    /// 2초마다 랜덤한 방향으로 이동
    /// </summary>
    private IEnumerator CoRandomMove()
    {
        while (true)
        {
            moveDirection = GetRandomDirection();
            yield return YieldCache.WaitForSeconds(2f);
        }
    }

    private Vector2 GetRandomDirection()
    {
        int randomValue = Random.Range(0, 4); // 0~3 랜덤값 생성
        return randomValue switch
        {
            0 => Vector2.up,
            1 => Vector2.down,
            2 => Vector2.left,
            3 => Vector2.right,
            _ => Vector2.zero
        };
    }

    private void LateUpdate()
    {
        // targetImage 움직임 처리
        targetImage.rectTransform.anchoredPosition += moveDirection * ImageMoveSpeed * Time.deltaTime;
    }
}
