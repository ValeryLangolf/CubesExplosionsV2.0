using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int _minimumObjects = 2;
    [SerializeField] private int _maximumObjects = 6;

    private List<Cube> _cubes;

    public event Action<Cube> CubeCreated;

    private void Awake()
    {
        _cubes = GetComponentsInChildren<Cube>().ToList();
    }

    private void OnEnable()
    {
        foreach (Cube cube in _cubes)
        {
            Subscribe(cube);
        }
    }

    private void OnDisable()
    {
        foreach (Cube cube in _cubes)
        {
            Unsubscribe(cube);
        }
    }

    private void Subscribe(Cube cube)
    {
        cube.Separating += CreateCubes;
        cube.Detonated += RemoveCube;
    }

    private void Unsubscribe(Cube cube)
    {
        cube.Separating -= CreateCubes;
        cube.Detonated -= RemoveCube;
    }

    private void CreateCubes(Cube cubeSource)
    {
        float decayProbability = cubeSource.DecayProbability;
        float scale = cubeSource.transform.localScale.x;

        int countNewObjects = UnityEngine.Random.Range(_minimumObjects, _maximumObjects + 1);

        List<Rigidbody> rigidbodies = new();

        for (int i = 0; i < countNewObjects; i++)
        {
            Cube newCube = Instantiate(cubeSource, cubeSource.transform.position, Quaternion.identity, transform);

            if (newCube.TryGetComponent(out Rigidbody rigidbody))
                rigidbodies.Add(rigidbody);

            newCube.Initialize(scale, decayProbability);

            AddCube(newCube);
        }

        cubeSource.Detonate(rigidbodies);
        RemoveCube(cubeSource);
    }

    private void AddCube(Cube cube)
    {
        _cubes.Add(cube);
        Subscribe(cube);

        CubeCreated?.Invoke(cube);
    }

    private void RemoveCube(Cube cube)
    {
        Unsubscribe(cube);
        _cubes?.Remove(cube);
    }
}