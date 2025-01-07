using UnityEngine;
using System.Collections;
using Common.Yield;
using Common.Data;

public class Table : BaseItem
{
    [SerializeField] private GameObject drawer1; // 서랍 1 오브젝트
    [SerializeField] private GameObject drawer2; // 서랍 2 오브젝트
    [SerializeField] private float openPositionZ = 0.5f; // 서랍이 열릴 때의 위치 (Z축)
    [SerializeField] private float moveSpeed = 2f; // 서랍 이동 속도
    [SerializeField] private AudioClip drawerOpen;
    [SerializeField] private AudioClip drawerClose;

    private bool isDrawer1Open = false;
    private bool isDrawer2Open = false;
    private bool isCoroutineRunning = false;
    private bool isHit = false;

    private string[] tableArr = new string[2];
    public override void Init()
    {
        base.Init();
        for(int i = 0; i < itemTable.interactText.Count; i++)
        {
            tableArr[i] = DataService.GetItemInteractText(ItemTable.interactText[0]);
        }
        //actionText = DataServise.GetInteractText(ItemTable.actionText);
    }

    public override void Interact(Player player)
    {
        if (isCoroutineRunning)
        {
            return;
        }

        // 플레이어가 쏘는 레이의 충돌 지점을 기준으로 판단
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == drawer1)
            {
                HandleDrawerInteraction(drawer1, ref isDrawer1Open, 0);
            }
            else if (hit.collider.gameObject == drawer2)
            {
                HandleDrawerInteraction(drawer2, ref isDrawer2Open, 1);
            }
        }
    }

    private void HandleDrawerInteraction(GameObject drawer, ref bool isDrawerOpen, int index)
    {
        if (!isDrawerOpen)
        {
            StartCoroutine(CoMoveDrawer(drawer.transform.localPosition, new Vector3(0, 0, openPositionZ), drawer));
            isDrawerOpen = true;
            tableArr[index] = DataService.GetItemInteractText(ItemTable.interactText[1]);
            Managers.Sound.SFX3DPlay(drawerOpen, transform);
        }
        else
        {
            StartCoroutine(CoMoveDrawer(drawer.transform.localPosition, Vector3.zero, drawer));
            isDrawerOpen = false;
            tableArr[index] = DataService.GetItemInteractText(ItemTable.interactText[0]);
            Managers.Sound.SFX3DPlay(drawerClose, transform);
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
