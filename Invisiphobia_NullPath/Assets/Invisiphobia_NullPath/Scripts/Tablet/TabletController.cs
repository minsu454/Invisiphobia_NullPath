using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TabletStateType
{
    Hidden,
    Idle,
    Active
}

public class TabletController : MonoBehaviour
{
    [SerializeField] private Transform handPosition;
    [SerializeField] private Transform viewPosition;
    [SerializeField] private float transitionSpeed = 5f;

    [SerializeField] private TabletStateType state;

    private void SetTabletState(TabletStateType newState)
    {
        state = newState;

        if (newState == TabletStateType.Hidden)
        {
            gameObject.SetActive(false); // 태블릿 숨기기
        }
        else
        {
            gameObject.SetActive(true); // 태블릿 보이기
        }
    }

    private void UpdateTabletPosition()
    {
        if (state == TabletStateType.Idle)
        {
            // 손 위치로 이동
            transform.position = Vector3.Lerp(transform.position, handPosition.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, handPosition.rotation, Time.deltaTime * transitionSpeed);
        }
        else if (state == TabletStateType.Active)
        {
            // 확대 위치로 이동
            transform.position = Vector3.Lerp(transform.position, viewPosition.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, viewPosition.rotation, Time.deltaTime * transitionSpeed);
        }
    }
}
