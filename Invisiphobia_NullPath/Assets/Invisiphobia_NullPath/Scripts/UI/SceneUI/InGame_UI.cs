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
    [SerializeField] private MainPanelManager pausePanelManager;
    public bool isPause = false;

    public override void Init()
    {
        base.Init();

        Player player = EntityManager.Instance.Player;
        player.PlayerController.playerEscActionEvent += SetPause;
        player.PlayerMovement.SetUI(staminaBar, sprintBarCanvasGroup);
        player.PlayerInteract.interactUIEvent += SetInteractDescriptionKey;
        player.PlayerInventory.OnHandItemChanged += SetInteractKey;

        mainPanelManager.OpenFirstTab();

        EventManager.Subscribe(GameEventType.UseCrossHair, OnSetCrossHairActive);
    }

    private void OnSetCrossHairActive(object args)
    {
        crossHair.SetActive(!(bool)args);
    }

    private void SetStaminaBar(int sprintRemaining)
    {
        staminaBar.value = sprintRemaining; // 현재 스태미나 동기화
    }

    public void SetPause()
    {
        if (mainPanelManager.currentPanelIndex == 0 && !isPause)
        {
            mainPanelManager.OpenPanel("Pause");
            EventManager.Dispatch(GameEventType.UseInput, isPause);
            EventManager.Dispatch(GameEventType.UseTabletPause, !isPause);
            EventManager.Dispatch(GameEventType.UseMonsterPause, !isPause);
            isPause = true;
        }
        else if (mainPanelManager.currentPanelIndex == 1 && pausePanelManager.currentPanelIndex == 0 && isPause)
        {
            mainPanelManager.OpenFirstTab();
            EventManager.Dispatch(GameEventType.UseInput, isPause);
            EventManager.Dispatch(GameEventType.UseTabletPause, !isPause);
            EventManager.Dispatch(GameEventType.UseMonsterPause, !isPause);
            isPause = false;
        }
    }

    private void SetInteractDescriptionKey(IInteractable interact)
    {
        if (interact == null)
            interactDescriptionKeyText.text = "";
        else
            interactDescriptionKeyText.text = $"{interact.InteractText}";
    }

    private void SetInteractKey(IInteractable interact)
    {
        if(interact == null)
        {
            interactKeyText.text = "";
        }
        else
        {
            interactKeyText.text = "[Right-click + Left-click]\n: " + interact.ActionText;
        }
    }

    public void GoTitle()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }

    protected override void OnDestroy()
    {
        EventManager.Unsubscribe(GameEventType.UseCrossHair, OnSetCrossHairActive);
        base.OnDestroy();
    }
}
