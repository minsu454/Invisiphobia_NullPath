using System.Runtime.CompilerServices;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private bool isFirst = false;

    private Vector3 savePos;

    private Vector3 firstGameObjectPos;
    private Vector3 firstInputPos;

    public void Init()
    {
        EntityManager.Instance.Player.PlayerController.playerWheelClickActionEvent += OnMove;
    }

    public void OnTab()
    {
        savePos = transform.position;
    }

    public void ResetPos()
    {
        transform.position = savePos;
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