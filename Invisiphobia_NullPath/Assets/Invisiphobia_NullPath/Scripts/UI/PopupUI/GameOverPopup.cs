using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.Dark;
using Common.Yield;
using Common.SceneEx;

public class GameOverPopup : BasePopupUI
{
    [SerializeField] private ModalWindowManager myModalWindow;

    private void OnEnable()
    {
        myModalWindow.ModalWindowIn();
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
