using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] private float MoveDuration = 0.5f;
    [SerializeField] private TabletStateType state;

    [SerializeField] private Detector detector;

    private Tween tw1;
    private Tween tw2;


    private void Awake()
    {
        detector = GetComponent<Detector>();
    }
    private void Update() // TODO 입력 Player에게 할당할지??
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Tab 키로 태블릿 활성화/비활성화
        {
            //Debug.Log("Tab키 입력");
            EnableTablet();
        }
        MoveTabletPosition();
    }

    public void EnableTablet()
    {
        if (state == TabletStateType.Idle)
        {
            SetTabletState(TabletStateType.Active);
        }
        else if (state == TabletStateType.Active)
        {
            detector.Reveal();
            SetTabletState(TabletStateType.Idle);
        }
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

    private void MoveTabletPosition()
    {
        if (state == TabletStateType.Idle)
        {
            if (tw1 == null && tw2 == null)
            {
                tw1 = transform.DOMove(handPosition.position, MoveDuration).OnComplete(() => { tw1 = null; });
                tw2 = transform.DORotateQuaternion(handPosition.rotation, MoveDuration).OnComplete(() => { tw2 = null; });
            }
            // 확대위치로
            //transform.position = Vector3.Lerp(transform.position, handPosition.position, Time.deltaTime * transitionSpeed);
            //transform.rotation = Quaternion.Lerp(transform.rotation, handPosition.rotation, Time.deltaTime * transitionSpeed);
        }
        else if (state == TabletStateType.Active)
        {
            if (tw1 == null && tw2 == null)
            {
                tw1 = transform.DOMove(viewPosition.position, MoveDuration).OnComplete(() => { tw1 = null; });
                tw2 = transform.DORotateQuaternion(viewPosition.rotation, MoveDuration).OnComplete(() => { tw2 = null; });
            }
            // 손위치로
            //transform.position = Vector3.Lerp(transform.position, viewPosition.position, Time.deltaTime * transitionSpeed);
            //transform.rotation = Quaternion.Lerp(transform.rotation, viewPosition.rotation, Time.deltaTime * transitionSpeed);
        }
    }

    private void OnDisable()
    {
        tw1.Kill();
        tw2.Kill();
        tw1 = null;
        tw2 = null;
    }
}
