using Common.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopup : BasePopupUI
{
    public override void Init<T>(T option)
    {
        base.Init(option);

        EventManager.Dispatch(GameEventType.UsePause, true);
    }

    public override void Close()
    {
        EventManager.Dispatch(GameEventType.UsePause, false);
        base.Close();
    }
}
