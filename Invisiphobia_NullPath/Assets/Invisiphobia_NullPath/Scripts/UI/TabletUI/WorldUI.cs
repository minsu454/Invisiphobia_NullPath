public abstract class WorldUI : BaseUI
{
    /// <summary>
    /// 초기화 함수
    /// </summary>
    public abstract void Init(IActiveStatable subject);

    /// <summary>
    /// 이벤트 구독 함수
    /// </summary>
    public abstract void Subscribe(IActiveStatable subject);

    /// <summary>
    /// 이벤트 구독취소 함수
    /// </summary>
    public abstract void Unsubscribe(IActiveStatable subject);
}