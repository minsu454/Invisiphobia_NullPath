using Common.Yield;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Tablet : MonoBehaviour 
{
    [Header("Controller")]
    [SerializeField] private TabletController controller;
    [SerializeField] private TabletUIController uiController;

    [Header("Manager")]
    [SerializeField] private TabletUIManager manager;

    [Header("Battery Settings")]
    [SerializeField] private float maxCharge = 100f;
    [SerializeField] private float currentCharge;
    [SerializeField] private float consumptionRate;
    [SerializeField] private float consumptionAmount;

    private bool isCharged = true;

    public event Action<TabletStateType> OnStateChangedEvent;
    public event Action<TabletStateType> OnShotEvent;
    private TabletStateType stateType = TabletStateType.Basic;
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

    public void Init(Player player)
    {
        manager.Init(this);

        controller.Init(this);
        uiController.Init(this);

        player.PlayerController.playerTabletActionEvent += ToggleTabletState;
        player.PlayerController.tabletSwitchActionEvent += OnSwitchTabletScreen;
        player.PlayerController.playerClickActionEvent += OnClick;

        SetCurrentCharge(maxCharge);
    }

    private void Start()
    {
    }

    /// <summary>
    /// 스텟 바꿔주는 함수
    /// </summary>
    private void ToggleTabletState()
    {
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

    public void Hidden()
    {
        State = TabletStateType.Hidden;
    }

    public void UnHidden()
    {
        State = TabletStateType.Basic;
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
            //manager.ChoiceIdx = 0;
            isCharged = true;
            OnSwitchTabletScreen(0);
        }
        // UI 업데이트
        StartCoroutine(CoConsumption());
    }

    private IEnumerator CoConsumption()
    {
        while(currentCharge > 0)
        {
            yield return YieldCache.WaitForSeconds(consumptionRate);
            currentCharge -= consumptionAmount;

            Debug.Log($"Battery Charge: {currentCharge}/{maxCharge}");
        }

        OffScreen();
        // 배터리고갈함수
    }

    private void OffScreen()
    {
        isCharged = false;
        manager.ChoiceIdx = 3;
    }
    #endregion

    /// <summary>
    /// tablet 스크린 변환 함수
    /// </summary>
    private void OnSwitchTabletScreen(int num)
    {
        if (isCharged)
        {
            manager.ChoiceIdx = num;
        }
    }

    private void OnClick()
    {
        OnShotEvent.Invoke(State);
    }
}