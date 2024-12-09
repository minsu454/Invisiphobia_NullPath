using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletController : MonoBehaviour
{
    [SerializeField] private Transform handTr;
    [SerializeField] private Transform viewTr;
    [SerializeField] private Transform hiddenTr;

    [SerializeField] private float transitionSpeed = 5f;

    private TabletStateType state = TabletStateType.Basic;

    public void Init(Tablet tablet)
    {
        tablet.OnStateChangedEvent += HandleStateChanged;
    }

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
        if (state == TabletStateType.Basic)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, handTr.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, handTr.rotation, Time.deltaTime * transitionSpeed);
        }
        else if (state == TabletStateType.Activate)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, viewTr.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, viewTr.rotation, Time.deltaTime * transitionSpeed);
        }
        else if (state == TabletStateType.Hidden)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, hiddenTr.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, hiddenTr.rotation, Time.deltaTime * transitionSpeed);
        }
    }
}
