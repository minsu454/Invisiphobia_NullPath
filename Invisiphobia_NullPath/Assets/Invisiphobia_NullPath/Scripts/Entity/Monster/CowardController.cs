using Common.Event;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class CowardController : MonoBehaviour, IDetectable
{
    [Header("Renderer")]
    public Renderer myRenderer;

    [Header("Target")]
    [SerializeField] private Transform targetPosition;

    protected float targetDistance;

    [Header("NavMeshAgent")]
    [SerializeField] protected NavMeshAgent agent;
    public Vector3 spawnPoint { get; private set; }

    public float fadeDuration = 2f;
    public float timeOutOfSight = 0f;
    public float saveSpeed;

    private Color originalColor;

    MapIcon IDetectable.MapIcon => mapIcon;
    private MapIcon mapIcon;                                                    //맵아이콘

    public bool IsDetectTablet { get; set; }

    public PropStateType StateType { get; protected set; } = PropStateType.None;

    #region Test
    [Header("MapIcon")]
    [SerializeField] private GameObject mapIconPrefab;                          //맵 아이콘 프리팹(임시)
    #endregion

    public void Start()
    {
        spawnPoint = transform.position;
        myRenderer.enabled = false;

        if (myRenderer != null)
        {
            originalColor = myRenderer.material.color;
        }

        GameObject go = Instantiate(mapIconPrefab);
        mapIcon = go.GetComponent<MapIcon>();
        mapIcon.Init(transform);
        EventManager.Subscribe(GameEventType.UseMonsterPause, OnUseMonsterPause);
    }

    private void Update()
    {
        if (StateType != PropStateType.Revealed)
            return;

        FleeingUpdate();
    }

    /// <summary>
    /// 도망 조건을 만족하며 도망 위치로 이동하는 함수
    /// </summary>
    void FleeingUpdate()
    {
        // 도착 지점 마지막에 fade 효과
        float remainingDistance = Vector3.Distance(transform.position, agent.destination);
        if (remainingDistance <= 2f)
        {
            StartFadeEffect(remainingDistance);
        }
    }

    /// <summary>
    /// 남은 거리에 따라 알파값 조정
    /// </summary>
    void StartFadeEffect(float remainingDistance)
    {
        if (myRenderer != null)
        {
            // 남은 거리가 1에서 0으로 줄어드는 동안 알파값을 1에서 0으로 변경
            float fadeAlpha = Mathf.Lerp(1f, 0f, (1f - remainingDistance) / 1f);
            Color monsterColor = originalColor;
            monsterColor.a = fadeAlpha;
            myRenderer.material.color = monsterColor;

            if (fadeAlpha <= 0.6f)  // 0.6 임시
            {
                myRenderer.enabled = false;
                myRenderer.material.color = originalColor;
                ResetToSpawnPoint();
            }
        }
    }

    /// <summary>
    /// 지정해준 포지션으로 이동하는 함수
    /// </summary>
    void SetTargetDestination()
    {
        float maxPosition = 1.0f;

        if (NavMesh.SamplePosition(targetPosition.position, out NavMeshHit hit, maxPosition, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    protected void ResetToSpawnPoint()
    {
        agent.Warp(spawnPoint);
        ResetCycle();
    }

    public virtual void Detected()
    {
        StateType = PropStateType.Detected;
        mapIcon.Detected();
    }

    public virtual void Detecting()
    {
        StateType = PropStateType.Detecting;
        mapIcon.Detecting();
    }

    public void DetectCompleted()
    {
        StateType = PropStateType.DetectCompleted;
    }

    public virtual void Revealed()
    {
        if (StateType != PropStateType.DetectCompleted)
        {
            Detected();
            return;
        }

        StateType = PropStateType.Revealed;
        myRenderer.enabled = true;
        mapIcon.Revealed();

        SetTargetDestination();
    }

    public virtual void Invisible()
    {
        StateType = PropStateType.None;
        mapIcon.Invisible();
        myRenderer.enabled = false;
    }

    public void ResetCycle()
    {
        if (IsDetectTablet)
        {
            mapIcon.Invisible();
            myRenderer.enabled = false;
            Detected();
        }
        else
        {
            Invisible();
        }
    }

    public void SetFillAmount(float value)
    {
        mapIcon.SetFillAmount(value);
    }

    public void SetMapIconToWall(bool active)
    {
        if (active)
        {
            mapIcon.Detected();
        }
        else
        {
            mapIcon.Invisible();
        }
    }

    private void OnUseMonsterPause(object args)
    {
        if (StateType != PropStateType.Revealed)
            return;

        if ((bool)args)
        {
            saveSpeed = agent.speed;
            agent.speed = 0f;
        }
        else
        {
            agent.speed = saveSpeed;
        }
    }

    private void OnDestroy()
    {
        EventManager.Subscribe(GameEventType.UseMonsterPause, OnUseMonsterPause);
    }
}
