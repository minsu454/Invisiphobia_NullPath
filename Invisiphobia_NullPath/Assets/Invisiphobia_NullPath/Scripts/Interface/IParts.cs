using UnityEngine;

/// <summary>
/// 부품 인터페이스
/// </summary>
public interface IParts
{
    public GameObject gameObject { get; }
    public Transform transform { get; }
}
