using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class FXHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource audioSource = null;

    [Header("VFX")]
    [SerializeField] ParticleSystem blockClearParticles = null;
    [SerializeField] ParticleSystem blockBrokenParticles = null;

    [Header("SFX")]
    [SerializeField] AudioClip blockClearSound = null;
    [SerializeField] AudioClip blockBrokenSound = null;

    private ParticleSystemRenderer blockClearParticleRenderer = null;
    private ParticleSystemRenderer blockBrokenParticleRenderer = null;

    internal void Initialize()
    {
        blockClearParticleRenderer = blockClearParticles.GetComponent<ParticleSystemRenderer>();
        blockBrokenParticleRenderer = blockBrokenParticles.GetComponent<ParticleSystemRenderer>();
    }

    internal void Deinitialize()
    {

    }

    public void HandleBlockCleared(Block block)
    {
        if(block.GetMesh() == null)
        {
            Debug.LogError("Block has no mesh");
        }

        if(blockClearParticleRenderer == null)
        {
            Debug.LogError("Renderer module is null");
        }

        blockClearParticles.transform.position = block.transform.position;
        blockClearParticleRenderer.mesh = CloneMesh(block.GetMesh());

        blockClearParticles.Play();

        audioSource.PlayOneShot(blockClearSound);
    }
    
    internal void HandleBlockBroken(Block block)
    {
        blockBrokenParticles.transform.position = block.transform.position;
        blockBrokenParticleRenderer.mesh = CloneMesh(block.GetMesh());

        blockBrokenParticles.Play();

        audioSource.PlayOneShot(blockBrokenSound);
    }

    private Mesh CloneMesh(Mesh source)
    {
        Mesh mesh = new Mesh();

        mesh.vertices = source.vertices.Select((x)=>x).ToArray();
        mesh.triangles = source.triangles.Select((x) => x).ToArray();
        mesh.normals = source.normals.Select((x) => x).ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }
}
