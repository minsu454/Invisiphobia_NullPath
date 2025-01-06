using Common.Objects;
using Common.Path;
using Common.StringEx;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;

    [SerializeField] private Player player;
    public Player Player { get { return player; } set { player = value; } }
    [SerializeField] private List<Monster> monsterList = new List<Monster>();
    public List<Monster> MonsterList { get { return monsterList; } }

    #region Test

    public void Awake()
    {
        Instance = this;
    }
    #endregion

    public void Init()
    {
        player.Init();

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
