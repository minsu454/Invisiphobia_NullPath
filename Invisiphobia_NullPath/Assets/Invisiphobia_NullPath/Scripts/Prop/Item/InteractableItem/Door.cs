using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool isOpen = false; // 문이 열린 상태인지 확인하는 변수

    public void Interact(Player player)
    {
        if (isOpen)
        {
            StartCoroutine(CloseDoor());
        }
        else
        {
            StartCoroutine(OpenDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        float timeToOpen = 1f; // 문이 열리는 데 걸리는 시간
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 90, 0); // 문이 90도 회전할 목표 회전값

        while (elapsedTime < timeToOpen)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, (elapsedTime / timeToOpen));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation; // 정확한 목표 회전값으로 설정
        isOpen = true; // 문이 열렸음을 표시
    }

    private IEnumerator CloseDoor()
    {
        float timeToClose = 1f; // 문이 닫히는 데 걸리는 시간
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, 0); // 문을 원래 상태로 되돌리는 목표 회전값

        while (elapsedTime < timeToClose)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, (elapsedTime / timeToClose));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation; // 정확한 목표 회전값으로 설정
        isOpen = false; // 문이 닫혔음을 표시
    }
}

