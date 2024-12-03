using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour, IDetectable
{
    [SerializeField] private MeshRenderer myRenderer;
    [SerializeField] private Collider myCollider;
    [SerializeField] private SpriteRenderer mapIcon;

    public bool isRevealed = false;
    /// <summary>
    /// Prop 초기화 함수
    /// </summary>
    public virtual void Init()
    {
        
    }

    private void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.enabled = false;
        myCollider = GetComponent<Collider>();
    }

    public void Detected()
    {
        // UI아이콘 활성화
    }

    public void Revealed()
    {
        myRenderer.enabled = true;
        isRevealed = true;
    }

    public void Invisible()
    {
        throw new System.NotImplementedException();
    }
}
