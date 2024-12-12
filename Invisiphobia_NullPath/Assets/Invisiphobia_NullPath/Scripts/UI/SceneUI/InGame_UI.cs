using Common.Data;
using Common.SceneEx;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGame_UI : BaseSceneUI
{
    [SerializeField] private Slider staminaBar;
    [SerializeField] private CanvasGroup sprintBarCanvasGroup; // CanvasGroup 추가
    public bool hideBarWhenFull = true;

    [SerializeField] private TextMeshProUGUI interactDescriptionKeyText;
    [SerializeField] private TextMeshProUGUI useDescriptionKey;

    #region Test
    public void Start()
    {
        if(SceneManager.GetActiveScene().name != "InGame")
        {
            Init();
        }
    }
    #endregion

    public override void Init()
    {
        base.Init();

        Player.Instance.PlayerMovement.SetUI(staminaBar, sprintBarCanvasGroup);
        Player.Instance.PlayerInteract.interactUIEvent += SetInteractDescriptionKey;
    }

    private void SetStaminaBar(int sprintRemaining)
    {
        staminaBar.value = sprintRemaining; // 현재 스태미나 동기화
    }

    public void Btn()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }

    private void SetInteractDescriptionKey(IInteractable interact)
    {
        if (interact == null)
            interactDescriptionKeyText.text = "";
        else
            interactDescriptionKeyText.text = interact.InteractText;
    }
}
