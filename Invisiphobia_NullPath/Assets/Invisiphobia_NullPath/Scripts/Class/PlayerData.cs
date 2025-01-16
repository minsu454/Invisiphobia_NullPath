using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Vector3 Pos;
    public Quaternion Rot;
    public int InHandItemId;
    public float battery;

    public PlayerData()
    {

    }

    public PlayerData(Vector3 pos, Quaternion rot, int inHandItemId, float battery)
    {
        Pos = pos;
        Rot = rot;
        InHandItemId = inHandItemId;
        this.battery = battery;
    }
}