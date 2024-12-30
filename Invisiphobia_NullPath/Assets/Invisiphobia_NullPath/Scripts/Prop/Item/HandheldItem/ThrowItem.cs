using Common.Data;
using UnityEngine;

public class ThrowItem : InHandItem
{
    [Header("ThrowItem")]
    [SerializeField] private Rigidbody myRb;            //내 rigidbody
    [SerializeField] private float throwSpeed = 10f;    //날아가는 스피드

    public override void Init()
    {
        base.Init();
    }

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab, Throw);
    }

    public void Throw(Transform playerTr)
    {
        transform.position = Camera.main.transform.forward * 0.1f + playerTr.position;
        Vector3 throwDirection = ThrowDirection(Camera.main.transform.forward);
        Vector3 initialVelocity = throwDirection * throwSpeed;

        myRb.velocity = Vector3.zero;
        myRb.AddForce(initialVelocity, ForceMode.Impulse);
    }

    /// <summary>
    /// 던지는 방향 반환 함수
    /// </summary>
    private Vector3 ThrowDirection(Vector3 direction)
    {
        direction.y += 0.3f;    // 포물선
        return direction.normalized;
    }
}