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
        tablet.OnStateChanged += HandleStateChanged;
    }

    public void HandleStateChanged(TabletStateType newState)
    {
        switch (newState)
        {
            case TabletStateType.Idle:
                mapCam.orthographicSize = idleMapSize;
                break;
            case TabletStateType.Active:
                mapCam.orthographicSize = activeMapSize;
                break;
        }
    }

    //public IEnumerable CoSwitchScreen()
    //{

    //}
}