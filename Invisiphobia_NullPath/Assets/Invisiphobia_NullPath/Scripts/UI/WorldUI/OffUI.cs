public class OffUI : WorldUI<TabletStateType>
{
    public override void Init(IActiveStatable<TabletStateType> subject)
    {

    }

    public override void Subscribe(IActiveStatable<TabletStateType> subject)
    {

    }

    public override void Unsubscribe(IActiveStatable<TabletStateType> subject)
    {
        gameObject.SetActive(false);
    }
}