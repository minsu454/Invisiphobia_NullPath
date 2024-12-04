using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour, IDetectable
{
    [Header("Prop")]
    [SerializeField] private MeshRenderer myRenderer;
    [SerializeField] private Collider myCollider;
    [SerializeField] private SpriteRenderer mapIcon;

    public PropStateType StateType { get; protected set; } = PropStateType.None;

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// Prop 초기화 함수
    /// </summary>
    public virtual void Init()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.enabled = false;
        myCollider = GetComponent<Collider>();
    }

    public virtual void Detected()
    {
        StateType = PropStateType.Detected;
    }

    public virtual void Revealed()
    {
        StateType = PropStateType.Revealed;
        myRenderer.enabled = true;
    }

    public virtual void Invisible()
    {
        StateType = PropStateType.None;
        myRenderer.enabled = false;
    }
}
