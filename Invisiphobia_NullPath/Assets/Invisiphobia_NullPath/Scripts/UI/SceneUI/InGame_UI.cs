using Common.Data;
using Common.Event;
using Common.SceneEx;
using Michsky.UI.Dark;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGame_UI : BaseSceneUI
{
    [Header("Stamina")]
    [SerializeField] private Slider staminaBar;
    [SerializeField] private CanvasGroup sprintBarCanvasGroup; // CanvasGroup 추가
    public bool hideBarWhenFull = true;

    [Header("CrossHair")]
    [SerializeField] private GameObject crossHair;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI interactKeyText;
    [SerializeField] private TextMeshProUGUI interactDescriptionKeyText;
    [SerializeField] private TextMeshProUGUI useDescriptionKey;

    [Header("Panel")]
    [SerializeField] private MainPanelManager mainPanelManager;
    public bool isPause = false;

    #region Test
    public void Start()
    {
        if(SceneManager.GetActiveScene().name != "InGame_Scene")
        {
            Init();
        }
    }
    #endregion

    public override void Init()
    {
        base.Init();

        EntityManager.Instance.Player.PlayerMovement.SetUI(staminaBar, sprintBarCanvasGroup);
        EntityManager.Instance.Player.PlayerInteract.interactUIEvent += SetInteractDescriptionKey;
        EntityManager.Instance.Player.PlayerInventory.OnHandItemChanged += SetInteractKey;

        mainPanelManager.OpenFirstTab();
    }

    private void OnSetCrossHairActive(object args)
    {
        crossHair.SetActive((bool)args);
    }

    private void SetStaminaBar(int sprintRemaining)
    {
        staminaBar.value = sprintRemaining; // 현재 스태미나 동기화
    }

    public void SetPause()
    {
        EventManager.Dispatch(GameEventType.UseInput, isPause);
        isPause = !isPause;

        Debug.Log("in2");
    }

    private void SetInteractDescriptionKey(IInteractable interact)
    {
        if (interact == null)
            interactDescriptionKeyText.text = "";
        else
            interactDescriptionKeyText.text = interact.InteractText;
    }

    private void SetInteractKey(IInteractable interact)
    {
        if(interact == null)
        {
            interactKeyText.text = "";
        }
        else
        {
            interactKeyText.text = "[Shift + right-click]\n: " + interact.ActionText;
        }
    }

    public void GoTitle()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }
}
