using System.Collections;
using UnityEngine;

public class TabletUIController : MonoBehaviour
{
    [SerializeField] private Camera mapCam;
    private bool isActive = false;

    [SerializeField] private float idleMapSize;
    [SerializeField] private float activeMapSize;

    //[SerializeField] private 

    public void HandleStateChanged(TabletStateType newState)
    {
        if(newState == TabletStateType.Idle)
        {
            mapCam.orthographicSize = idleMapSize;
        }
        else if (newState == TabletStateType.Active)
        {
            mapCam.orthographicSize = activeMapSize;
        }
    }

    //public IEnumerable CoSwitchScreen()
    //{

    //}
}