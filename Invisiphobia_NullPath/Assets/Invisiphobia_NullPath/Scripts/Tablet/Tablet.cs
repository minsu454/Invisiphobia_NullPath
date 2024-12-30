using Common.Event;
using Common.Yield;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Tablet : MonoBehaviour 
{
    [Header("Controller")]
    [SerializeField] private TabletController controller;           //태블릿 컨트롤러
    [SerializeField] private TabletUIController uiController;       //태블릿 UI컨트롤러

    [Header("Manager")]
    [SerializeField] private TabletUIManager manager;               //태블릿 UI매니저

    [Header("Battery Settings")]
    [SerializeField] private float maxCharge = 100f;                //배터리 최대 값
    [SerializeField] private float currentCharge;                   //현재 배터리 값
    [SerializeField] private float consumptionRate;                 //소모주기
    [SerializeField] private float consumptionAmount;               //주기마다 소모되는 값
    private Coroutine myCoroutine;                                  //배터리 소모 코루틴 저장

    private bool isCharged = true;                                  //배터리가 있는지 확인해주는 bool
    public bool IsCharged { get { return isCharged; } }

    private bool useSwitchScreen = true;                             //스크린 스위치 가능한 상태인지 확인하는 bool

    public event Action<TabletStateType> OnStateChangedEvent;       //스텟 바뀔 때에 이벤트
    public event Action<TabletStateType> OnShotEvent;               //태블릿 사용 이벤트
    private TabletStateType stateType = TabletStateType.Basic;      //태블릿 상태 타입
    public TabletStateType State {
        get { return stateType; }
        private set
        {
            if (stateType == value)
            {
                return;
            }

            stateType = value;
            OnStateChangedEvent?.Invoke(stateType);
        }
    }
    private bool useStateChange = true;                             //태블릿 State전환 사용여부

    private event Func<int> ItemCountEvent;                         //아이템 카운트 가져오는 이벤트

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init(Player player)
    {
        manager.Init(this);

        controller.Init(this);
        uiController.Init(this);

        player.PlayerController.playerTabletActionEvent += ToggleTabletState;
        player.PlayerController.tabletSwitchActionEvent += OnSwitchTabletScreen;
        player.PlayerController.playerClickActionEvent += OnClick;

        ItemCountEvent += player.PlayerInventory.Count;

        SetCurrentCharge(maxCharge);
    }

    /// <summary>
    /// 스텟 바꿔주는 함수
    /// </summary>
    private void ToggleTabletState()
    {
        if (!useStateChange)
            return;

        switch (State)
        {
            case TabletStateType.Basic:
                State = TabletStateType.Activate;
                break;
            case TabletStateType.Activate:
                State = TabletStateType.Basic;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 숨김 상태 함수
    /// </summary>
    public void Hidden()
    {
        OnSwitchTabletScreen(0);
        State = TabletStateType.Hidden;
    }

    /// <summary>
    /// 숨김 해제 함수
    /// </summary>
    public void UnHidden()
    {
        int itemCount = ItemCountEvent.Invoke();

        if (itemCount >= 2)
            return;

        State = TabletStateType.Basic;
    }

    /// <summary>
    /// 퍼즐 생성 함수
    /// </summary>
    public int InitPuzzle(string path, Action onCompleted)
    {
        WorldUI<TabletStateType> worldUI = manager.PuzzleInstantiate(path, StopPuzzle + onCompleted);
        int idx = manager.IndexOf(worldUI);

        if (idx == -1)
            Debug.LogError($"not Found : {worldUI}");

        return idx;
    }

    /// <summary>
    /// 퍼즐 플레이 함수
    /// </summary>
    public void PlayPuzzle(int index)
    {
        useStateChange = false;
        useSwitchScreen = false;
        manager.ChoiceIdx = index;
        State = TabletStateType.Activate;
        EventManager.Dispatch(GameEventType.UseMove, false);
        EventManager.Dispatch(GameEventType.UseEsc, false);
    }

    /// <summary>
    /// 퍼즐 멈추는 함수
    /// </summary>
    public void StopPuzzle()
    {
        State = TabletStateType.Basic;
        useStateChange = true;
        useSwitchScreen = true;
        OnSwitchTabletScreen(0);
        EventManager.Dispatch(GameEventType.UseEsc, true);
        EventManager.Dispatch(GameEventType.UseMove, true);
    }

    #region 배터리관련
    public float GetCurrentCharge()
    {
        return currentCharge;
    }

    public float GetMaxCharge()
    {
        return maxCharge;
    }

    public void SetCurrentCharge(float value)
    {
        currentCharge = value;
        if(currentCharge > 0 && !isCharged)
        {
            isCharged = true;
            useStateChange = true;
            useSwitchScreen = true;
            OnSwitchTabletScreen(0);
        }
        Consomtion();
        UpdateBatteryUI();
    }

    private void Consomtion()
    {
        if(myCoroutine == null)
        {
            myCoroutine = StartCoroutine(CoConsumption());
        }
        else
        {
            StopCoroutine(myCoroutine);
            myCoroutine = StartCoroutine(CoConsumption());
        } 
    }

    private IEnumerator CoConsumption()
    {
        while(currentCharge > 0)
        {
            yield return YieldCache.WaitForSeconds(consumptionRate);
            currentCharge -= consumptionAmount;
            UpdateBatteryUI();
        }
        OffScreen();
        // 배터리고갈함수
    }

    public void UpdateBatteryUI()
    {
        float amount = Mathf.Clamp01(currentCharge / maxCharge);

        manager.UpdateBattery(amount);
    }

    private void OffScreen()
    {
        isCharged = false;
        useStateChange = false;
        useSwitchScreen = false;
        EventManager.Dispatch(GameEventType.UseMove, true);
        OnSwitchTabletScreen(2);
    }
    #endregion

    /// <summary>
    /// tablet 스크린 변환 함수
    /// </summary>
    private void OnSwitchTabletScreen(int num)
    {
        if (useSwitchScreen)
        {
            manager.ChoiceIdx = num;
        }
    }

    /// <summary>
    /// 클릭 이벤트 함수
    /// </summary>
    private void OnClick()
    {
        OnShotEvent.Invoke(State);
    }
}