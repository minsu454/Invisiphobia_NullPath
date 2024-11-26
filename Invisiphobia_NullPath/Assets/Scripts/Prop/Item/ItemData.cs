using static BaseItem;

public class ItemData
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public ItemCarryType type { get; private set; }

    public ItemData(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}