using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Ray Check Option")]
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxDistance;
    public LayerMask layerMask; //어떤 레이어가 달려있는 게임오브젝트를 추출할지
    private Vector3 screenCenterRay;//레이 중앙에서 쏘기 위한 변수

    [Header("InteractGameObject")]
    public GameObject curInteractGameObject;
    private IInteractable curInteractable;
    public TextMeshProUGUI promptText;

    private new Camera camera;

    private void Start()
    {
        camera = Camera.main;
        screenCenterRay = Vector3.zero;
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            PlayerInteraction();
        }
    }

    private void PlayerInteraction()
    {
        //new를 사용하지 않는 방식으로 Garbage 최소화
        screenCenterRay.Set(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterRay);
        RaycastHit raycastHit;

        //raycastHit = 레이쏴서 충돌된 물체
        if(Physics.Raycast(ray, out raycastHit, maxDistance, layerMask))
        {
            if(raycastHit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = raycastHit.collider.gameObject;
                curInteractable = raycastHit.collider.GetComponent<IInteractable>();
                SetPromptText();
            }
        }
        else
        {
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        //promptText.text = curInteractable.(여기에 상호작용 시 뜰 프롬포트 추가)
    }

    public void OnInteraction()
    {
        if(Input.GetKeyDown(KeyCode.E) && curInteractable != null)
        {
            //curInteractable.Interact();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
