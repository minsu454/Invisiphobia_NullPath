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
    private Player player;

    public void Init(Player player)
    {
        mainCam = Camera.main;
        screenCenterRay = Vector3.zero;

        this.player = player;

        player.PlayerController.playerInteractActionEvent += OnInteraction;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            PlayerInteraction();
        }
    }

    public void PlayerInteraction()
    {
        Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit raycastHit;

        // 레이캐스트 시각적으로 표시 (초록색은 닿지 않았을 때, 빨간색은 닿았을 때)
        if (Physics.Raycast(ray, out raycastHit, maxDistance, layerMask))
        {
            if (raycastHit.collider.TryGetComponent(out IInteractable interactable))
            {
                curInteractable = interactable;
                // 충돌된 경우 빨간색으로 표시
            }
            else
            {
                curInteractable = null;
                // 충돌한 객체가 IInteractable이 아닌 경우 초록색으로 표시
            }
        }
        else
        {
            curInteractable = null;
        }
    }

    public void OnInteraction()
    {
        if (curInteractable != null)
        {
            curInteractable.Interact(player);
        }
    }

    public Vector3 GetRayDirection()
    {
        // 플레이어가 현재 화면 중간을 기준으로 쏘는 레이의 방향을 반환
        Camera mainCam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCam.ScreenPointToRay(screenCenter);
        return ray.direction;
    }
}
