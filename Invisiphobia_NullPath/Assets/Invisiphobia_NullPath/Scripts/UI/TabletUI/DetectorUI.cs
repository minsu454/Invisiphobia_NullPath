using Common.Yield;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorUI : WorldUI
{
    [Header("Detector")]
    [SerializeField] private TriggerDetector detector;
    private List<IDetectable> detectedObjectList = new List<IDetectable>();
    private float updateInterval = 1f;
    private float closestdistance;

    private Coroutine timer = null;

    private Coroutine coDetecting;
    private float curDetectTime = 0;
    [SerializeField] private float maxDetectTime = 2f;

    private int layerMask;

    public override void Init(IActiveStatable subject)
    {
        layerMask = LayerMask.GetMask("Wall");

        detector.EnterEvent += TriggerEnter;
        detector.ExitEvent += TriggerExit;
    }

    public override void Subscribe(IActiveStatable subject)
    {
        subject.BasicStateEvent += Reveal;
        subject.BasicStateEvent += StopDetecting;

        subject.ActiveStateEvent += Detecting;
    }

    public override void Unsubscribe(IActiveStatable subject)
    {
        Reveal();
        StopDetecting();

        subject.BasicStateEvent -= Reveal;
        subject.BasicStateEvent -= StopDetecting;

        subject.ActiveStateEvent -= Detecting;
        gameObject.SetActive(false);
    }

    private void TriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDetectable detectable))
        {
            if (detectable.StateType == PropStateType.Revealed)
                return;

            StartTimer();
            detectable.Detected();
            detectedObjectList.Add(detectable);
        }

    }

    private void TriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDetectable detectable))
        {
            if (detectable.StateType == PropStateType.Revealed)
                return;

            detectable.Invisible();
            detectedObjectList.Remove(detectable);
            if (detectedObjectList.Count == 0)
            {
                StopTimer();
            }
        }
    }

    public void HandleStateChanged(TabletStateType newState)
    {
        switch (newState)
        {
            case TabletStateType.Basic:
                
                break;
            case TabletStateType.Activate:
                Detecting();
                break;
        }
    }

    private bool HasLineOfSight(IDetectable target) //IDetectable과 Detecter사이에 Wall이 있는지 판단
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, distance, layerMask))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void StartTimer()
    {
        if (timer == null)
        {
            timer = StartCoroutine(CoCheckTimer());
        }
    }

    private void StopTimer()
    {
        StopCoroutine(timer);
        timer = null;
    }

    private IEnumerator CoCheckTimer()
    {
        while (true)
        {
            closestdistance = float.MaxValue;
            for (int i = detectedObjectList.Count - 1; i >= 0; i--)
            {
                if (HasLineOfSight(detectedObjectList[i]))
                {
                    UpdateDistances(detectedObjectList[i], ref closestdistance);
                    break;
                }
            }
            HandleAlarm(closestdistance);
            yield return YieldCache.WaitForSeconds(updateInterval);
        }
    }

    private void UpdateDistances(IDetectable detectable, ref float closest)
    {
        float distance = Vector3.Distance(transform.position, detectable.transform.position);
        if (distance < closest)
        {
            closest = distance;
        }
    }

    private void HandleAlarm(float distance) //TODO : 조명과 오디오로 알람
    {
        if (distance < 5f)
        {
            Debug.Log("물체가 가깝습니다!!");
        }
        else if (distance < 10f)
        {
            Debug.Log("물체가 감지되었습니다!");
        }
    }

    private void Reveal()
    {
        for (int i = detectedObjectList.Count - 1; i >= 0; i--)
        {
            if (detectedObjectList[i].StateType != PropStateType.DetectCompleted)
            {
                detectedObjectList[i].Detected();
                continue;
            }

            detectedObjectList[i].Revealed();
            detectedObjectList.RemoveAt(i);
        }
    }

    /// <summary>
    ///  감지 중 함수
    /// </summary>
    public void Detecting()
    {
        for (int i = 0; i < detectedObjectList.Count; i++)
        {
            detectedObjectList[i].Detecting();
        }

        coDetecting = StartCoroutine(CoDetecting());
    }

    /// <summary>
    /// 감지 완료 함수
    /// </summary>
    private void DetectCompleted()
    {
        for (int i = 0; i < detectedObjectList.Count; i++)
        {
            if (detectedObjectList[i].StateType == PropStateType.Detecting)
                detectedObjectList[i].DetectCompleted();
        }
    }

    /// <summary>
    /// 감지 스톱 함수
    /// </summary>
    private void StopDetecting()
    {
        if (coDetecting == null)
            return;

        StopCoroutine(coDetecting);
        coDetecting = null;
        curDetectTime = 0;
    }

    private IEnumerator CoDetecting()
    {
        while (true)
        {
            curDetectTime += Time.deltaTime;
            if (curDetectTime >= maxDetectTime)
            {
                break;
            }

            for (int i = 0; i < detectedObjectList.Count; i++)
            {
                if (detectedObjectList[i].StateType == PropStateType.Detecting)
                    detectedObjectList[i].SetFillAmount(curDetectTime / maxDetectTime);
            }

            yield return null;
        }

        DetectCompleted();
    }
}
