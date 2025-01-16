using Common.Data;
using Common.Yield;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ButtonAndDoor : MonoBehaviour
{
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip buttonPress;
    [SerializeField] private float doorOpenDelay = 1.0f;
    float elapsedTime = 0f;

    public Transform door;

    Quaternion startRotation;
    Quaternion endRotation;

    private EventParts parts;

    public void Start()
    {
        parts = GetComponent<EventParts>();

        startRotation = door.transform.rotation;
        endRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y - 90, startRotation.eulerAngles.z);

        if (parts.IsCompleted)
        {
            door.rotation = endRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            if (parts.IsCompleted)
            {
                return;
            }

            Managers.Sound.SFX3DPlay(buttonPress, gameObject.transform);
            StartCoroutine(CoOpenDoor(startRotation, endRotation, 1f));
            parts.IsCompleted = true;
        }
    }

    private IEnumerator CoOpenDoor(Quaternion a, Quaternion b, float timeToAnimate)
    {
        elapsedTime = 0f;
        yield return YieldCache.WaitForSeconds(doorOpenDelay);

        while (elapsedTime < timeToAnimate)
        {
            door.transform.rotation = Quaternion.Slerp(a, b, (elapsedTime / timeToAnimate));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0f;

        door.transform.rotation = b;
        Managers.Sound.SFX2DPlay(doorOpen);
    }
}
