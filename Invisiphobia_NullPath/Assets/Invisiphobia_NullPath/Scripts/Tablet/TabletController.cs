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

    [SerializeField] private Detector detector;
    private void Awake()
    {
        detector = GetComponent<Detector>();
    }

    private void Update() // TODO 입력 Player에게 할당할지??
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Tab 키로 태블릿 활성화/비활성화
        {
            Debug.Log("Tab키 입력");
            if (state == TabletStateType.Idle)
                SetTabletState(TabletStateType.Active);
            else if (state == TabletStateType.Active)
                SetTabletState(TabletStateType.Idle);
        }

        UpdateTabletPosition();
    }

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
            // 확대위치로
            detector.Reveal();
            transform.position = Vector3.Lerp(transform.position, handPosition.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, handPosition.rotation, Time.deltaTime * transitionSpeed);
        }
        else if (state == TabletStateType.Active)
        {
            // 손위치로
            transform.position = Vector3.Lerp(transform.position, viewPosition.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, viewPosition.rotation, Time.deltaTime * transitionSpeed);

        }
    }
}
