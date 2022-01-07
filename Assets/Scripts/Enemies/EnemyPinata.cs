using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPinata : Enemy
{
    [Header("Pinata")]
    [SerializeField] Transform principalMesh;
    float audioReaction;
    [SerializeField] AnimationCurve destroyCurve;
    [SerializeField] float breakForce = 100;
    int typesNum;

    void Start()
    {
        
    }


    void Update()
    {
        float audioReact = AudioReaction.Instance.GetDropValue();

        transform.Rotate(0.1f *audioReact , 0.15f * audioReact, 0);
        audioReaction = 1 + factor;
    }

    public void ChangeValue()
    {
        typesNum++;
        typesNum = Mathf.Clamp(typesNum, 0, principalMesh.childCount);
        principalMesh.localScale *= 1.03f;
        for (int i = 0; i < typesNum; i++)
        {
            float rand = Random.Range(0, 0.07f);
            principalMesh.GetChild(i).localScale = principalMesh.GetChild(i).localScale * (1-rand);
        }
        //foreach (Rigidbody rb in principalMesh.transform.GetComponentsInChildren<Rigidbody>())
        //{
        //    Vector3 force = (rb.transform.position - principalMesh.transform.position).normalized * breakForce;
        //    force = new Vector3(100,100,100);
        //    Debug.Log(force);
        //    rb.AddForce(force);
        //}
    }
}
