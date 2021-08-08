using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefabs;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    [SerializeField]
    private int[] _table = { 30, 20, 15, 10, 10, 10, 5 }; //Ammo, Health, Triple Shot, Speed, Shield, Negative, Laser Beam
    [SerializeField]
    private int _total, _randomNumber;
    [SerializeField]
    private List<GameObject> _powerUps;
    [SerializeField]
    private bool _powerUpSpawn;

    void Start()
    {
        foreach(var item in _table)
        {
            _total += item;
        }
    }

    void Update()
    {
        RestartPowerUpSpawnRoutine();
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            if (_stopSpawning == true)
            {
                yield break;
            }
            else
            {
                int randomEnemy = Random.Range(0, 1);
                Vector3 posToSpawn = new Vector3(Random.Range(-13.4f, 13.4f), 9f, 0f);
                GameObject newEnemy = Instantiate(_enemyPrefabs[randomEnemy], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(5.0f);
            }
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(5.0f, 10.0f));

            _randomNumber = Random.Range(0, _total);

            if (_stopSpawning == true)
            {
                _powerUpSpawn = false;
                yield break;
            }
            else
            {
                for (int i = 0; i < _table.Length; i++)
                {
                    if (_randomNumber <= _table[i])
                    {
                        Vector3 posToSpawn = new Vector3(Random.Range(-9.4f, 9.4f), 8.5f, 0f);
                        Instantiate(_powerUps[i], posToSpawn, Quaternion.identity);

                        _powerUpSpawn = true;

                        yield break;
                    }
                    else
                    {
                        _randomNumber -= _table[i];
                    }
                }
            }
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    private void RestartPowerUpSpawnRoutine()
    {
        if (_powerUpSpawn == true)
        {
            _powerUpSpawn = false;
            StartCoroutine(SpawnPowerUpRoutine());
        }
    }
}
