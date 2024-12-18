using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public void Init(Player player)
    {
        
    }

    public void Die()
    {
        GameManager.Instance.GameOver();
    }
}