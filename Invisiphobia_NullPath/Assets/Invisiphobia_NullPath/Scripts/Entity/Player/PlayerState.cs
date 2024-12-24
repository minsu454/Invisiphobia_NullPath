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
        GameManager.Instance.GameOver();
    }

    public void Clear()
    {
        Debug.Log("깼음");
    }
}