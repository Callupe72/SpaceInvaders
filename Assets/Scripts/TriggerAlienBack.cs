using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAlienBack : MonoBehaviour
{
    [SerializeField]
    Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            player.TakeDommage(1);
        }
    }
}
