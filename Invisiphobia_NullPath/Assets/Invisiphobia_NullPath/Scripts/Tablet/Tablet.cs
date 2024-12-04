using UnityEngine;

public enum TabletStateType
{
    Hidden,
    Idle,
    Active
}

public class Tablet : MonoBehaviour 
{
    public static Tablet Instance { get; private set; }

    [SerializeField] private TabletController controller;
    [SerializeField] private TabletUIController uiController;
    [SerializeField] private Detector detector;

    public TabletStateType state {  get; private set; }

    public event System.Action<TabletStateType> OnStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        controller = GetComponent<TabletController>();
        uiController = GetComponent<TabletUIController>();
        detector = GetComponent<Detector>();

        OnStateChanged += controller.HandleStateChanged;
        OnStateChanged += uiController.HandleStateChanged;
    }

    private void Start()
    {
        SetTabletState(TabletStateType.Idle);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Tab 키로 태블릿 활성화/비활성화
        {
            Debug.Log("Tab키 입력");
            ToggleTabletState();
        }
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