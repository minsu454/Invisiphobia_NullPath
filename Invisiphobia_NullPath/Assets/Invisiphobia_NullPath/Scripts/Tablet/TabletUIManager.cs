using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TabletUIManager : MonoBehaviour, IActiveStatable<TabletStateType>
{
    [SerializeField] private List<WorldUI<TabletStateType>> worldUIList = new List<WorldUI<TabletStateType>>();
    private int choiceIdx = 0;
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

    public event Action BasicStateEvent;
    public event Action ActiveStateEvent;
    public event Action<TabletStateType> ShotEvent;

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

        choiceIdx = num;
    }

    private void OnShot(TabletStateType type)
    {
        ShotEvent?.Invoke(type);
    }
}