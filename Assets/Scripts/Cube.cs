using System;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private CubeView _cubeView;
    [SerializeField] private Explosion _explosion;
    [SerializeField] private float _decayProbability = 1f;
    [SerializeField] private float _robabilityMultiplier = 0.5f;
    [SerializeField] private float _scaleMultiplier = 0.5f;

    public event Action<Cube> Separation;
    public event Action<Cube> Exploded;

    public float DecayProbability => _decayProbability;

    private List<Rigidbody> CubesInRadius
    {
        get
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _explosion.RadiusFromScale);
            List<Rigidbody> cubesRigidbodies = new List<Rigidbody>();

            foreach (Collider hit in hits)
            {
                if (hit.attachedRigidbody != null)
                    cubesRigidbodies.Add(hit.attachedRigidbody);
            }

            return cubesRigidbodies;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (_decayProbability > UnityEngine.Random.value)
        {
            Separation?.Invoke(this);
        }
        else
        {
            _explosion.Bang(CubesInRadius);
            Exploded?.Invoke(this);
        }            

        Destroy(gameObject);
    }

    public void Initialize(float scale, float decayProbability)
    {
        _decayProbability = decayProbability * _robabilityMultiplier;
        _cubeView.Modify(scale * _scaleMultiplier);
    }

    public void Explode(List<Rigidbody> rigidbodies)
    {
        _explosion.Split(rigidbodies);
    }
}