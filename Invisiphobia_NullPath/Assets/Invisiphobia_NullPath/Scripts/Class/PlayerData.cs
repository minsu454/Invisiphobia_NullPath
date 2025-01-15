using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Vector3 Pos;
    public Quaternion Rot;
    public int InHandItemId;

    public PlayerData(Vector3 pos, Quaternion rot, int inHandItemId)
    {
        Pos = pos;
        Rot = rot;
        InHandItemId = inHandItemId;
    }
}