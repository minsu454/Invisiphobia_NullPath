using Common.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeMonsterKillScene : MonsterKillScene
{
    protected override void Kill()
    {
        target.Die();
    }
}
