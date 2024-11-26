using System;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField]private List<GameObject> DetectedObjects;

    private float updateInterval = 0.5f;
    private float timeSinceLastUpdate = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleObject"))
        {
            if (HasLineOfSight(other))
            {
                DetectedObjects.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("InvisibleObject"))
        {
            DetectedObjects.Remove(other.gameObject);
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

    private void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= updateInterval)
        {
            timeSinceLastUpdate = 0f;
            UpdateDistances();
        }
    }

    void UpdateDistances()
    {
        GameObject closestObject = null;
        float minDistance = float.MaxValue;

        for (int i = DetectedObjects.Count - 1; i >= 0; i--)
        {
            if (DetectedObjects[i] == null)
            {
                throw new Exception();
            }
            else
            {
                float distance = Vector3.Distance(transform.position, DetectedObjects[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestObject = DetectedObjects[i];
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