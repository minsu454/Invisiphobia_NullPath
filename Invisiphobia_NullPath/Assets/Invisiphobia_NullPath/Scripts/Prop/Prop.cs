using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour, IDetectable
{
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Collider collider;

    public bool isRevealed = false;
    /// <summary>
    /// Prop 초기화 함수
    /// </summary>
    public virtual void Init()
    {

    }

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.enabled = false;
        collider = GetComponent<Collider>();
    }

    public void Detected()
    {
        // UI아이콘 활성화
    }

    public void Revealed()
    {
        renderer.enabled = true;
        isRevealed = true;
    }
}
