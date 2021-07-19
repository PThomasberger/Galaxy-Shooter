using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefabs;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private float _laserBeamPowerUpDelay = 60f;
    [SerializeField]
    private bool _isLaserBeamPowerUpReady = false;
    private bool _stopSpawning = false;

    void Start()
    {
        
    }

    void Update()
    {
        LaserBeamPowerUpActive();
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
            int randomEnemy = Random.Range(0, 1);
            Vector3 posToSpawn = new Vector3(Random.Range(-13.4f, 13.4f), 9f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefabs[randomEnemy], posToSpawn, Quaternion.identity);
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

            Vector3 posToSpawn = new Vector3(Random.Range(-9.4f, 9.4f), 8.5f, 0f);
            int randomPowerUp = Random.Range(0, 6);
            
            if (_isLaserBeamPowerUpReady == true)
            {
                Instantiate(_powerUps[6], posToSpawn, Quaternion.identity);
                _isLaserBeamPowerUpReady = false;
                _laserBeamPowerUpDelay = Time.time + 60f;
            }
            else
            {
                Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }

    private void LaserBeamPowerUpActive()
    {
        if (Time.time > _laserBeamPowerUpDelay)
        {
            _isLaserBeamPowerUpReady = true;
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
