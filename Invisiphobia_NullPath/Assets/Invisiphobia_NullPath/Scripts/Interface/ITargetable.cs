using UnityEngine;

public interface ITargetable
{
    public Transform transform { get; }
    public void Die();
}