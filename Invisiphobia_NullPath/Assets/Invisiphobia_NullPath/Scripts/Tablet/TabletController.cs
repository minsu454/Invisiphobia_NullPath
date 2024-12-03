using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletController : MonoBehaviour
{
    [SerializeField] private Transform handPosition;
    [SerializeField] private Transform viewPosition;
    [SerializeField] private Transform hiddenPosition;

    [SerializeField] private float transitionSpeed = 5f;

    private TabletStateType state;

    private void Update()
    {
        MoveTabletPosition();
    }

    public void HandleStateChanged(TabletStateType newState)
    {
        state = newState;
    }

    private void MoveTabletPosition()
    {
        if (state == TabletStateType.Idle)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, handPosition.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, handPosition.rotation, Time.deltaTime * transitionSpeed);
        }
        else if (state == TabletStateType.Active)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, viewPosition.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, viewPosition.rotation, Time.deltaTime * transitionSpeed);
        }
    }
}
