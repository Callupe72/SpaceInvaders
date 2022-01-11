using DG.Tweening;
using UnityEngine;

public class MovingPlanet : MonoBehaviour
{
    float timeToGo;
    Player player;
    [SerializeField] MeshRenderer meshRenderer;

    public Color[] colors;

    public bool doIRandomizeColor = true;
    [SerializeField] Material sunMat;

    void Start()
    {
        player = FindObjectOfType<Player>();
        Spawn();
    }

    void Spawn()
    {
        int offset = Random.Range(10000, 20000);
        transform.DOMoveZ(player.transform.position.z + offset, .01f);
        if (doIRandomizeColor)
        {
            Color color = colors[Random.Range(0, colors.Length)];
            meshRenderer.materials[0].SetColor("_BaseColor", color);
        }
        else
        {
            meshRenderer.materials[0] = sunMat;
        }
        float scale = Random.Range(1, 3);
        timeToGo = offset / 400;
        transform.localScale = Vector3.one * scale * 100;
        transform.DOMoveZ(player.transform.position.z - 10, timeToGo).OnComplete(() => Spawn());
    }
}
