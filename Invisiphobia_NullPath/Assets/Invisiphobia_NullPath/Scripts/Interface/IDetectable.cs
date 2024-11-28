using System.Collections;
using UnityEngine;

public interface IDetectable
{
    public Transform transform { get; set; }
    public void Detected();

    public void Revealed();
}
