using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUI : WorldUI
{
    public override void Init(IActiveStatable subject)
    {
        
    }

    public override void Subscribe(IActiveStatable subject)
    {
    }

    public override void Unsubscribe(IActiveStatable subject)
    {
        gameObject.SetActive(false);    
    }
}
