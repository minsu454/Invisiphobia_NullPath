public enum AIStateType
{
    Idle,           // 투명화로 멈춰있는 상태
    Wandering,      // 잠시 어슬렁거리는 상태
    Attacking,      // 플레이어를 쫓고 공격하는 상태
    Fleeing,        // 플레이어가 도망쳤을 때의 상태
    Stun            // 공격을 받고 기절한 상태
}
