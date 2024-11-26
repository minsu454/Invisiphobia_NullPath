using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField]private List<Collider> DetectedObjectList;

    private float updateInterval = 1f;

    private Coroutine currentCoroutine = null;

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleObject"))
        {
            StartTimer();
            DetectedObjectList.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleObject"))
        {
            DetectedObjectList.Remove(other);
            if (DetectedObjectList.Count == 0)
            {
                StopTimer();
            }
        }
    }

    bool HasLineOfSight(Collider target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.transform.position);

        int layerMask = LayerMask.GetMask("Wall");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, distance, layerMask))
        {
            // Raycast가 벽을 맞췄을 때
            return false;
        }
        else
        {
            return true;
        }
    }

    private void StartTimer()
    {
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(CoCheckTimer());
        }
    }
    
    private void StopTimer()
    {
        StopCoroutine(currentCoroutine);
        currentCoroutine = null;
    }

    private IEnumerator CoCheckTimer()
    {
        while (true)
        {
            for (int i = 0; i < DetectedObjectList.Count; i++)
            {
                if (HasLineOfSight(DetectedObjectList[i]))
                {
                    UpdateDistances();
                    break;
                }
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }

    void UpdateDistances()
    {
        GameObject closestObject = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < DetectedObjectList.Count; i++)
        {
            if (DetectedObjectList[i] == null)
            {
                throw new Exception();
            }
            else
            {
                float distance = Vector3.Distance(transform.position, DetectedObjectList[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestObject = DetectedObjectList[i].gameObject;
                }
            }
        }

        if (closestObject != null)
        {
            HandleAlarm(minDistance, closestObject);
        }
    }

    private void HandleAlarm(float distance, GameObject detectedobject)
    {
        if(distance < 5f)
        {
            Debug.Log("물체가 가깝습니다!!");
        }
        else if (distance < 10f)
        {
            Debug.Log("물체가 감지되었습니다!");
        }
    }
}
