using Common.Data;
using Common.Event;
using Common.SceneEx;
using DG.Tweening;
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
    [SerializeField] private TextMeshProUGUI interactTabletKeyText;
    [SerializeField] private TextMeshProUGUI interactDescriptionKeyText;
    [SerializeField] private TextMeshProUGUI errorMessageText;
    private Tween errorMessageTween;

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
        player.PlayerInteract.errorMessageUIEvent += SetErrorMessageText;
        player.PlayerInventory.OnHandItemChanged += SetInteractKey;

        mainPanelManager.OpenFirstTab();

        interactTabletKeyText.gameObject.SetActive(false);

        EventManager.Subscribe(GameEventType.UseCrossHair, OnSetCrossHairActive);
        EventManager.Subscribe(GameEventType.UseWheelClick, OnSetWheelClickActive);
    }

    private void OnSetCrossHairActive(object args)
    {
        crossHair.SetActive(!(bool)args);
    }

    private void OnSetWheelClickActive(object args)
    {
        interactTabletKeyText.gameObject.SetActive((bool)args);
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
            EventManager.Dispatch(GameEventType.UseTabletPause, !isPause);
            EventManager.Dispatch(GameEventType.UseInput, isPause);
            EventManager.Dispatch(GameEventType.UseMonsterPause, !isPause);
            isPause = true;
        }
        else if (mainPanelManager.currentPanelIndex == 1 && pausePanelManager.currentPanelIndex == 0 && isPause)
        {
            mainPanelManager.OpenFirstTab();
            EventManager.Dispatch(GameEventType.UseTabletPause, !isPause);
            EventManager.Dispatch(GameEventType.UseInput, isPause);
            EventManager.Dispatch(GameEventType.UseMonsterPause, !isPause);
            crossHair.SetActive(true);
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
            interactKeyText.text = "[R-click + L-click] : " + interact.ActionText;
        }
    }

    private void SetErrorMessageText(IErrorMessageable errorMessage)
    {
        if(errorMessageTween != null)
        {
            errorMessageTween.Kill();
            errorMessageTween = null;
        }    

        if (errorMessage == null)
            errorMessageText.text = "";
        else
        {
            errorMessageText.text = $"{errorMessage.ErrorMessageText}";
            errorMessageTween = errorMessageText.DOFade(0, 1.5f).OnComplete(()=> errorMessageText.text = "");
        }
    }

    public void GoTitle()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }

    protected override void OnDestroy()
    {
        EventManager.Unsubscribe(GameEventType.UseCrossHair, OnSetCrossHairActive);
        EventManager.Unsubscribe(GameEventType.UseWheelClick, OnSetWheelClickActive);
        base.OnDestroy();
    }
}
