using Common.Event;
using Common.SceneEx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopup : BasePopupUI
{
    public override void Init<T>(T option)
    {
        base.Init(option);

        EventManager.Dispatch(GameEventType.UseInput, false);
    }

    public void GoTitle()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }

    public override void Close()
    {
        EventManager.Dispatch(GameEventType.UseInput, true);

        base.Close();
    }
}
