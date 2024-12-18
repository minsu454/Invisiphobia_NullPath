using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.Dark;
using Common.Yield;

public class GameOverPopup : BasePopupUI
{
    [SerializeField] private ModalWindowManager myModalWindow;

    private void OnEnable()
    {
        myModalWindow.ModalWindowIn();
    }

    private IEnumerator CoStart()
    {
        yield return YieldCache.WaitForSecondsRealtime(0.5f);
        myModalWindow.ModalWindowIn();
    }

    //private void Update()
    //{
    //    myModalWindow.UpdateUI();
    //}

    public override void Close()
    {
        myModalWindow.ModalWindowOut();
        base.Close();
    }
}
