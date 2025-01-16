using UnityEngine;
using Michsky.UI.Dark;
using Common.SceneEx;
using Common.Event;
using Common.Save;

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

        EventManager.Dispatch(GameEventType.UseLockMouse, false);
    }

    public void GoTitle()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }

    public void GoContinue()
    {
        if (SaveService.Exists)
            SaveService.SetCurPath(true);

        SceneManagerEx.LoadingAndNextScene(SceneType.InGame);
    }
}
