using JetBrains.Annotations;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : WorldUI<TabletStateType>
{
    [SerializeField] private Image activeButton;
    [SerializeField] private Color32 uiColor;

    [SerializeField] private GameObject progressBackground;         //진행 바 배경 변수
    [SerializeField] private Image progressBar;                     //진행 바 변수

    private Coroutine coProgress;                                   //코루틴 감지 시간 변수
    private float curProgressTime = 0;                              //현재 감지 시간 변수
    [SerializeField] private float maxProgressTime = 2f;            //최대 감지 시간 변수

    private bool isShotable = false;

    [Header("Camera")]
    [SerializeField] private Camera tabletCamera;
    [SerializeField] private float maxDistance = 50f;               // 거리 제한

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

    private void OnBasicState()
    {
        progressBackground.SetActive(true);
    }

    private void OnActiveState()
    {
        progressBackground.SetActive(false);
    }

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
