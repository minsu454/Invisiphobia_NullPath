public abstract class WorldUI : BaseUI
{
    public abstract void Init(IActiveStatable subject);

    public abstract void Subscribe(IActiveStatable subject);
    public abstract void Unsubscribe(IActiveStatable subject);
}