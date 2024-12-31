using Common.Yield;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MovingPuzzle : PuzzleUI
{
    [SerializeField] Image targetImage;
    [SerializeField] Image followImage;

    [SerializeField] RectTransform moveRange;

    [SerializeField] float moveDuration = 2f; // 이동 시간
    [SerializeField] private float interval = 1f; // 대기 시간

    private Vector2 targetImageMoveDirection;

    private Coroutine moveCoroutine;
    private Coroutine overlapCoroutine;

    private bool isMouseInsideBoard = true;

    [SerializeField] MovingPuzzle_Collider movingPuzzle_Collider;
    public override void Init(IActiveStatable<TabletStateType> subject)
    {
        movingPuzzle_Collider.clearActionEvent += GameOver;
        Cursor.lockState = CursorLockMode.None;
        StartRandomMoveCoroutine();
    }

    public override void Subscribe(IActiveStatable<TabletStateType> subject)
    {
        gameObject.SetActive(true);
    }

    public override void Unsubscribe(IActiveStatable<TabletStateType> subject)
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MouseFollowImage();
    }

    #region targetMove
    /// <summary>
    /// 랜덤 이동 실행
    /// </summary>
    private void StartRandomMoveCoroutine()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(CoRandomMoveRoutine());
    }

    /// <summary>
    /// 랜덤 이동 코루틴
    /// </summary>
    private IEnumerator CoRandomMoveRoutine()
    {
        while (true)
        {
            TargetImageMoveRandom();
            yield return YieldCache.WaitForSeconds(interval);
        }
    }

    /// <summary>
    /// targetimage 이동 함수
    /// </summary>
    private void TargetImageMoveRandom()
    {
        // 이동 가능한 영역의 범위 계산
        float minX = moveRange.rect.xMin;
        float maxX = moveRange.rect.xMax;
        float minY = moveRange.rect.yMin;
        float maxY = moveRange.rect.yMax;

        // 랜덤한 목표 위치 생성
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        // targetImage를 랜덤 위치로 이동
        targetImage.rectTransform.DOAnchorPos(randomPosition, moveDuration).SetEase(Ease.InOutQuad);
    }
    #endregion


    #region followImage

    /// <summary>
    /// followimage가 마우스를 따라가게 하는 함수
    /// </summary>
    void MouseFollowImage()
    {
        // 마우스 위치 가져오기 (화면 좌표)
        Vector3 mousePosition = Input.mousePosition;

        // followImage의 Canvas가 Screen Space - Overlay라면 바로 적용
        if (followImage.canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            followImage.rectTransform.position = mousePosition;
        }
        else
        {
            // Screen Space - Camera 또는 World Space일 경우 변환 필요
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                followImage.canvas.GetComponent<RectTransform>(), // Canvas의 RectTransform
                mousePosition,
                followImage.canvas.worldCamera, // UI 카메라
                out Vector3 worldPosition
            );

            followImage.rectTransform.position = worldPosition;
        }
    }

    void GameOver()
    {
        OnComplete();
    }
    #endregion
}
