using Common.Data;
using Common.SceneEx;
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
    }

    private void OnSetCrossHairActive(object args)
    {
        crossHair.SetActive((bool)args);
    }

    private void SetStaminaBar(int sprintRemaining)
    {
        staminaBar.value = sprintRemaining; // 현재 스태미나 동기화
    }

    public void CreatePausePopup()
    {
        Managers.UI.CreatePopup<PausePopup>();
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
}
