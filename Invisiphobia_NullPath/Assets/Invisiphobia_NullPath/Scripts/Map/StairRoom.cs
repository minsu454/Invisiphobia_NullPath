using Common.Data;
using Common.Event;
using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class StairRoom : MonoBehaviour
{
    [SerializeField] private TriggerDetector enterdetector;
    [SerializeField] private TriggerDetector exitDetector;

    [SerializeField] private int lockoffItemId;

    [SerializeField] private Transform door;

    private float elapsedTime = 0f;
    private Quaternion startRotation;
    private Quaternion endRotation;
    private bool isOpen = false;

    private void Awake()
    {
        enterdetector.EnterEvent += ClearZoneEvent;
        exitDetector.EnterEvent += GameClearEvent;

        startRotation = door.rotation;
        endRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y - 90, startRotation.eulerAngles.z);
    }

    private void ClearZoneEvent(Collider col)
    {
        if (col.TryGetComponent(out PlayerInventory inventory))
        {
            if (!inventory.IsLockOffItemInHand(lockoffItemId) || isOpen)
                return;

            isOpen = true;
            StartCoroutine(DoorInteract(startRotation, endRotation, 1f));
        }
    }

    private void GameClearEvent(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            EventManager.Dispatch(GameEventType.GameClear, null);
        }
    }

    private IEnumerator DoorInteract(Quaternion a, Quaternion b, float timeToAnimate)
    {
        elapsedTime = 0f;

        while (elapsedTime < timeToAnimate)
        {
            door.rotation = Quaternion.Slerp(a, b, (elapsedTime / timeToAnimate));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0f;

        door.rotation = b;
    }
}