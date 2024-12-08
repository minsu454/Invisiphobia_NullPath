using Common.SceneEx;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI : BaseSceneUI
{
    [SerializeField] private Slider staminaBar;
    [SerializeField] private CanvasGroup sprintBarCanvasGroup; // CanvasGroup 추가
    public bool hideBarWhenFull = true;

    public override void Init()
    {
        base.Init();

        Player.Instance.PlayerMovement.SetUI(staminaBar, sprintBarCanvasGroup);
    }

    private void SetStaminaBar(int sprintRemaining)
    {
        staminaBar.value = sprintRemaining; // 현재 스태미나 동기화
    }

    public void Btn()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }
}
