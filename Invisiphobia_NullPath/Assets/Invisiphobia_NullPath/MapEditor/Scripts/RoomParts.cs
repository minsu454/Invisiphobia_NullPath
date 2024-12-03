using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomParts : Parts
{
    [SerializeField] private string description;
    public string Description { get { return description; } }

    [SerializeField] private List<MeshRenderer> floorList;
    [SerializeField] private List<MeshRenderer> wallList;

    public Material FloorMaterial { get; private set; }
    public Material WallMaterial { get; private set; }

    public void Init(Material floor, Material wall)
    {
        FloorMaterial = floor;
        WallMaterial = wall;

        SetFloorList(floor);
        SetWallList(wall);
    }

    private void SetFloorList(Material material)
    {
        foreach (var floor in floorList)
        {
            floor.material = material;
        }
    }

    private void SetWallList(Material material)
    {
        foreach (var wall in wallList)
        {
            wall.material = material;
        }
    }
}

