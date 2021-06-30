using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps;
    private float _ultraPowerUpDelay = 60f;
    [SerializeField]
    private bool _isUltraPowerUpReady = false;

    private bool _stopSpawning = false;

    private void Update()
    {
        UltraPowerUpActive();
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
            Vector3 posToSpawn = new Vector3(Random.Range(-9.4f, 9.4f), 7.5f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);

            if (_stopSpawning == true)
            {
                yield break;
            }
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));

            if (_stopSpawning == true)
            {
                yield break;
            }

            Vector3 posToSpawn = new Vector3(Random.Range(-9.4f, 9.4f), 7.5f, 0f);
            int randomPowerUp = Random.Range(0, 5);


            if (_isUltraPowerUpReady == true)
            {
                Instantiate(_powerUps[5], posToSpawn, Quaternion.identity);
                _isUltraPowerUpReady = false;
                _ultraPowerUpDelay = Time.time + 60f;
            }
            else
            {
                Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }

    private void UltraPowerUpActive()
    {
        if (Time.time > _ultraPowerUpDelay)
        {
            _isUltraPowerUpReady = true;
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
