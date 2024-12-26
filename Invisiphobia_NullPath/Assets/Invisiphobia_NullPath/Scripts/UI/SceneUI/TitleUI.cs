using Common.Objects;
using Common.Path;
using Common.SceneEx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : BaseSceneUI
{
    public override void Init()
    {
        Managers.Sound.BGMPlay(ObjectManager.Return<AudioClip>(AddressablePath.BGMPath("Title")));
    }

    public void Btn()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.InGame);
    }
}
