using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletController : MonoBehaviour
{
    [SerializeField] private Transform handTr;                  //손에 든 위치 Transform
    [SerializeField] private Transform viewTr;                  //내가 보는 위치 Transform
    [SerializeField] private Transform hiddenTr;                //숨기는 위치 Transform

    [SerializeField] private float transitionSpeed = 5f;        //움직임 속도

    private Transform pickTr;                                   //지금 상태에 선택한 위치 Transform

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init(Tablet tablet)
    {
        tablet.OnStateChangedEvent += HandleStateChanged;
        pickTr = handTr;
    }

    private void Update()
    {
        MoveTabletPosition();
    }

    /// <summary>
    /// 테블릿 스텟 변화 이벤트 등록 함수
    /// </summary>
    private void HandleStateChanged(TabletStateType newState)
    {
        switch (newState)
        {
            case TabletStateType.Basic:
                pickTr = handTr;
                break;
            case TabletStateType.Activate:
                pickTr = viewTr;
                break;
            case TabletStateType.Hidden:
                pickTr = hiddenTr;
                break;
        }
    }

    /// <summary>
    /// tablet이 pickTr위치로 이동 함수
    /// </summary>
    private void MoveTabletPosition()
    {
        float speed = Time.deltaTime * transitionSpeed;

        transform.position = Vector3.LerpUnclamped(transform.position, pickTr.position, speed);
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, pickTr.rotation, speed);
    }
}
