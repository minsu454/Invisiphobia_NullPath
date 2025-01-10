using Common.Data;
using Common.Yield;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Managers.Sound.SFX3DPlay(buttonPress, gameObject.transform);
            StartCoroutine(CoOpenDoor(startRotation, endRotation, 1f));
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
