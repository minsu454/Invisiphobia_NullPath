using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopup : BasePopupUI
{
    [SerializeField] private Michsky.UI.Dark.ModalWindowManager modalWindowManager;

    public override void Init<T>(T option)
    {
        base.Init(option);
        Time.timeScale = 0f;

        modalWindowManager.ModalWindowIn();
    }

    public override void Close()
    {
        modalWindowManager.ModalWindowOut();
        base.Close();
    }
}
