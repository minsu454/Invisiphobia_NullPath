using System.Collections;
using UnityEngine;

public class Tablet : MonoBehaviour 
{
    [Header("Controller")]
    [SerializeField] private TabletController controller;
    [SerializeField] private TabletUIController uiController;

    [Header("Detecting")]
    [SerializeField] private Detector detector;

    private TabletStateType stateType = TabletStateType.Idle;
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

    public event System.Action<TabletStateType> OnStateChangedEvent;

    public void Init(Player player)
    {
        controller.Init(this);
        uiController.Init(this);
        detector.Init(this);

        player.PlayerController.playerTabletActionEvent += ToggleTabletState;
    }

    //public void ActivateTablet()
    //{
    //    uiController.ApplyMapSize();
    //}

    private void ToggleTabletState()
    {
        switch (state)
        {
            case TabletStateType.Idle:
                state = TabletStateType.Active;
                break;
            case TabletStateType.Active:
                state = TabletStateType.Idle;
                break;
        }
    }

}