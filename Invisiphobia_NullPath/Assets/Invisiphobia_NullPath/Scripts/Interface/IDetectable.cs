using System;
using System.Collections;
using UnityEngine;

public interface IDetectable
{
    public Transform transform { get; }

    /// <summary>
    /// 프롭 상태 타입
    /// </summary>
    public PropStateType StateType { get; }

    public MapIcon MapIcon { get; }

    /// <summary>
    /// 감지되었을 때(리스트에 추가되었을 때)확인 bool 변수
    /// </summary>
    public bool IsDetectTablet { get; set; }

    /// <summary>
    /// 감지될때 실행될 로직(아직 투명하지만 알람은 울릴때)
    /// </summary>
    public void Detected();

    /// <summary>
    /// 감지 중일 때 실행될 로직(테블릿으로 물체 검사할 때)
    /// </summary>
    public void Detecting();

    /// <summary>
    /// 감지 완료일 때 실행될 로직(테블릿에 있는 바가 다 채워졌을 때)
    /// </summary>
    public void DetectCompleted();

    /// <summary>
    /// 드러날때 실행될 로직(태블릿에서 확인해서 투명화 해제)
    /// </summary>
    public void Revealed();

    /// <summary>
    /// 사라졌을 때 실행될 로직
    /// </summary>
    public void Invisible();

    /// <summary>
    /// 감지바 설정 함수
    /// </summary>
    public void SetFillAmount(float value);
}
