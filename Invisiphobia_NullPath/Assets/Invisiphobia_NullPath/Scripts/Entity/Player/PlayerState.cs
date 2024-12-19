using Common.Event;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private bool isDie = false;

    public void Init(Player player)
    {
        EventManager.Subscribe(GameEventType.GameOver, Die);
        EventManager.Subscribe(GameEventType.GameClear, Clear);
    }

    public void Die(object args)
    {
        if (isDie)
            return;

        isDie = true;
        GameManager.Instance.GameOver();
    }

    public void Clear(object args)
    {
        Debug.Log("깼음");
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEventType.GameOver, Die);
        EventManager.Unsubscribe(GameEventType.GameClear, Clear);
    }
}