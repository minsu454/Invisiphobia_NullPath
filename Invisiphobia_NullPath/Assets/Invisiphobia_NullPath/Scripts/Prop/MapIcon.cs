using UnityEngine;

public class MapIcon : MonoBehaviour
{
    [SerializeField] private Transform targetTr;

    [SerializeField] private SpriteRenderer CompletedRenderer;
    [SerializeField] private SpriteRenderer FillAmountRenderer;

    private MaterialPropertyBlock propertyBlock;
    private float y = 15;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        transform.position = targetTr.position + (Vector3.up * y);
    }

    public void Init(Transform targetTr)
    {
        this.targetTr = targetTr;
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
