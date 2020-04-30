using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : PickupItem
{

    
    public override void OnPickUp(Transform item)
    {
        base.OnPickUp(item);
        print("Pick up ammo");
    }
}
