using System.Collections.Generic;
using UnityEngine;

public class ParticleFirePool : MonoBehaviour
{
    [SerializeField] private ParticleSystem _prefab;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private float _defaultScale = 1f;

    private readonly List<ParticleSystem> _pool = new();
    private readonly List<Cube> _cubes = new();

    private void OnEnable()
    {
        _spawner.CubeCreated += AddCube;

        foreach (Cube cube in _cubes)
        {
            Subscribe(cube);
        }
    }

    private void OnDisable()
    {
        _spawner.CubeCreated -= AddCube;

        foreach (Cube cube in _cubes)
        {
            Unsubcribe(cube);
        }
    }

    private void Subscribe(Cube cube)
    {        
        cube.Detonated += ShowFire;
        cube.Separating += RemoveCube;
    }

    private void Unsubcribe(Cube cube)
    {
        cube.Detonated -= ShowFire;
        cube.Separating -= RemoveCube;
    }

    private void AddCube(Cube cube)
    {
        _cubes.Add(cube);
        Subscribe(cube);
    }

    private void RemoveCube(Cube cube)
    {
        _cubes.Remove(cube);
        Unsubcribe(cube);
    }

    private void ShowFire(Cube cube)
    {
        ParticleSystem particle = GetFire();

        Vector3 position = cube.transform.position;
        particle.transform.position = position;

        float scale = _defaultScale / cube.transform.localScale.x;
        particle.transform.localScale = Vector3.one * scale;

        particle.Play();

        RemoveCube(cube);
    }    

    private ParticleSystem GetFire()
    {
        foreach (ParticleSystem fire in _pool)
        {
            if (fire.isPlaying == false)
                return fire;
        }

        ParticleSystem fireInstance = Instantiate(_prefab, transform);
        _pool.Add(fireInstance);

        return fireInstance;
    }
}