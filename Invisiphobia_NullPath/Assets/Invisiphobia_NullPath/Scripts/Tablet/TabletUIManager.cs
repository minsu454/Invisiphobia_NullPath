using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TabletUIManager : MonoBehaviour, IActiveStatable<TabletStateType>
{
    [SerializeField] private List<WorldUI<TabletStateType>> worldUIList = new List<WorldUI<TabletStateType>>(); //타블렛에 들어가는 능력 리스트
    private int choiceIdx = 0;                                                                                  //현재 선택중인 인덱스                            
    public int ChoiceIdx    
    {
        get { return choiceIdx; }
        set
        {
            if (choiceIdx == value)
            {
                return;
            }

            SwitchTabletScreen(value);
        }
    }

    [SerializeField] private Image batteryBar;          //배터리 바

    public event Action BasicStateEvent;
    public event Action ActiveStateEvent;
    public event Action<TabletStateType> ShotEvent;
    private event Action hiddenEvent;                   //숨김 이벤트

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init(Tablet tablet)
    {
        foreach (WorldUI<TabletStateType> worldUI in worldUIList)
        {
            worldUI.Init(this);
            worldUI.gameObject.SetActive(false);
        }

        choiceIdx = 0;

        worldUIList[choiceIdx].gameObject.SetActive(true);
        worldUIList[choiceIdx].Subscribe(this);

        tablet.OnStateChangedEvent += OnStateChanged;
        tablet.OnShotEvent += OnShot;
        hiddenEvent += tablet.UnHidden;
    }

    /// <summary>
    /// 타블렛 상태 변환 시 실행 함수
    /// </summary>
    private void OnStateChanged(TabletStateType type)
    {
        switch (type)
        {
            case TabletStateType.Basic:
                BasicStateEvent?.Invoke();
                break;
            case TabletStateType.Activate:
                ActiveStateEvent?.Invoke();
                break;
        }
    }

    /// <summary>
    /// 타블렛 스크린 전환 함수
    /// </summary>
    private void SwitchTabletScreen(int num)
    {
        worldUIList[ChoiceIdx].Unsubscribe(this);

        worldUIList[num].gameObject.SetActive(true);
        worldUIList[num].Subscribe(this);

        hiddenEvent.Invoke();

        choiceIdx = num;
    }

    /// <summary>
    /// 좌클릭 사용 이벤트 연결
    /// </summary>
    private void OnShot(TabletStateType type)
    {
        ShotEvent?.Invoke(type);
    }

    /// <summary>
    /// 배터리 업데이트 함수
    /// </summary>
    public void UpdateBattery(float amount)
    {
        batteryBar.fillAmount = amount;
    }
}