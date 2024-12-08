using System.Collections;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class TabletUIController : MonoBehaviour
{
    [SerializeField] private Camera mapCam;
    private bool isActive = false;

    [SerializeField] private float idleMapSize;
    [SerializeField] private float activeMapSize;

    public void Init(Tablet tablet)
    {
        tablet.OnStateChangedEvent += HandleStateChanged;
    }

    public void HandleStateChanged(TabletStateType newState)
    {
        switch (newState)
        {
            case TabletStateType.Basic:
                //mapCam.orthographicSize = idleMapSize;
                break;
            case TabletStateType.Activate:
                //mapCam.orthographicSize = activeMapSize;
                break;
        }
    }
}