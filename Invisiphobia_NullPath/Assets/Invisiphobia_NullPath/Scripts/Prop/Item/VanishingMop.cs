using UnityEngine;
using System.Collections;
using Common.Yield;

public class VanishingMop : Prop
{
    [SerializeField] private Animator animator;
    [SerializeField] private float fieldOfView = 90f;

    Transform targetTr;
    Camera playerCamera;

    public void Start()
    {
        targetTr = EntityManager.Instance.Player.transform;
        playerCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CheckVisibilityCoroutine());
        }
    }

    private IEnumerator CheckVisibilityCoroutine()
    {
        while (true)
        {
            if (StateType == PropStateType.Revealed)
            {
                Vector3 dir = transform.position - targetTr.position;
                
                dir.Normalize();
                float angle = Vector3.Angle(targetTr.forward, dir);

                if (angle < fieldOfView * 0.5f)
                {
                    animator.SetBool("Vanish", true);
                    yield return YieldCache.WaitForSeconds(5);
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.Log("타겟이 카메라 시야 밖에 있습니다.");
                }

                yield return YieldCache.WaitForSeconds(0.02f);
            }

            yield return null;
        }
    }
}