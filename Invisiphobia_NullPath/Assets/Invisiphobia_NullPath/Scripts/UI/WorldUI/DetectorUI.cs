using Common.Yield;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectorUI : WorldUI<TabletStateType>
{
    [SerializeField] private Image activeButton;
    [SerializeField] private Color32 uiColor;

    [SerializeField] private AudioClip alarmClip;
    [SerializeField] private Image popup;

    [Header("Detector")]
    [SerializeField] private TriggerDetector detector;                          //외부 콜라이더 Trigger 변수
    private List<IDetectable> detectedObjectList = new List<IDetectable>();     //감지한 객체 리스트
    private float updateInterval = 0.8f;                                        //코루틴 업데이트 주기 변수
    private float closestdistance;                                              //업데이트 때 제일 가까운 거리 저장 변수

    private Coroutine timer = null;                                             //코루틴 타이머 변수
        
    private Coroutine coDetecting;                                              //코루틴 감지 시간 변수
    private float curDetectTime = 0;                                            //현재 감지 시간 변수
    [SerializeField] private float maxDetectTime = 2f;                          //최대 감지 시간 변수

    private int layerMask;                                                      //벽 레이어 변수

    private void OnEnable()
    {
        StartTimer();
    }

    public override void Init(IActiveStatable<TabletStateType> subject)
    {
        layerMask = LayerMask.GetMask("Wall");

        popup.enabled = false;
        detector.EnterEvent += TriggerEnter;
        detector.ExitEvent += TriggerExit;
    }

    public override void Subscribe(IActiveStatable<TabletStateType> subject)
    {
        subject.BasicStateEvent += Reveal;
        subject.BasicStateEvent += StopDetecting;

        subject.ActiveStateEvent += Detecting;

        activeButton.color = Color.white;
    }

    public override void Unsubscribe(IActiveStatable<TabletStateType> subject)
    {
        Reveal();
        StopDetecting();

        subject.BasicStateEvent -= Reveal;
        subject.BasicStateEvent -= StopDetecting;

        subject.ActiveStateEvent -= Detecting;

        gameObject.SetActive(false);

        activeButton.color = uiColor;
    }

    /// <summary>
    /// 외부 TriggerEnter 실행 이벤트 함수
    /// </summary>
    private void TriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDetectable detectable))
        {
            if (detectable.StateType != PropStateType.Revealed)
            {
                detectable.Detected();

                if (HasLineOfSight(detectable))
                {
                    detectable.SetMapIconToWall(false);
                }
            }

            detectedObjectList.Add(detectable);
            detectable.IsDetectTablet = true;
        }
    }

    /// <summary>
    /// 외부 TriggerExit 실행 이벤트 함수
    /// </summary>
    private void TriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDetectable detectable))
        {
            detectedObjectList.Remove(detectable);
            detectable.IsDetectTablet = false;

            if (detectable.StateType == PropStateType.Revealed)
                return;

            detectable.Invisible();
        }
    }

    /// <summary>
    /// IDetectable과 Detecter사이에 Wall이 있는지 판단 함수
    /// </summary>
    private bool HasLineOfSight(IDetectable target)
    {
        Vector3 direction = (target.transform.position - detector.transform.position).normalized;
        float distance = Vector3.Distance(detector.transform.position, target.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(detector.transform.position, direction, out hit, distance, layerMask))
        {
            return true;
        }

        return false;
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

            for (int i = 0; i < detectedObjectList.Count; i++)
            {
                if (detectedObjectList[i].StateType != PropStateType.Detected)
                {
                    continue;
                }

                if (HasLineOfSight(detectedObjectList[i]))
                {
                    detectedObjectList[i].SetMapIconToWall(false);
                    continue;
                }

                detectedObjectList[i].SetMapIconToWall(true);
                UpdateDistances(detectedObjectList[i], ref closestdistance);
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
        float distance = Vector3.Distance(detector.transform.position, detectable.transform.position);
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
        if (distance <= 2f)
        {
            Debug.Log("물체가 가깝습니다!!");
            Managers.Sound.SFX2DPlay(alarmClip, 1.5f);
            StartCoroutine(Copopup());
        }
        else if (distance <= 6f)
        {
            Debug.Log("물체가 감지되었습니다!");
            Managers.Sound.SFX2DPlay(alarmClip);
            StartCoroutine(Copopup());
        }
    }

    private IEnumerator Copopup()
    {
        popup.enabled = true;
        yield return YieldCache.WaitForSeconds(0.5f);
        popup.enabled = false;
    }

    /// <summary>
    /// 감지 완료 후 테블릿 이동 시 보이게 하는 함수
    /// </summary>
    private void Reveal()
    {
        for (int i = detectedObjectList.Count - 1; i >= 0; i--)
        {
            if (detectedObjectList[i].StateType == PropStateType.Revealed)
                continue;
            else if (detectedObjectList[i].StateType == PropStateType.DetectCompleted)
            {
                detectedObjectList[i].Revealed();
                continue;
            }

            detectedObjectList[i].Detected();
        }
    }

    /// <summary>
    ///  감지 중 함수
    /// </summary>
    public void Detecting()
    {
        for (int i = 0; i < detectedObjectList.Count; i++)
        {
            if (detectedObjectList[i].StateType == PropStateType.Revealed)
                continue;

            if (HasLineOfSight(detectedObjectList[i]))
                continue;

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
