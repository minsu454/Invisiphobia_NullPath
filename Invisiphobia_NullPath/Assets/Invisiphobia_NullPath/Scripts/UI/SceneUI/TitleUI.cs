using Common.SceneEx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : BaseSceneUI
{
    public override void Init()
    {

    }

    public void Btn()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.InGame);
    }
}
