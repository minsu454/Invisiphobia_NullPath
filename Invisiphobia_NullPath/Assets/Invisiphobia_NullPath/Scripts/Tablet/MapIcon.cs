using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MapIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer CompletedRenderer;
    [SerializeField] private SpriteRenderer FillAmountRenderer;

    public void Init()
    {
        CompletedRenderer.color = Color.gray;
        CompletedRenderer.gameObject.SetActive(false);
        FillAmountRenderer.gameObject.SetActive(true);
        FillAmountRenderer.material.SetFloat("_FillAmount", 0);
    }

    public void Detected()
    {
        CompletedRenderer.gameObject.SetActive(true);
    }

    public void Detecting(float value)
    {
        FillAmountRenderer.material.SetFloat("_FillAmount", value);
    }

    public void Revealed()
    {
        FillAmountRenderer.gameObject.SetActive(false);
        CompletedRenderer.color = Color.white;
    }

    public void Invisible()
    {
        Init();
    }
}
