using Common.Save;
using Common.SceneEx;

public class TitleUI : BaseSceneUI
{
    public void NewGame()
    {
        SaveService.SetCurPath(false);
        SceneManagerEx.LoadingAndNextScene(SceneType.InGame);
    }

    public void LoadGame()
    {
        if (!SaveService.Exists)
            return;

        SaveService.SetCurPath(true);
        SceneManagerEx.LoadingAndNextScene(SceneType.InGame);
    }
}
