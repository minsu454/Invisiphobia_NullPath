using Common.Yield;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detector : MonoBehaviour
{
    [SerializeField] private List<IDetectable> detectedObjectList = new List<IDetectable>();

    private float updateInterval = 1f;

    private Coroutine currentCoroutine = null;

    private float closestdistance;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDetectable detectable))
        {
            if (detectable.StateType != PropStateType.None)
                return;

            StartTimer();
            detectable.Detected();
            detectedObjectList.Add(detectable);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDetectable detectable))
        {
            if (detectable.StateType == PropStateType.Revealed)
                return;

            detectable.Invisible();
            detectedObjectList.Remove(detectable);
            if (detectedObjectList.Count == 0)
            {
                StopTimer();
            }
        }
    }

    bool HasLineOfSight(IDetectable target) //IDetectable과 Detecter사이에 Wall이 있는지 판단
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.transform.position);

        int layerMask = LayerMask.GetMask("Wall");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, distance, layerMask))
        {
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
            closestdistance = float.MaxValue;
            for (int i = detectedObjectList.Count - 1; i >= 0; i--)
            {
                if (detectedObjectList[i] == null)
                {
                    detectedObjectList.RemoveAt(i);
                    continue;
                }

                if (HasLineOfSight(detectedObjectList[i]))
                {
                    UpdateDistances(detectedObjectList[i], ref closestdistance);
                    break;
                }
            }
            HandleAlarm(closestdistance);
            yield return YieldCache.WaitForSeconds(updateInterval);
        }
    }

    void UpdateDistances(IDetectable detectable, ref float closest)
    {
        float distance = Vector3.Distance(transform.position, detectable.transform.position);
        if (distance < closest)
        {
            closest = distance;
        }
    }

    private void HandleAlarm(float distance) //TODO : 조명과 오디오로 알람
    {
        if (distance < 5f)
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
        for (int i = detectedObjectList.Count - 1; i >= 0; i--)
        {
            if (detectedObjectList[i] == null)
            {
                detectedObjectList.RemoveAt(i);
                continue;
            }

            if (HasLineOfSight(detectedObjectList[i]))
            {
                detectedObjectList[i].Revealed();
            }
        }
    }
}