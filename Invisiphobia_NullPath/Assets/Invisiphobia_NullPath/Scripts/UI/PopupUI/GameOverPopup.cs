using UnityEngine;
using Michsky.UI.Dark;
using Common.SceneEx;
using Common.Event;

public class GameOverPopup : BasePopupUI
{
    [SerializeField] private ModalWindowManager myModalWindow;

    private void OnEnable()
    {
        myModalWindow.ModalWindowIn();
    }

    public override void Init<T>(T option)
    {
        base.Init(option);
        EventManager.Dispatch(GameEventType.UseInput, true);
        EventManager.Dispatch(GameEventType.UseEsc, false);
    }

    public void GoTitle()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }

    public void GoContinue()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.InGame);
    }
}
