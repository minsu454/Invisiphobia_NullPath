using Common.Event;
using Common.Objects;
using Common.VolumeEx;
using Common.Yield;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TabletUIManager : MonoBehaviour, IActiveStatable<TabletStateType>
{
    [SerializeField] private List<WorldUI<TabletStateType>> worldUIList = new List<WorldUI<TabletStateType>>();

    [SerializeField] private List<GameObject> UpdateUIList = new List<GameObject>();

    [SerializeField] private Volume volume;
    private LimitlessGlitch8 glitch;

    [SerializeField] private Image batteryBar; 
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
    public event Action<bool> UsePauseEvent;
    public event Action<TabletStateType> ShotEvent;
    private event Action hiddenEvent;

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

        volume = VolumeManagerEx.Volume;
        volume.profile.TryGet(out glitch);
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
        StartCoroutine(CoGlitch());
        worldUIList[ChoiceIdx].Unsubscribe(this);

        worldUIList[num].gameObject.SetActive(true);
        worldUIList[num].Subscribe(this);

        hiddenEvent.Invoke();

        choiceIdx = num;
    }
    
    /// <summary>
    /// 퍼즐 생성 함수
    /// </summary>
    public PuzzleUI PuzzleInstantiate(string path, Action oncompleted)
    {
        GameObject go = ObjectManager.Instantiate(path, transform);

        PuzzleUI puzzleUI = go.GetComponent<PuzzleUI>();
        puzzleUI.Init(this);
        Add(puzzleUI);

        puzzleUI.OnCompletedEvent += oncompleted;
        puzzleUI.OnDestroyEvent += Remove;

        return puzzleUI;
    }

    /// <summary>
    /// 인덱스 반환 함수
    /// </summary>
    public int IndexOf(WorldUI<TabletStateType> worldUI)
    {
        return worldUIList.IndexOf(worldUI);
    }

    /// <summary>
    /// UI 추가해주는 함수
    /// </summary>
    public void Add(WorldUI<TabletStateType> worldUI)
    {
        worldUIList.Add(worldUI);
    }

    /// <summary>
    /// UI 지워주는 함수
    /// </summary>
    public void Remove(WorldUI<TabletStateType> worldUI)
    {
        worldUIList.Remove(worldUI);
    }

    /// <summary>
    /// 퍼즈 사용 여부 함수
    /// </summary>
    public void UsePause(bool value)
    {
        UsePauseEvent?.Invoke(value);
    }

    /// <summary>
    /// 공격 시 사용 할 이벤트 함수
    /// </summary>
    private void OnShot(TabletStateType type)
    {
        ShotEvent?.Invoke(type);
    }

    public void UpdateBattery(float amount)
    {
        batteryBar.fillAmount = amount;
    }

    public void UpgradePopup(int index)
    {
        StartCoroutine(CoUpdate(index));
    }

    private IEnumerator CoUpdate(int index)
    {
        StartCoroutine(CoGlitch());
        GameObject updateUI = UpdateUIList[index];
        updateUI.SetActive(true);
        EventManager.Dispatch(GameEventType.UseTabletInput, false);
        yield return YieldCache.WaitForSeconds(4f);
        StartCoroutine(CoGlitch());
        EventManager.Dispatch(GameEventType.UseTabletInput, true);
        updateUI.SetActive(false);
    }

    private IEnumerator CoGlitch()
    {
        glitch.active = true;

        yield return YieldCache.WaitForSeconds(0.2f);

        glitch.active = false;
    }

    
}