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
        Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height /2));
        RaycastHit raycastHit;

        //raycastHit = 레이쏴서 충돌된 물체
        if (Physics.Raycast(ray, out raycastHit, maxDistance, layerMask))
        {
            if (raycastHit.collider.TryGetComponent(out IInteractable interactable))
            {
                curInteractable = interactable;
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
}
