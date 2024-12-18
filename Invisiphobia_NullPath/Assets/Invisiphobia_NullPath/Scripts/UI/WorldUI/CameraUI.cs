using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : WorldUI<TabletStateType>
{
    [SerializeField] private GameObject progressBackground;     //진행 바 배경 변수
    [SerializeField] private Image progressBar;                 //진행 바 변수

    [SerializeField] private Image activeButton;
    [SerializeField] private Color32 uiColor;

    private Coroutine coProgress;                               //코루틴 감지 시간 변수
    private float curProgressTime = 0;                          //현재 감지 시간 변수
    [SerializeField] private float maxProgressTime = 2f;        //최대 감지 시간 변수

    private bool isShotable = false;                            //공격 가능한 상태인지 저장 변수

    [Header("Camera")]
    [SerializeField] private Camera tabletCamera;               //테블릿 화면전용 카메라
    [SerializeField] private float maxDistance = 50f;           // 거리 제한

    public override void Init(IActiveStatable<TabletStateType> subject)
    {
        ResetProgress();
    }

    public override void Subscribe(IActiveStatable<TabletStateType> subject)
    {
        coProgress = StartCoroutine(CoProgress());

        subject.BasicStateEvent += OnBasicState;
        subject.ActiveStateEvent += OnActiveState;

        subject.ShotEvent += OnShot;
        progressBackground.SetActive(true);
        activeButton.color = Color.white;
    }

    public override void Unsubscribe(IActiveStatable<TabletStateType> subject)
    {
        if (coProgress != null)
            StopCoroutine(coProgress);

        subject.BasicStateEvent -= OnBasicState;
        subject.ActiveStateEvent -= OnActiveState;

        subject.ShotEvent -= OnShot;

        gameObject.SetActive(false);
        activeButton.color = uiColor;
    }

    /// <summary>
    /// 좌클릭시 카메라 찍는 함수
    /// </summary>
    private void OnShot(TabletStateType type)
    {
        if (type != TabletStateType.Activate)
            return;

        if (!isShotable)
            return;

        foreach (Monster monster in EntityManager.Instance.monsterTestList)
        {
            Vector3 viewportPos = tabletCamera.WorldToViewportPoint(monster.gameObject.transform.position);

            if (viewportPos.x < 0 || viewportPos.x > 1 ||
                viewportPos.y < 0 || viewportPos.y > 1 ||
                viewportPos.z <= 0)
            {
                continue;
            }

            float distance = Vector3.Distance(tabletCamera.transform.position, tabletCamera.transform.position);

            if (distance > maxDistance)
            {
                continue;
            }

            monster.MyController.PlayerAttackMonster();
        }

        isShotable = false;
        coProgress = StartCoroutine(CoProgress());
    }

    /// <summary>
    /// 기본 상태일 때 사용 이벤트 함수
    /// </summary>
    private void OnBasicState()
    {
        progressBackground.SetActive(true);
    }

    /// <summary>
    /// 활성화 상태일 때 사용 이벤트 함수
    /// </summary>
    private void OnActiveState()
    {
        progressBackground.SetActive(false);
    }

    /// <summary>
    /// 차징 중 업데이트 사용 코루틴
    /// </summary>
    private IEnumerator CoProgress()
    {
        ResetProgress();
        
        while (true)
        {
            curProgressTime += Time.deltaTime;
            if (curProgressTime >= maxProgressTime)
            {
                break;
            }

            SetFillAmount(curProgressTime / maxProgressTime);

            yield return null;
        }

        isShotable = true;
    }

    /// <summary>
    /// 차징 리셋해주는 함수
    /// </summary>
    private void ResetProgress()
    {
        isShotable = false;
        curProgressTime = 0;
        SetFillAmount(0);
    }

    /// <summary>
    /// 진행 바 채워주는 함수
    /// </summary>
    private void SetFillAmount(float value)
    {
        progressBar.fillAmount = value;
    }
}
