using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class PuzzleUI : WorldUI<TabletStateType>
{
    [Header("Puzzle")]
    [SerializeField] private GameObject progressBackground;     //진행 바 배경 변수
    [SerializeField] private Image progressBar;                 //진행 바 변수

    public event Action OnCompletedEvent;                       //클리어시 이벤트
    protected void OnComplete()
    {
        OnCompletedEvent?.Invoke();
    }
}
