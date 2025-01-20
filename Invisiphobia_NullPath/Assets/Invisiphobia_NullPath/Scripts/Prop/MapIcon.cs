using UnityEngine;

public class MapIcon : MonoBehaviour
{
    [SerializeField] private Transform targetTr;                    //아이콘 띄워줄 목표 Transform

    [SerializeField] private SpriteRenderer CompletedRenderer;      //감지 아이콘 랜더러
    [SerializeField] private SpriteRenderer FillAmountRenderer;     //감지바 랜더러

    [SerializeField] private Color32 activecolor = Color.white;

    private MaterialPropertyBlock propertyBlock;                    //머터리얼 복사본 생성하지 않고 값 수정하기 위한 변수
    private float y = 15;                                           //아이콘 높이
        
    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        transform.position = targetTr.position + (Vector3.up * y);
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init(Transform targetTr)
    {
        this.targetTr = targetTr;
        ResetIcon();
    }

    /// <summary>
    /// 감지될때 실행될 함수(아직 투명하지만 알람은 울릴때)
    /// </summary>
    public void Detected()
    {
        CompletedRenderer.gameObject.SetActive(true);
        SetFillAmount(0);
    }

    /// <summary>
    /// 감지 바 채우는 중 실행될 함수(아직 투명하지만 알람은 울릴때)
    /// </summary>
    public void Detecting()
    {
        CompletedRenderer.gameObject.SetActive(true);
    }

    /// <summary>
    /// 드러날때 실행될 함수(태블릿에서 확인해서 투명화 해제)
    /// </summary>
    public void Revealed()
    {
        FillAmountRenderer.gameObject.SetActive(false);
        CompletedRenderer.color = activecolor;
    }

    /// <summary>
    /// 테블릿에서 사라졌을 때 실행될 함수
    /// </summary>
    public void Invisible()
    {
        ResetIcon();
    }

    /// <summary>
    /// 아이콘 리셋해주는 함수
    /// </summary>
    private void ResetIcon()
    {
        CompletedRenderer.color = Color.gray;
        CompletedRenderer.gameObject.SetActive(false);
        FillAmountRenderer.gameObject.SetActive(true);
        SetFillAmount(0);
    }

    /// <summary>
    /// 감지바 설정 함수
    /// </summary>
    public void SetFillAmount(float value)
    {
        FillAmountRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_FillAmount", value);
        FillAmountRenderer.SetPropertyBlock(propertyBlock);
    }
}
