using System.Collections;
using UnityEngine;

public class Tablet : MonoBehaviour 
{
    [Header("Controller")]
    [SerializeField] private TabletController controller;
    [SerializeField] private TabletUIController uiController;

    [Header("Managerr")]
    [SerializeField] private TabletUIManager manager;

    public event System.Action<TabletStateType> OnStateChangedEvent;
    private TabletStateType stateType = TabletStateType.Basic;
    public TabletStateType state {
        get { return stateType; }
        private set
        {
            if (state == value)
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
    }

    /// <summary>
    /// 스텟 바꿔주는 함수
    /// </summary>
    private void ToggleTabletState()
    {
        switch (state)
        {
            case TabletStateType.Basic:
                state = TabletStateType.Activate;
                break;
            case TabletStateType.Activate:
                state = TabletStateType.Basic;
                break;
        }
    }

    /// <summary>
    /// tablet 스크린 변환 함수
    /// </summary>
    private void OnSwitchTabletScreen(int num)
    {
        manager.ChoiceIdx = num;
    }
}