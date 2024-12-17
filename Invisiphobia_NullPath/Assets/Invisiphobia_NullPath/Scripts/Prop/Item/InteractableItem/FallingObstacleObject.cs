using Common.Data;
using UnityEngine;

public class FallingObstacleObject : BaseItem
{
    private bool isFallen = false;
    [SerializeField] private bool isDestroyed = false;

    public override void Init()
    {
        base.Init();
        interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);
    }

    public override void Interact(Player player)
    {
        if (isFallen)
            return;

        isFallen = true;
        if(isDestroyed)
            DestroyObstacle();
    }

    private void DestroyObstacle()
    {
        Destroy(gameObject);
    }
}
