
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool isOpen = false; // 문이 열린 상태인지 확인하는 변수
    float elapsedTime = 0f;
    Quaternion startRotation;
    Quaternion endRotation;

    public void Awake()
    {
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y - 90, startRotation.eulerAngles.z);
    }
    public void Interact(Player player)
    {
        if ((elapsedTime != 0))
        {
            return;
        }

        if (isOpen)
        {
            StartCoroutine(DoorInteract(endRotation, startRotation, 1f)); // 닫기 동작
        }
        else
        {
            StartCoroutine(DoorInteract(startRotation, endRotation, 1f)); // 열기 동작
        }
    }

    private IEnumerator DoorInteract(Quaternion a, Quaternion b, float timeToAnimate)
    {
        elapsedTime = 0f;

        while (elapsedTime < timeToAnimate)
        {
            transform.rotation = Quaternion.Slerp(a, b, (elapsedTime / timeToAnimate));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0f;

        transform.rotation = b; // 정확한 목표 회전값으로 설정

        // 문 상태 업데이트
        isOpen = startRotation != transform.rotation;
    }
}

