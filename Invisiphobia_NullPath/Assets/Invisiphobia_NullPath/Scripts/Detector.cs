using Common.Yield;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] private List<IDetectable> detectedObjectList;

    private float updateInterval = 1f;

    private Coroutine currentCoroutine = null;


    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDetectable detectable))
        {
            StartTimer();
            detectedObjectList.Add(detectable);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDetectable detectable))
        {
            detectedObjectList.Remove(detectable);
            if (detectedObjectList.Count == 0)
            {
                StopTimer();
            }
        }
    }
    
    bool HasLineOfSight(IDetectable target)
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
            for (int i = 0; i < detectedObjectList.Count; i++)
            {
                if (HasLineOfSight(detectedObjectList[i]))
                {
                    UpdateDistances();
                    break;
                }
            }
            yield return YieldCache.WaitForSeconds(updateInterval);
        }
    }

    void UpdateDistances()
    {
        IDetectable closestDetectable = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < detectedObjectList.Count; i++)
        {
            if (detectedObjectList[i] == null)
            {
                throw new Exception();
            }
            else
            {
                float distance = Vector3.Distance(transform.position, detectedObjectList[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestDetectable = detectedObjectList[i];
                }
            }
        }

        if (closestDetectable != null)
        {
            HandleAlarm(minDistance, closestDetectable);
        }
    }

    private void HandleAlarm(float distance, IDetectable detectedobject)
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
    
    public void Reveal()
    {
        for (int i = 0; i < detectedObjectList.Count; i++)
        {
            detectedObjectList[i].Revealed();
        }
    }
}
