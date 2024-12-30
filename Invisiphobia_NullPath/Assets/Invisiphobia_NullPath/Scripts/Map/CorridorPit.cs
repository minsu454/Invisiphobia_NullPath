using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorPit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.Die();
        }
    }
}
