using System;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Visualizer _visualizer;
    [SerializeField] private Detonator _detonator;
    [SerializeField] private float _decayProbability = 1f;
    [SerializeField] private float _robabilityMultiplier = 0.5f;
    [SerializeField] private float _scaleMultiplier = 0.5f;

    public event Action<Cube> Separating;
    public event Action<Cube> Detonated;

    public float DecayProbability => _decayProbability;

    private void OnMouseUpAsButton()
    {
        if (_decayProbability > UnityEngine.Random.value)
        {
            Separating?.Invoke(this);
        }
        else
        {
            List<Rigidbody> cubesInRadius = FindCubesInRadius();
            _detonator.Detonate(cubesInRadius);
            Detonated?.Invoke(this);
        }            

        Destroy(gameObject);
    }

    public void Initialize(float scale, float decayProbability)
    {
        _decayProbability = decayProbability * _robabilityMultiplier;
        _visualizer.Modify(scale * _scaleMultiplier);
    }

    public void Detonate(List<Rigidbody> rigidbodies)
    {
        _detonator.Detonate(rigidbodies);
    }

    private List<Rigidbody> FindCubesInRadius()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _detonator.RadiusFromScale);
        List<Rigidbody> cubesRigidbodies = new();

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
                cubesRigidbodies.Add(hit.attachedRigidbody);
        }

        return cubesRigidbodies;
    }
}