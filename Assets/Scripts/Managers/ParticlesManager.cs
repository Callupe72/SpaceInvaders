using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    [SerializeField] bool canParticle = true;


    [System.Serializable]
    public struct Particles
    {
        public string name;
        public GameObject particle;
        public Vector3 scale;
        [Range(0,5)] public float speed;
    }

    public Particles[] particles;

    public static ParticlesManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void SpawnParticles(string name, Transform parentTransform, Vector3 rotation, bool setToParent)
    {
        if (!canParticle)
            return;
        Particles p = Array.Find(particles, particle => particle.name == name);

        GameObject particleGo = Instantiate(p.particle, parentTransform.position, Quaternion.Euler(rotation));
        if(setToParent)
            particleGo.transform.parent = parentTransform;
        ParticleSystem particlesComponent = particleGo.GetComponent<ParticleSystem>();
        particlesComponent.Play();
        particleGo.transform.localScale = p.scale;
        particlesComponent.playbackSpeed = p.speed;
        foreach (Transform item in particleGo.transform)
        {
            if (GetComponent<ParticleSystem>())
            {
                item.GetComponent<ParticleSystem>().playbackSpeed = p.speed;
            }
        }
    }

    public void SetCanParticles(bool isTrue)
    {
        canParticle = isTrue;
    }

    public bool GetCanParticles()
    {
        return canParticle;
    }
}
