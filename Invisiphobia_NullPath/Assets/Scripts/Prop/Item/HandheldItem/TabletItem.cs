public class TabletItem : HandheldItem
{
    public string TabletType { get; private set; }

    public TabletItem(int id, string name, string description, string tabletType) : base(id, name, description)
    {
        TabletType = tabletType;
    }

    public override void Interact()
    {
    }
}
