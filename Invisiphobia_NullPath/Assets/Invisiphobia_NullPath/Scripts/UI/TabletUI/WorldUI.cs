using System;

public abstract class WorldUI<T> : BaseUI where T : Enum
{
    /// <summary>
    /// 초기화 함수
    /// </summary>
    public abstract void Init(IActiveStatable<T> subject);

    /// <summary>
    /// 이벤트 구독 함수
    /// </summary>
    public abstract void Subscribe(IActiveStatable<T> subject);

    /// <summary>
    /// 이벤트 구독취소 함수
    /// </summary>
    public abstract void Unsubscribe(IActiveStatable<T> subject);
}