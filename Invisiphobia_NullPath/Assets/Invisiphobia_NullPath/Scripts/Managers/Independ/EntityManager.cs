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
    public Player Player { get { return player; } }
    private readonly List<Monster> monsterList = new List<Monster>();

    #region Test
    public List<Monster> monsterTestList;

    public void Awake()
    {
        Instance = this;
    }
    #endregion

    public void Init(TotalMapData totalData)
    {
        new GameObject("-----------Entity-------------");
        Setting(totalData.EntityData);
    }

    private void Setting(EntityData entityData)
    {
        {
            string name = entityData.playerData.Name.ToFirstName("_");
            GameObject go = ObjectManager.Instantiate(AddressablePath.EntityPath(name));

            go.name = name;
            go.transform.position = entityData.playerData.Pos;
            go.transform.rotation = entityData.playerData.Rot;

            player = go.GetComponent<Player>();

            player.Init();
        }

        foreach (PointData data in entityData.monsterDataList)
        {
            string name = data.Name.ToFirstName("_");
            GameObject go = ObjectManager.Instantiate(AddressablePath.EntityPath(name));

            go.name = name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;

            Monster monster = go.GetComponent<Monster>();

            AddMonster(monster);

            monster.Init();
        }
    }

    private void AddMonster(Monster monster)
    {
        if (monsterList.Contains(monster))
            return;
        
        monsterList.Add(monster);
    }

    private void RemoveMonster(Monster monster)
    {
        if (!monsterList.Contains(monster))
            return;
        
        monsterList.Remove(monster);
    }
}
