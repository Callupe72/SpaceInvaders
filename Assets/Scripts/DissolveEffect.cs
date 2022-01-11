using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    MeshRenderer mesh;
    SkinnedMeshRenderer skinnedMesh;
    [Header("After effect")]
    public bool dieAfter;
    [Header("Dissolve")]
    public Shader shader;
    public float borderWidth = 0.03f;
    public float noiseScale = 6;
    public float speed = 5;
    [ColorUsage(true, true)] public Color color;

    [HideInInspector] public List<MeshRenderer> multiMesh;
    [HideInInspector] public float activeEffectAfterTime;
    [HideInInspector] public bool dissolveMultipleObj;

    [HideInInspector] public List<Material> mat;
    [HideInInspector] public List<Texture> texture;
    [HideInInspector] public Material[] oldMat;
    bool dissolve;
    bool alreadyCalled;
    bool showOn;
    float _dissolveValue;
    Color colorTexture;

    void Update()
    {
        Dissolve(dissolveMultipleObj);
    }
    public void ChangeMat(bool _showOn)
    {
        showOn = _showOn;
        if (dissolveMultipleObj)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                multiMesh.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
            }
            StartCoroutine(WaitBeforeDissolve());
            return;
        }
        else
        {
            if (GetComponent<MeshRenderer>())
                mesh = GetComponent<MeshRenderer>();
            else
                skinnedMesh = GetComponent<SkinnedMeshRenderer>();
        }


        if (!alreadyCalled)
        {
            if (mesh)
            {
                mat.Clear();
                alreadyCalled = true;
                for (int i = 0; i < mesh.sharedMaterials.Length; i++)
                {
                    Material material = new Material(shader);
                    mat.Add(material);
                    if (mesh.sharedMaterials[i].mainTexture != null)
                    {
                        texture.Add(mesh.sharedMaterials[i].mainTexture);
                    }
                    else
                    {
                        texture.Add(null);
                    }
                }

                for (int i = 0; i < mesh.sharedMaterials.Length; i++)
                {
                    mat[i].SetTexture("_BaseMap", texture[i]);
                    if (_showOn)
                    {
                        mat[i].SetFloat("_DissolveScale", 1);
                    }
                    else
                    {
                        mat[i].SetFloat("_DissolveScale", -1);
                    }
                    mat[i].SetFloat("_NoiseScale", noiseScale);
                    mat[i].SetFloat("_BorderWidth", borderWidth);
                    mat[i].SetColor("_BaseColor", color);
                }

                oldMat = mesh.sharedMaterials;

                mesh.sharedMaterials = mat.ToArray();

                if (_showOn)
                {
                    _dissolveValue = 1;
                }
                else
                {
                    _dissolveValue = -1;
                }
                dissolve = true;
            }
            else
            {
                mat.Clear();
                alreadyCalled = true;
                for (int i = 0; i < skinnedMesh.sharedMaterials.Length; i++)
                {
                    Material material = new Material(shader);
                    mat.Add(material);
                    if (skinnedMesh.sharedMaterials[i].mainTexture != null)
                    {
                        texture.Add(skinnedMesh.sharedMaterials[i].mainTexture);
                    }
                    else
                    {
                        texture.Add(null);
                    }
                }

                for (int i = 0; i < skinnedMesh.sharedMaterials.Length; i++)
                {
                    mat[i].SetTexture("_BaseMap", texture[i]);
                    if (_showOn)
                    {
                        mat[i].SetFloat("_DissolveScale", 1);
                    }
                    else
                    {
                        mat[i].SetFloat("_DissolveScale", -1);
                    }
                    mat[i].SetFloat("_NoiseScale", noiseScale);
                    mat[i].SetFloat("_BorderWidth", borderWidth);
                    mat[i].SetColor("_BaseColor", color);
                }
                skinnedMesh.sharedMaterials = mat.ToArray();

                if (_showOn)
                {
                    _dissolveValue = 1;
                }
                else
                {
                    _dissolveValue = -1;
                }
                dissolve = true;
            }
        }

    }

    IEnumerator WaitBeforeDissolve()
    {
        yield return new WaitForSeconds(activeEffectAfterTime);
        ChangeMultipleMesh();
    }

    void ChangeMultipleMesh()
    {
        if (!alreadyCalled)
        {
            alreadyCalled = true;
            Material material = new Material(shader);
            mat.Add(material);
            if (multiMesh[0].sharedMaterials[0].mainTexture)
            {
                texture.Add(multiMesh[0].sharedMaterials[0].mainTexture);
            }
            else
            {
                texture.Add(null);
            }
            colorTexture = multiMesh[0].sharedMaterials[0].color;
            mat[0].SetTexture("_BaseMap", texture[0]);
            if (showOn)
            {
                mat[0].SetFloat("_DissolveScale", 1);
            }
            else
            {
                mat[0].SetFloat("_DissolveScale", -1);
            }
            mat[0].SetFloat("_NoiseScale", noiseScale);
            mat[0].SetFloat("_BorderWidth", borderWidth);
            mat[0].SetColor("_BaseColor", color);
            mat[0].SetColor("_ColorTexture", colorTexture);

            for (int i = 0; i < multiMesh.Count; i++)
            {
                multiMesh[i].sharedMaterial = mat[0];
            }
            if (showOn)
            {
                _dissolveValue = 1;
            }
            else
            {
                _dissolveValue = -1;
            }
            dissolve = true;
        }
    }

    void Dissolve(bool editMultiple)
    {
        if (dissolve)
        {
            if (!showOn)
            {
                _dissolveValue += (speed / 1000 * Time.timeScale);

                if (!editMultiple)
                {
                    if (mesh)
                    {
                        for (int i = 0; i < mesh.sharedMaterials.Length; i++)
                        {
                            mesh.sharedMaterials[i].SetFloat("_DissolveScale", _dissolveValue);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < skinnedMesh.sharedMaterials.Length; i++)
                        {
                            skinnedMesh.sharedMaterials[i].SetFloat("_DissolveScale", _dissolveValue);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < multiMesh.Count; i++)
                    {
                        multiMesh[i].sharedMaterials[0].SetFloat("_DissolveScale", _dissolveValue);
                    }
                }

                if (_dissolveValue >= 0.95f)
                {
                    mesh.sharedMaterials = oldMat;

                    if (dieAfter)
                    {
                        Die();
                    }
                    dissolve = false;
                    alreadyCalled = false;
                }
            }
            else
            {
                _dissolveValue -= (speed / 1000 * Time.timeScale);

                if (!editMultiple)
                {
                    if (mesh)
                    {
                        for (int i = 0; i < mesh.sharedMaterials.Length; i++)
                        {
                            mesh.sharedMaterials[i].SetFloat("_DissolveScale", _dissolveValue);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < skinnedMesh.sharedMaterials.Length; i++)
                        {
                            skinnedMesh.sharedMaterials[i].SetFloat("_DissolveScale", _dissolveValue);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < multiMesh.Count; i++)
                    {
                        multiMesh[i].sharedMaterials[0].SetFloat("_DissolveScale", _dissolveValue);
                    }
                }

                if (_dissolveValue <= -0.95f)
                {
                    if (dieAfter)
                    {
                        Die();
                    }

                    //mesh.sharedMaterials = mat.ToArray();

                    dissolve = false;
                    alreadyCalled = false;


                    mesh.sharedMaterials = oldMat;

                    this.enabled = false;
                }
            }
            if (dissolveMultipleObj)
            {
                _dissolveValue = multiMesh[0].sharedMaterial.GetFloat("_DissolveScale");
            }
            else
            {
                if (mesh)
                {
                    _dissolveValue = mesh.sharedMaterial.GetFloat("_DissolveScale");
                }
                else
                {
                    _dissolveValue = skinnedMesh.sharedMaterial.GetFloat("_DissolveScale");
                }
            }

        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
