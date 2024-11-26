public class RescueTarget : HandheldItem
{
    public bool IsRescueTarget { get; private set; }

    public RescueTarget(int id, string name, string description, bool isRescueTarget) : base(id, name, description)
    {
        IsRescueTarget = isRescueTarget;
    }

    public override void Interact()
    {
    }
}
