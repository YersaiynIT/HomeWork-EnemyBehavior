using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private List<SpawnPoint> _spawnPoint;

    [SerializeField] private List<Transform> _patrulPoints;
    [SerializeField] private Transform _heroTarget;

    private void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        foreach (SpawnPoint spawnPoint in _spawnPoint)
        {
            IdleBehaviorType idleBehavior = spawnPoint.IdleBehavior;
            AggroBehaviorType aggroBehavior = spawnPoint.AggroBehavior;

            Enemy enemy = Instantiate(_enemyPrefab, spawnPoint.Positon, Quaternion.identity);

            enemy.Initialize(idleBehavior, aggroBehavior, _patrulPoints, _heroTarget);

            enemy.InitializePatrulPoints();
        }
    }
}
