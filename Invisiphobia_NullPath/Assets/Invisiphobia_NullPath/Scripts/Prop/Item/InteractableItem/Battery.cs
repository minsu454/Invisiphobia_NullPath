using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MapItem
{
    [Header("Battery Settings")]
    public float dischargeRate = 1f; // 초당 감소량
    private float maxCharge = 100f; // 배터리의 최대 충전량

    [SerializeField] private float currentCharge; // 현재 배터리 충전량

    public override void Interact(Player player)
    {
    }

    void Start()
    {
        currentCharge = maxCharge; // 초기 충전량 설정
        StartCoroutine(DischargeBattery());
    }

    private IEnumerator DischargeBattery()
    {
        while (currentCharge > 0)
        {
            yield return new WaitForSeconds(1f);
            currentCharge -= dischargeRate;
            currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);
        }
    }
}
