using System.Collections.Generic;
using UnityEngine;

public class Detonator : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private float _radius;

    public float RadiusFromScale => _radius / transform.localScale.x;

    public void Detonate(List<Rigidbody> cubes)
    {
        float force = _force / transform.localScale.x;
        float radius = RadiusFromScale;

        foreach (Rigidbody cube in cubes)
        {
            cube.AddExplosionForce(force, transform.position, radius);
        }
    }
}