using UnityEngine;
using UnityEngine.UI;

public class TabletUIManager : MonoBehaviour
{
    [SerializeField] private Camera mapCam;

    [SerializeField] private float idleMapSize;
    [SerializeField] private float activeMapSize;

    public void Init(Tablet tablet)
    {
        tablet.OnStateChangedEvent += HandleStateChanged;
        HandleStateChanged(TabletStateType.Idle);
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
}