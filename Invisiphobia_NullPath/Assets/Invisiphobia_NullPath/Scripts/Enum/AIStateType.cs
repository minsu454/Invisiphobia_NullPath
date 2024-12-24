public enum AIStateType
{
    Idle,           // 투명화로 멈춰있는 상태
    Wandering,      // 잠시 어슬렁거리는 상태
    Attacking,      // 플레이어를 쫓고 공격하는 상태
    Fleeing,        // 플레이어가 몬스터로부터 도망치는 상태
    MonsterFleeing  // 몬스터가 플레이어로부터 공격받은 상태
}
