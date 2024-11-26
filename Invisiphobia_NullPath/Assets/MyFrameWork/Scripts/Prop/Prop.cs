using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour, IDetectable
{
    [SerializeField] private MeshRenderer renderer;

    /// <summary>
    /// Prop 초기화 함수
    /// </summary>
    public virtual void Init()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.enabled = false;
    }

    public void Detected()
    {
        // UI아이콘 활성화
    }

    public void Revealed()
    {
        renderer.enabled = true;
    }
}
