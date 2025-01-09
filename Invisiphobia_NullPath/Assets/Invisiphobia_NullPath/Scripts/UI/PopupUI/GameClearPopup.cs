using Common.Event;
using Common.SceneEx;
using Michsky.UI.Dark;
using UnityEngine;

public class GameClearPopup : BasePopupUI
{
    [SerializeField] private ModalWindowManager myModalWindow;

    private void OnEnable()
    {
        myModalWindow.ModalWindowIn();
    }

    public override void Init<T>(T option)
    {
        base.Init(option);
        EventManager.Dispatch(GameEventType.UseLockMouse, false);
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
