using System;

/// <summary>
/// Active Stat을 사용하는 Enum 인터페이스(WorldUI 전용)
/// </summary>
public interface IActiveStatable<T> where T : Enum
{
    /// <summary>
    /// 기본상태 이벤트
    /// </summary>
    public event Action BasicStateEvent;

    /// <summary>
    /// 활성화상태 이벤트
    /// </summary>
    public event Action ActiveStateEvent;

    /// <summary>
    /// 사용이벤트
    /// </summary>
    public event Action<bool> UsePauseEvent;

    /// <summary>
    /// 사용이벤트
    /// </summary>
    public event Action<T> ShotEvent;
}