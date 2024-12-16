using UnityEngine;

public class ShaderFade : MonoBehaviour
{
    public float fadeDuration = 2f;
    private Material objectMaterial;

    void Start()
    {
        objectMaterial = GetComponent<Renderer>().material;
    }

    public void StartFade()
    {
        if (objectMaterial != null)
        {
            StartCoroutine(FadeOut());
        }
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float fadeValue = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            objectMaterial.SetFloat("_Fade", fadeValue);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}