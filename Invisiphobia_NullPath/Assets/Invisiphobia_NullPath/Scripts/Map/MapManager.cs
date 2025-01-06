using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public List<Prop> itemPartsList = new List<Prop>();

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        Item();
    }

    /// <summary>
    /// 아이템 생성 함수
    /// </summary>
    private void Item()
    {
        foreach (Prop prop in itemPartsList)
        {
            prop.Init();
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
