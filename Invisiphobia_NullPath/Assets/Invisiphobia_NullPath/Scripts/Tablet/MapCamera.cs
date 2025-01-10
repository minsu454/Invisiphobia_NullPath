using System.Runtime.CompilerServices;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private bool isFirst = false;

    private Vector3 savePos;

    private Vector3 firstGameObjectPos;
    private Vector3 firstInputPos;

    private Transform playerTr;

    public void Init()
    {
        playerTr = EntityManager.Instance.Player.transform;
        EntityManager.Instance.Player.PlayerController.playerWheelClickActionEvent += OnMove;
    }

    public void ResetPos()
    {
        transform.position = playerTr.position + (Vector3.up * 20);
    }

    private void OnMove(bool active)
    {
        if (!active)
        {
            isFirst = true;

            return;
        }

        if (isFirst)
        {
            firstGameObjectPos = transform.position;
            firstInputPos = Input.mousePosition;
            isFirst = false;
        }
        else
        {
            Vector3 movePos = Camera.main.ScreenToViewportPoint(firstInputPos - Input.mousePosition);

            movePos.z = movePos.y;
            movePos.y = 0;

            transform.position = firstGameObjectPos + (movePos * moveSpeed);
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90f, 0, 0); //회전값 고정
    }
}