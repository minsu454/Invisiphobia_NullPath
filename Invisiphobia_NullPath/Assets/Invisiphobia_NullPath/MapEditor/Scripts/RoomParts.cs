using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomParts : MonoBehaviour, IParts
{
    [SerializeField] private string description;
    public string Description { get { return description; } }

    [SerializeField] private List<MeshRenderer> floorList;
    [SerializeField] private List<MeshRenderer> wallList;

    private Material customFloorMaterial;
    private Material customWallMaterial;
    public Material CustomFloorMaterial { get { return customFloorMaterial; } }
    public Material CustomWallMaterial { get { return customWallMaterial; } }

    public void Init(Material floor, Material wall)
    {
        customFloorMaterial = floor;
        customWallMaterial = wall;

        SetFloorList(floor);
        SetWallList(wall);
    }

    private void SetFloorList(Material material)
    {
        foreach (var floor in floorList)
        {
            floor.sharedMaterial = material;
        }
    }

    private void SetWallList(Material material)
    {
        foreach (var wall in wallList)
        {
            wall.sharedMaterial = material;
        }
    }
}
