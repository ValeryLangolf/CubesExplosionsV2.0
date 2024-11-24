using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CubeView : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    public void Modify(float scale)
    {
        gameObject.transform.localScale = Vector3.one * scale;
        _renderer.material.color = new(Random.value, Random.value, Random.value);
    }
}