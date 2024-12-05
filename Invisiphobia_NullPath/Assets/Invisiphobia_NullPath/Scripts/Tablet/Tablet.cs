using UnityEngine;

public class Tablet : MonoBehaviour 
{
    [SerializeField] private TabletController controller;
    [SerializeField] private TabletUIController uiController;
    [SerializeField] private Detector detector;

    public TabletStateType state {  get; private set; }

    public event System.Action<TabletStateType> OnStateChanged;

    private void Init(Player player)
    {
        controller.Init(this);
        uiController.Init(this);

        player.PlayerController.playerTabletActionEvent += ToggleTabletState;
    }

    private void Start()
    {
        SetTabletState(TabletStateType.Idle);
    }

    //public void ActivateTablet()
    //{
    //    uiController.ApplyMapSize();
    //}

    private void ToggleTabletState()
    {
        if(state == TabletStateType.Idle)
        {
            SetTabletState(TabletStateType.Active);
        }
        else if(state == TabletStateType.Active)
        {
            detector.Reveal();
            SetTabletState(TabletStateType.Idle);
        }
    }

    private void SetTabletState(TabletStateType newState)
    {
        if (state != newState)
        {
            state = newState;
            OnStateChanged?.Invoke(newState);
        }
    }

}