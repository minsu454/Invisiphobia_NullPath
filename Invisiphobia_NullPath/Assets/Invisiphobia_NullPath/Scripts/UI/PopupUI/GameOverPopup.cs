using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.Dark;

public class GameOverPopup : BasePopupUI
{
    [SerializeField] private ModalWindowManager myModalWindow;

    public override void Init<T>(T option)
    {
        base.Init(option);

        //myModalWindow.title = "New Title"; // Change title
        //myModalWindow.description = "Description"; // Change desc
        //myModalWindow.ModalWindowIn(); // Open window
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
