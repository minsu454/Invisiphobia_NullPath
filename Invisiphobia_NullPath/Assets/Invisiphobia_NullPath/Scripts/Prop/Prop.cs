using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Prop : MonoBehaviour, IDetectable
{
    [Header("Prop")]
    [SerializeField] private MeshRenderer myRenderer;
    [SerializeField] private Collider myCollider;

    MapIcon IDetectable.MapIcon => mapIcon;
    [SerializeField] private MapIcon mapIcon;

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

        mapIcon.Init();
    }

    public virtual void Detected()
    {
        StateType = PropStateType.Detected;
        mapIcon.Detected();
    }

    public virtual void Detecting(float value)
    {
        mapIcon.Detecting(value);
    }

    public void DetectCompleted()
    {
        StateType = PropStateType.DetectCompleted;
    }

    public virtual void Revealed()
    {
        if (StateType != PropStateType.DetectCompleted)
            Invisible();

        StateType = PropStateType.Revealed;
        myRenderer.enabled = true;
        mapIcon.Revealed();
    }

    public virtual void Invisible()
    {
        StateType = PropStateType.None;
        myRenderer.enabled = false;
            mapIcon.Invisible();
    }
}
