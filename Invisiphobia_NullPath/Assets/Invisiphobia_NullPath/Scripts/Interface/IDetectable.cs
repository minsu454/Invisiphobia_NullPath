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

    /// <summary>
    /// 감지될때 실행될 로직(아직 투명하지만 알람은 울릴때)
    /// </summary>
    public void Detected();

    /// <summary>
    /// 드러날때 실행될 로직(태블릿에서 확인해서 투명화 해제)
    /// </summary>
    public void Revealed();

    /// <summary>
    /// 사라졌을 때 실행될 로직
    /// </summary>
    public void Invisible();
}
