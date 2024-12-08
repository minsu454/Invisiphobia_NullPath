using Common.Yield;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public event Action<Collider> EnterEvent;
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