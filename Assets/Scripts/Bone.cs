using UnityEngine;

public class Bone : MonoBehaviour
{
    [SerializeField] private Material[] materials = new Material[2];
    private MeshRenderer meshRenderer;
    public bool isActive;
    public void Initialization()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = materials[0];
    }

    public void BeActive()
    {
        isActive = true;
        meshRenderer.sharedMaterial = materials[1];
    }

    public void BeUnActive()
    {
        isActive = false;
        meshRenderer.sharedMaterial = materials[0];
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
