using Common.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class WallAndDoor : MonoBehaviour
{
    float elapsedTime = 0f;

    public GameObject door;

    Quaternion startRotation;
    Quaternion endRotation;

    public void Start()
    {
        startRotation = door.transform.rotation;
        endRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y - 90, startRotation.eulerAngles.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ThrowItem item))
        {
            StartCoroutine(CoOpenDoor(startRotation, endRotation, 1f));
        }
    }

    private IEnumerator CoOpenDoor(Quaternion a, Quaternion b, float timeToAnimate)
    {
        elapsedTime = 0f;

        while (elapsedTime < timeToAnimate)
        {
            door.transform.rotation = Quaternion.Slerp(a, b, (elapsedTime / timeToAnimate));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0f;

        door.transform.rotation = b;
    }
}
