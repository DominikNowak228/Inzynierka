using UnityEngine;
// MaterialSetter
[RequireComponent(typeof(MeshRenderer))]
public class MaterialSetter : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetSingleMaterial(Material material)
    {
        _meshRenderer.material = material;
    }
}
