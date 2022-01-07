using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPinata : Enemy
{
    float factor;
    float audioReaction;

    void Update()
    {
        audioReaction = AudioReaction.Instance.GetDropValue();
        transform.localScale = Vector3.one * audioReaction;
        foreach (Transform item in transform)
        {
            item.localScale = Vector3.one * (1 - factor);
        }
    }
}
