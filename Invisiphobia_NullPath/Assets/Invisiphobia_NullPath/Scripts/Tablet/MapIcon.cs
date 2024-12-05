using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MapIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer CompletedRenderer;
    [SerializeField] private SpriteRenderer FillAmountRenderer;

    private MaterialPropertyBlock propertyBlock;

    public void Init()
    {
        propertyBlock = new MaterialPropertyBlock();

        ResetIcon();
    }

    public void Detected()
    {
        CompletedRenderer.gameObject.SetActive(true);
        SetFillAmount(0);
    }

    public void Revealed()
    {
        FillAmountRenderer.gameObject.SetActive(false);
        CompletedRenderer.color = Color.white;
    }

    public void Invisible()
    {
        ResetIcon();
    }

    private void ResetIcon()
    {
        CompletedRenderer.color = Color.gray;
        CompletedRenderer.gameObject.SetActive(false);
        FillAmountRenderer.gameObject.SetActive(true);
        SetFillAmount(0);
    }

    public void SetFillAmount(float value)
    {
        FillAmountRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_FillAmount", value);
        FillAmountRenderer.SetPropertyBlock(propertyBlock);
    }
}
