using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;

    public Player Player;

    [SerializeField] private List<Monster> monsterList = new List<Monster>();
    public List<Monster> MonsterList { get { return monsterList; } }

    public void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        foreach (var monster in monsterList)
        {
            monster.Init();
        }
    }

    public void AddMonster(Monster monster)
    {
        if (monsterList.Contains(monster))
            return;
        
        monsterList.Add(monster);
    }

    public void RemoveMonster(Monster monster)
    {
        if (!monsterList.Contains(monster))
            return;
        
        monsterList.Remove(monster);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
