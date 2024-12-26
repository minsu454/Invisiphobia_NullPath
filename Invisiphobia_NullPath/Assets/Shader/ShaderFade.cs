using UnityEngine;

public class ShaderFade : MonoBehaviour
{
    private Material objectMaterial;
    private UnityEngine.AI.NavMeshAgent agent;
    
    void Start()
    {
        objectMaterial = GetComponent<Renderer>().material;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();  // NavMeshAgent가 필요한 경우
    }

    public void StartFade(Vector3 targetPosition)
    {
        if (objectMaterial != null && agent != null)
        {
            StartCoroutine(FadeOut(targetPosition));
        }
    }

    private System.Collections.IEnumerator FadeOut(Vector3 targetPosition)
    {
        float startDistance = Vector3.Distance(transform.position, targetPosition);

        while (true)
        {
            // 현재 위치에서 목표 지점까지의 거리
            float remainingDistance = Vector3.Distance(transform.position, targetPosition);

            // 목표 지점에서 1만큼 남았을 때부터 fade 시작
            if (remainingDistance <= 1f)
            {
                // 남은 거리가 1에서 0으로 줄어드는 동안 알파값을 1에서 0으로 변경
                float fadeValue = Mathf.Clamp01((1f - remainingDistance) / 1f);
                objectMaterial.SetFloat("_Fade", fadeValue);
            }

            // 남은 거리가 0보다 작거나 같으면 끝내기
            if (remainingDistance <= 0f)
            {
                objectMaterial.SetFloat("_Fade", 0f);

                // 완전히 투명해지면 SetActive(false) 호출
                if (objectMaterial.GetFloat("_Fade") <= 0f)
                {
                    gameObject.SetActive(false);
                }

                break;
            }

            yield return null;
        }
    }
}