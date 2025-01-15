using Common.Event;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private bool isDie = false;

    public void Init(Player player)
    {
    }

    public void Die()
    {
        if (isDie)
            return;

        isDie = true;
        InGameLoader.Instance.Game.GameOver();
    }

    public void Clear()
    {
        InGameLoader.Instance.Game.GameClear();
    }
}