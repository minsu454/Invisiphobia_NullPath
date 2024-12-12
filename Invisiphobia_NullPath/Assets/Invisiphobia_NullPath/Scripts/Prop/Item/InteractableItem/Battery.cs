using Common.Yield;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : InHandItem
{
    [Header("Battery Settings")]
    [SerializeField] private float maxCharge = 1f;
    [SerializeField] private float currentCharge; // 현재 배터리 충전량

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab, ReplaceBattery);
    }

    void ReplaceBattery(Transform playerTr)
    {
        Player player = playerTr.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("이시끼 어디갔어.");
            return;
        }

        Tablet tablet = player.PlayerInventory.Tablet;
        if (tablet == null)
        {
            Debug.LogWarning("타블렛 어디갔어.");
            return; 
        }

        float tabletMaxCharge = tablet.GetMaxCharge();

        float tabletRatio = tablet.GetCurrentCharge() / tabletMaxCharge;
        float batteryRatio = currentCharge * maxCharge;

        tablet.SetCurrentCharge(batteryRatio * tabletMaxCharge); 
        currentCharge = tabletRatio * maxCharge;

        transform.position = Camera.main.transform.forward + playerTr.position;
        Debug.Log(currentCharge);
    }

    //void Start()
    //{
    //    currentCharge = maxCharge; // 초기 충전량 설정
    //    StartCoroutine(DischargeBattery());
    //}

    //private IEnumerator DischargeBattery()
    //{
    //    while (currentCharge > 0)
    //    {
    //        yield return YieldCache.WaitForSeconds(1f);
    //        currentCharge -= dischargeRate;
    //        currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);
    //    }
    //}
}
