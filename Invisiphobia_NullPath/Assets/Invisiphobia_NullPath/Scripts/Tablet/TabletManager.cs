using UnityEngine;

public class TabletManager : MonoBehaviour
{
    public static TabletManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [SerializeField] private Camera mapCam;
    private bool isActive = false;

    [SerializeField] private float idleMapSize;
    [SerializeField] private float activeMapSize;

    public void ApplyMapSize()
    {
        if (!isActive)
        {
            isActive = true;
            mapCam.orthographicSize = activeMapSize;
        }
        else
        {
            isActive = false;
            mapCam.orthographicSize = idleMapSize;
        }
    }
}