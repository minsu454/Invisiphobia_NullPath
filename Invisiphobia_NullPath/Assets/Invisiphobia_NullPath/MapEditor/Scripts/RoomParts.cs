using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomParts : MonoBehaviour, IParts
{
    [SerializeField] private string description;
    public string Description { get { return description; } }

    [SerializeField] private List<MeshRenderer> floorList;
    [SerializeField] private List<MeshRenderer> wallList;

    public Material CustomFloorMaterial { get; private set; }
    public Material CustomWallMaterial { get; private set; }

    public void Init(Material floor, Material wall)
    {
        CustomFloorMaterial = floor;
        CustomWallMaterial = wall;

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
