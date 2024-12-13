using UnityEngine;
using System.Collections;
using Common.Yield;
using Common.Data;
using System.Collections.Generic;
using System.Diagnostics;

public class Table : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject drawer1; // 서랍 1 오브젝트
    [SerializeField] private GameObject drawer2; // 서랍 2 오브젝트
    [SerializeField] private float openPositionZ = 0.5f; // 서랍이 열릴 때의 위치 (Z축)
    [SerializeField] private float moveSpeed = 2f; // 서랍 이동 속도

    [Header("Table")]
    [SerializeField] private int itemId;
    protected ItemTable itemTable;
    public ItemTable ItemTable
    {
        get { return itemTable; }
    }

    protected string interactText = "[E]";
    public string InteractText { get { return interactText; } }

    protected string actionText;
    public string ActionText { get { return actionText; } }

    public bool IsReveal => true;

    private bool isDrawer1Open = false;
    private bool isDrawer2Open = false;
    private bool isCoroutineRunning = false;
    private bool isHit = false;

    private string[] tableArr = new string[2];
    private void Awake()
    {
        itemTable = DataService.GetItemTableByKey(itemId);
        //actionText = DataServise.GetInteractText(ItemTable.actionText);
        for(int i = 0; i < itemTable.interactText.Count; i++)
        {
            tableArr[i] = DataService.GetInteractText(ItemTable.interactText[0]);
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 테이블이 레이의 충돌 지점이라면 해당 방향을 계산
            Vector3 hitPoint = hit.point;
            Vector3 tableCenter = transform.position;

            // 충돌 지점이 테이블의 중심보다 왼쪽인지 오른쪽인지 판단
            if (hitPoint.x < tableCenter.x)
            {
                interactText = tableArr[0];
            }
            else
            {
                interactText = tableArr[1];
            }
        }
    }

    public void Interact(Player player)
    {
        if (isCoroutineRunning)
        {
            return;
        }

        // 플레이어가 쏘는 레이의 충돌 지점을 기준으로 판단
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 테이블이 레이의 충돌 지점이라면 해당 방향을 계산
            Vector3 hitPoint = hit.point;
            Vector3 tableCenter = transform.position;

            // 충돌 지점이 테이블의 중심보다 왼쪽인지 오른쪽인지 판단
            if (hitPoint.x < tableCenter.x)
            {
                // 충돌 지점이 테이블의 왼쪽이면 drawer1을 다룬다
                if (!isDrawer1Open)
                {
                    StartCoroutine(CoMoveDrawer(drawer1.transform.localPosition, new Vector3(0, 0, openPositionZ), drawer1));
                    isDrawer1Open = true;
                    tableArr[0] = DataService.GetInteractText(ItemTable.interactText[1]);
                }
                else
                {
                    StartCoroutine(CoMoveDrawer(drawer1.transform.localPosition, Vector3.zero, drawer1));
                    isDrawer1Open = false;
                    tableArr[0] = DataService.GetInteractText(ItemTable.interactText[0]);
                }
            }
            else
            {
                // 충돌 지점이 테이블의 오른쪽이면 drawer2를 다룬다
                if (!isDrawer2Open)
                {
                    StartCoroutine(CoMoveDrawer(drawer2.transform.localPosition, new Vector3(0, 0, openPositionZ), drawer2));
                    isDrawer2Open = true;
                    tableArr[1] = DataService.GetInteractText(ItemTable.interactText[1]);
                }
                else
                {
                    StartCoroutine(CoMoveDrawer(drawer2.transform.localPosition, Vector3.zero, drawer2));
                    isDrawer2Open = false;
                    tableArr[1] = DataService.GetInteractText(ItemTable.interactText[0]);
                }
            }
        }
    }

    private IEnumerator CoMoveDrawer(Vector3 fromPosition, Vector3 toPosition, GameObject drawer)
    {
        isCoroutineRunning = true;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            drawer.transform.localPosition = Vector3.Lerp(fromPosition, toPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }
        drawer.transform.localPosition = toPosition; // 최종 위치 보장
        yield return YieldCache.WaitForSeconds(0.2f);
        isCoroutineRunning = false;
    }
}
