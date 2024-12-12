public interface IInteractable
{
    /// <summary>
    /// 아이템 테이블
    /// </summary>
    public ItemTable ItemTable  { get; }

    /// <summary>
    /// 상호작용 텍스트
    /// </summary>
    public string InteractText  { get; }
    /// <summary>
    /// 키 입력 행동 텍스트
    /// </summary>
    public string ActionText    { get; }

    /// <summary>
    /// 아이템이 들어나 있는지 체크하는 bool
    /// </summary>
    public bool IsReveal        { get; }

    /// <summary>
    /// 플레이어 상호작용 함수
    /// </summary>
    public void Interact(Player player);
}
