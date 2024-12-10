using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : WorldUI<TabletStateType>
{
    [SerializeField] private GameObject progressBackground;         //진행 바 배경 변수
    [SerializeField] private Image progressBar;                     //진행 바 변수

    private Coroutine coProgress;                                   //코루틴 감지 시간 변수
    private float curProgressTime = 0;                              //현재 감지 시간 변수
    [SerializeField] private float maxProgressTime = 2f;            //최대 감지 시간 변수

    private bool isShotable = false;

    public override void Init(IActiveStatable<TabletStateType> subject)
    {
        ResetProgress();
    }

    public override void Subscribe(IActiveStatable<TabletStateType> subject)
    {
        coProgress = StartCoroutine(CoProgress());

        subject.ShotEvent += OnShot;
    }

    public override void Unsubscribe(IActiveStatable<TabletStateType> subject)
    {
        if (coProgress != null)
            StopCoroutine(coProgress);

        subject.ShotEvent -= OnShot;

        gameObject.SetActive(false);    
    }

    private void OnShot(TabletStateType type)
    {
        if (type != TabletStateType.Activate)
            return;

        if (!isShotable)
            return;

        Debug.Log("찰칵");
        isShotable = false;
        coProgress = StartCoroutine(CoProgress());
    }

    private IEnumerator CoProgress()
    {
        ResetProgress();
        
        progressBackground.SetActive(true);
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
        progressBackground.SetActive(false);
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
