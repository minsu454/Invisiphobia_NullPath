using Common.Event;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public void Init(Player player)
    {
        EventManager.Subscribe(GameEventType.GameOver, Die);
    }

    public void Die(object args)
    {
        GameManager.Instance.GameOver();
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEventType.GameOver, Die);
    }
}