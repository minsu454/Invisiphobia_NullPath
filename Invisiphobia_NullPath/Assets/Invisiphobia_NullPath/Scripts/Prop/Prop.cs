using UnityEngine;
using UnityEngine.SceneManagement;

public class Prop : MonoBehaviour, IDetectable, IParts
{
    [Header("Prop")]
    [SerializeField] private MeshRenderer[] myRendererArr;                      //해당 오브젝트 랜더러들 모음 배열

    MapIcon IDetectable.MapIcon => mapIcon;
    private MapIcon mapIcon;                                                    //맵아이콘

    public bool IsDetectTablet { get; set; }

    public PropStateType StateType { get; protected set; } = PropStateType.None;

    #region Test
    [Header("MapIcon")]
    [SerializeField] private GameObject mapIconPrefab;                          //맵 아이콘 프리팹(임시)

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "InGame_Scene")
            Init();
    }
    #endregion

    /// <summary>
    /// Prop 초기화 함수
    /// </summary>
    public virtual void Init()
    {
        foreach (MeshRenderer renderer in myRendererArr)
        {
            renderer.enabled = false;
        }
        
        GameObject go = Instantiate(mapIconPrefab);
        mapIcon = go.GetComponent<MapIcon>();
        mapIcon.Init(transform);
    }

    public virtual void Detected()
    {
        StateType = PropStateType.Detected;
        mapIcon.Detected();
    }

    public virtual void Detecting()
    {
        StateType = PropStateType.Detecting;
        mapIcon.Detecting();
    }

    public void DetectCompleted()
    {
        StateType = PropStateType.DetectCompleted;
    }

    public virtual void Revealed()
    {
        if (StateType != PropStateType.DetectCompleted)
        {
            Detected();
            return;
        }
        
        StateType = PropStateType.Revealed;

        foreach (MeshRenderer renderer in myRendererArr)
        {
            renderer.enabled = true;
        }

        mapIcon.Revealed();
    }

    public virtual void Invisible()
    {
        StateType = PropStateType.None;

        foreach (MeshRenderer renderer in myRendererArr)
        {
            renderer.enabled = false;
        }

        mapIcon.Invisible();
    }

    public void SetFillAmount(float value)
    {
        mapIcon.SetFillAmount(value);
    }

    public void IconActive(bool active)
    {
        if(mapIcon != null)
            mapIcon.gameObject.SetActive(active);
    }

    public void SetMapIconToWall(bool active)
    {
        if (active)
        {
            mapIcon.Detected();
        }
        else
        {
            mapIcon.Invisible();
        }
    }

    private void OnDisable()
    {
        IconActive(false);
    }
}
