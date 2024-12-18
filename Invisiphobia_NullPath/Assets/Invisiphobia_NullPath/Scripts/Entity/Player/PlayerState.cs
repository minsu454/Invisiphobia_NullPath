using Common.Event;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private bool isDie = false;

    public void Init(Player player)
    {
        EventManager.Subscribe(GameEventType.GameOver, Die);
    }

    public void Die(object args)
    {
        if (isDie)
            return;

        isDie = true;
        GameManager.Instance.GameOver();
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEventType.GameOver, Die);
    }
}