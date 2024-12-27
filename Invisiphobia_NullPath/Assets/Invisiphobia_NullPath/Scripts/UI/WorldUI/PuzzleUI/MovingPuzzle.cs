using Common.Yield;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MovingPuzzle : PuzzleUI
{
    [SerializeField] Image backGround;
    [SerializeField] Image movingImage;
    [SerializeField] Image followImage;

    [SerializeField] float ImageMoveSpeed = 2f;
    [SerializeField] Image fillAmount;

    Vector2 moveDirection;
    private Coroutine moveCoroutine;
    private bool isOverlapping = false; //겹침 상태 확인.

    //movingImage가 background에서 움직이고
    //background에서 위나 아래 왼쪽 오른쪽중 랜덤한 방향으로 2초마다 방향이 전환되는 로직 필요.
    //followImage가 movingImage 위에 2/3이상 겹쳐져 있는 상태에서 2초가 지나면
    //fillAmount의 값을 0.1만큼 증가시키는 로직이 필요.

    public override void Init(IActiveStatable<TabletStateType> subject)
    {
        
    }

    public override void Subscribe(IActiveStatable<TabletStateType> subject)
    {
        gameObject.SetActive(true);
    }

    public override void Unsubscribe(IActiveStatable<TabletStateType> subject)
    {
        gameObject.SetActive(false);
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

}
