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
    private IInteractable curInteractable;

    private Camera mainCam;
    public KeyCode interactKey = KeyCode.E;
    Player player;

    public void Init(Player player)
    {
        this.player = player;
    }
    private void Start()
    {
        mainCam = Camera.main;
        screenCenterRay = Vector3.zero;
        Player.Instance.PlayerController.playerInteractActionEvent += OnInteraction;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            PlayerInteraction();
        }
    }

    private void PlayerInteraction()
    {
        Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit raycastHit;

        // 레이를 디버그 로그로 시각화
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green, checkRate);

        // 레이캐스트 시각적으로 표시 (초록색은 닿지 않았을 때, 빨간색은 닿았을 때)
        if (Physics.Raycast(ray, out raycastHit, maxDistance, layerMask))
        {
            Debug.Log("Hit detected: " + raycastHit.collider.name);
            if (raycastHit.collider.TryGetComponent(out IInteractable interactable))
            {
                curInteractable = interactable;
                // 충돌된 경우 빨간색으로 표시
                Debug.DrawRay(ray.origin, ray.direction * raycastHit.distance, Color.red, checkRate);
            }
            else
            {
                curInteractable = null;
                // 충돌한 객체가 IInteractable이 아닌 경우 초록색으로 표시
                Debug.DrawRay(ray.origin, ray.direction * raycastHit.distance, Color.green, checkRate);
            }
        }
        else
        {
            curInteractable = null;
            // 레이가 어떤 것도 닿지 않았을 때 초록색으로 표시
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green, checkRate);
        }
    }

    public void OnInteraction()
    {
        if (Input.GetKeyDown(interactKey))
        {
            //if (curInteractable != null)
            {
                curInteractable.Interact(player);
            }
        }
    }
}
