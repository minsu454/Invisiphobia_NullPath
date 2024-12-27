using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Material fadeMaterial;
    private float fadeDuration = 2f;

    public void StartFade(System.Action onFadeComplete)
    {
        StartCoroutine(FadeToBlack(onFadeComplete));
    }

    private IEnumerator FadeToBlack(System.Action onFadeComplete)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float fadeAmount = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeMaterial.SetFloat("_Fade", fadeAmount);
            yield return null;
        }

        fadeMaterial.SetFloat("_Fade", 1f);
        onFadeComplete?.Invoke();
    }
}