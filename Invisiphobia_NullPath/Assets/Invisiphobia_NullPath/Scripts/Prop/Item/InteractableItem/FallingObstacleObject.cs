using Common.Data;
using UnityEngine;

public class FallingObstacleObject : BaseItem
{
    public Transform forcePosition; // 힘을 가할 위치
    public float forceAmount = 500f; // 힘의 크기
    private Rigidbody rb;
    private bool isFallen = false;
    [SerializeField] private bool isDestroyed = false;

    public override void Init()
    {
        base.Init();
        interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);

        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public override void Interact(Player player)
    {
        if (isFallen)
            return;

        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;

        Vector3 forceDirection = forcePosition.right;
        rb.AddForceAtPosition(forceDirection * forceAmount, forcePosition.position, ForceMode.Impulse);

        isFallen = true;
        rb.isKinematic = true;

        if (isDestroyed)
            DestroyObstacle();
    }

    private void DestroyObstacle()
    {
        Destroy(gameObject);
    }
}
