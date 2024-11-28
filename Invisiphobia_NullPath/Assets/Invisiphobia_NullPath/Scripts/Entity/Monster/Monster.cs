using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity , IDetectable
{
    [SerializeField] private MonsterController controller;
    public override void Init()
    {
        base.Init();
    }

    private void Awake()
    {
        
    }

    public void Detected()
    {

    }

    public void Revealed()
    {
        //controller.isRevealed = true;
    }

}
