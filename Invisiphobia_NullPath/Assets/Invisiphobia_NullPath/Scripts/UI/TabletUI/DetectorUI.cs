using Common.Yield;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorUI : WorldUI
{
    [Header("Detector")]
    [SerializeField] private TriggerDetector detector;                          //외부 콜라이더 Trigger 변수
    private List<IDetectable> detectedObjectList = new List<IDetectable>();     //감지한 객체 리스트
    private float updateInterval = 1f;                                          //코루틴 업데이트 주기 변수
    private float closestdistance;                                              //업데이트 때 제일 가까운 거리 저장 변수

    private Coroutine timer = null;                                             //코루틴 타이머 변수
        
    private Coroutine coDetecting;                                              //코루틴 감지 시간 변수
    private float curDetectTime = 0;                                            //현재 감지 시간 변수
    [SerializeField] private float maxDetectTime = 2f;                          //최대 감지 시간 변수

    private int layerMask;

    private void OnEnable()
    {
        StartTimer();
    }

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

    /// <summary>
    /// 외부 TriggerEnter 실행 이벤트 함수
    /// </summary>
    private void TriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDetectable detectable))
        {
            if (detectable.StateType == PropStateType.Revealed)
                return;

            detectable.Detected();
            detectedObjectList.Add(detectable);
        }
    }

    /// <summary>
    /// 외부 TriggerExit 실행 이벤트 함수
    /// </summary>
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

    /// <summary>
    /// IDetectable과 Detecter사이에 Wall이 있는지 판단 함수
    /// </summary>
    private bool HasLineOfSight(IDetectable target)
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

    /// <summary>
    /// 타이머 코루틴 시작해주는 함수
    /// </summary>
    private void StartTimer()
    {
        if (timer != null)
            return;

        timer = StartCoroutine(CoCheckTimer());
    }

    /// <summary>
    /// 타이머 코루틴 멈추는 함수
    /// </summary>
    private void StopTimer()
    {
        if (timer == null)
            return;

        StopCoroutine(timer);
        timer = null;
    }

    /// <summary>
    /// 일정 거리 체크 코루틴
    /// </summary>
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

    /// <summary>
    /// 해당 오브젝트 중 제일 가까운 것 저장해주는 함수
    /// </summary>
    private void UpdateDistances(IDetectable detectable, ref float closest)
    {
        float distance = Vector3.Distance(transform.position, detectable.transform.position);
        if (distance < closest)
        {
            closest = distance;
        }
    }

    /// <summary>
    /// 물체 감지 알림 함수
    /// </summary>
    private void HandleAlarm(float distance) //TODO : 조명과 오디오로 알람
    {
        if (distance < 5f)
        {
            Debug.Log("물체가 가깝습니다!!");
        }
        else
        {
            Debug.Log("물체가 감지되었습니다!");
        }
    }

    /// <summary>
    /// 감지 완료 후 테블릿 이동 시 보이게 하는 함수
    /// </summary>
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

    /// <summary>
    /// 감지 바 채워주는 함수
    /// </summary>
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

    private void OnDisable()
    {
        StopTimer();
    }
}
