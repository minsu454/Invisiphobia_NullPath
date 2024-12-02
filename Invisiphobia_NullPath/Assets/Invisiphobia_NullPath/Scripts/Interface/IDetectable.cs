using System.Collections;
using UnityEngine;

public interface IDetectable
{
    public Transform transform { get; }

    public void Detected(); // 감지될때 실행될 로직(아직 투명하지만 알람은 울릴때)

    public void Revealed(); // 드러날때 실행될 로직(태블릿에서 확인해서 투명화 해제)

    public void Invisible(); // 사라졌을 때 실행될 로직
}
