using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private float _radius;

    public float RadiusFromScale => _radius / transform.localScale.x;

    public void Split(List<Rigidbody> cubes)
    {
        Explode(cubes, _force, _radius);
    }

    public void Bang(List<Rigidbody> cubes)
    {
        float force = _force / transform.localScale.x;
        float radius = RadiusFromScale;

        Explode(cubes, force, radius);
    }

    private void Explode(List<Rigidbody> cubes, float force, float radius)
    {
        foreach (Rigidbody cube in cubes)
        {
            cube.AddExplosionForce(force, transform.position, radius);
        }
    }
}