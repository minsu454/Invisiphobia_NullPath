using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TabletUIManager : MonoBehaviour, IActiveStatable
{
    [SerializeField] private List<WorldUI> worldUIList = new List<WorldUI>();
    private int choiceIdx = 0;
    public int ChoiceIdx
    {
        get { return choiceIdx; }
        set
        {
            if (choiceIdx == value)
            {
                return;
            }

            SwitchTabletScreen(value);
        }
    }

    public event Action BasicStateEvent;
    public event Action ActiveStateEvent;

    public void Init(Tablet tablet)
    {
        foreach (WorldUI worldUI in worldUIList)
        {
            worldUI.Init(this);
            worldUI.gameObject.SetActive(false);
        }

        choiceIdx = 0;

        worldUIList[choiceIdx].gameObject.SetActive(true);
        worldUIList[choiceIdx].Subscribe(this);

        tablet.OnStateChangedEvent += OnStateChanged;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnStateChanged(TabletStateType type)
    {
        switch (type)
        {
            case TabletStateType.Basic:
                BasicStateEvent?.Invoke();
                break;
            case TabletStateType.Activate:
                ActiveStateEvent?.Invoke();
                break;
        }
    }

    private void SwitchTabletScreen(int num)
    {
        worldUIList[ChoiceIdx].Unsubscribe(this);

        worldUIList[num].gameObject.SetActive(true);
        worldUIList[num].Subscribe(this);

        choiceIdx = num;
    }
}