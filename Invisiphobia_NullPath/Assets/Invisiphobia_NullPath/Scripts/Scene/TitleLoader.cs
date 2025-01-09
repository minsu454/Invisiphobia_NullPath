using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLoader : BaseSceneLoader<TitleLoader>
{
    protected override void InitScene()
    {

    }

    private void Start()
    {
        Managers.Sound.FirstSceneBGMPlay(SceneType.Title, 0.5f);
    }
}
