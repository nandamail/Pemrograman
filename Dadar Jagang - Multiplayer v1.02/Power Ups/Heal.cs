using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
   public int heal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hostile")
        {
            HostileMark.instance.HealPlayer(heal);

            Destroy(gameObject);
        }
    }
}
