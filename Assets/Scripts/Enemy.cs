using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private UIManager _uiManager;
    private Animator _enemyAnim;
    [SerializeField]
    private AudioSource _explosionAudioSource;
    [SerializeField]
    private AudioSource _laserAudioSource;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private Player _player;
    [SerializeField]
    private BoxCollider2D _collider1, _collider2;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL!");
        }
        
        _enemyAnim = GetComponent<Animator>();

        if (_enemyAnim == null)
        {
            Debug.LogError("The Enemy Animator is NULL!");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }

        StartCoroutine(InstantiateEnemyLaserRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        EndGame();
    }

    void EnemyStartPosition()
    {
        transform.position = new Vector3(Random.Range(-9.4f, 9.4f), 7.5f, 0f);
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8.5f)
        {
            EnemyStartPosition();
        }
    }

        private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _collider1.enabled = false;
            _collider2.enabled = false;

            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
            
            UpdateScore();
            _enemyAnim.SetTrigger("OnEnemyDeath");
            _speed = 1.0f;
            Destroy(this.gameObject, 2.2f);
        }
        
        if(other.tag == "Laser")
        {
            _collider1.enabled = false;
            _collider2.enabled = false;
            Destroy(other.gameObject);
            _explosionAudioSource.Play();
            UpdateScore();
            _enemyAnim.SetTrigger("OnEnemyDeath");
            _speed = 1.0f;
            Destroy(this.gameObject, 2.2f);
        }
    }

    private void UpdateScore()
    {
        _uiManager.AddPoints();
    }
    IEnumerator InstantiateEnemyLaserRoutine()
    {
        while (_collider1.enabled == true && _collider2.enabled == true)
        {
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, 0.79f, 0), Quaternion.identity);
            _laserAudioSource.Play();
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));

            if (_collider1.enabled == false || _collider2.enabled == false)
            {
                yield break;
            }
        }
    }

    private void EndGame()
    {
        if (_player ==null)
        {
            Destroy(this.gameObject);
        }
    }
}
