using Common.Yield;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : InHandItem
{
    [Header("Battery Settings")]
    [SerializeField] private float maxCharge = 1f;
    [SerializeField] private float currentCharge; // 현재 배터리 충전량

    private Player player;

    public override void Interact(Player player)
    {
        this.player = player;
        player.PlayerInventory.SetHand(this, prefab, ReplaceBattery);
    }

    void ReplaceBattery(Transform playerTr)
    {
        Tablet tablet = player.PlayerInventory.Tablet;

        float tabletMaxCharge = tablet.GetMaxCharge();

        float tabletRatio = tablet.GetCurrentCharge() / tabletMaxCharge;
        float batteryRatio = currentCharge * maxCharge;

        tablet.SetCurrentCharge(batteryRatio * tabletMaxCharge); 
        currentCharge = tabletRatio * maxCharge;

        transform.position = Camera.main.transform.forward * 0.1f + playerTr.position;
    }
}
