using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : BaseItem
{
    public override GameObject Interact()
    {
        return gameObject;
    }
}
