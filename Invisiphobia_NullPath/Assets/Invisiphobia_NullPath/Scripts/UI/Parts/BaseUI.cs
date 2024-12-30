using System;
using UnityEngine;

public class BaseUI : MonoBehaviour, IAddressable
{
    public event Action<GameObject> ReleaseEvent;

    protected virtual void OnDestroy()
    {
        ReleaseEvent?.Invoke(gameObject);
    }
}