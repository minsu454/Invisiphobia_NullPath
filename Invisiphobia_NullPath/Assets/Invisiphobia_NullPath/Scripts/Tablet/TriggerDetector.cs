using Common.Yield;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 트리거 콜라이더가 다른곳에 있을 때 사용하는 클래스
/// </summary>
public class TriggerDetector : MonoBehaviour
{
    /// <summary>
    /// Trigger Enter시 사용 이벤트
    /// </summary>
    public event Action<Collider> EnterEvent;

    /// <summary>
    /// Trigger Exit시 사용 이벤트
    /// </summary>
    public event Action<Collider> ExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        EnterEvent?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ExitEvent?.Invoke(other);
    }
}