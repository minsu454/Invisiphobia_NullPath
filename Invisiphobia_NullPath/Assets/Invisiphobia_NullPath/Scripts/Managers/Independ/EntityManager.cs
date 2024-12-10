using Common.Objects;
using Common.Path;
using Common.StringEx;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;

    public Player Player { get; private set; }
    private readonly HashSet<Monster> monsterHashSet = new HashSet<Monster>();

    #region Test
    public List<Monster> monster;

    public void Awake()
    {
        Instance = this;
    }
    #endregion

    public void Init(TotalMapData totalData)
    {
        Setting(totalData.EntityData);
    }

    private void Setting(EntityData entityData)
    {
        { 
            GameObject go = ObjectManager.Instantiate(AddressablePath.EntityPath(entityData.playerData.Name));

            go.name = entityData.playerData.Name;
            go.transform.position = entityData.playerData.Pos;
            go.transform.rotation = entityData.playerData.Rot;

            Player = go.GetComponent<Player>();
        }

        foreach (PointData data in entityData.monsterDataList)
        {
            string name = data.Name.ToFirstName("_");
            GameObject go = ObjectManager.Instantiate(AddressablePath.EntityPath(name));

            go.name = data.Name;
            go.transform.position = data.Pos;
            go.transform.rotation = data.Rot;

            Monster monster = go.GetComponent<Monster>();

            AddMonster(monster);
        }
    }

    private void AddMonster(Monster monster)
    {
        if (monsterHashSet.Contains(monster))
            return;
        
        monsterHashSet.Add(monster);
    }

    private void RemoveMonster(Monster monster)
    {
        if (!monsterHashSet.Contains(monster))
            return;
        
        monsterHashSet.Remove(monster);
    }
}
