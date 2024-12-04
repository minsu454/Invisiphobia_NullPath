using UnityEngine;
using System.Linq;
using DG.Tweening;
using System.Collections;

public class IconManager : MonoBehaviour
{
    public GameObject iconPrefab;
    public Canvas uiCanvas;

    [SerializeField] private Camera mapCam;

    private void Start()
    {
        IDetectable[] detectableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDetectable>().ToArray();

        foreach(var obj in detectableObjects)
        {
            CreateIconForObject(obj);
        }
    }

    private void CreateIconForObject(IDetectable detectable)
    {
        // 아이콘 생성
        GameObject icon = Instantiate(iconPrefab, uiCanvas.transform);

        // 아이콘 위치를 업데이트하는 코루틴 시작
        StartCoroutine(CoUpdateIconPosition(icon, detectable));
    }

    private IEnumerator CoUpdateIconPosition(GameObject icon, IDetectable detactable)
    {
        Transform targetTransform = (detactable as MonoBehaviour).transform;

        while(targetTransform != null)
        {
            icon.transform.position = Vector3.LerpUnclamped(transform.position, targetTransform.position, Time.deltaTime * 50f);
            icon.transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.LookRotation(transform.position - mapCam.transform.position), Time.deltaTime * 50f);

            yield return null;
        }

        Destroy(icon);
    }
}
