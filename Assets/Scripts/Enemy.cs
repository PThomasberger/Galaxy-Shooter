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
    private AudioSource _enemyLaserAudioSource;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private Player _player;
    [SerializeField]
    private BoxCollider2D _collider1, _collider2;
    private float _enemyStartPosition;
    private bool _canFire = false;
    private RaycastHit2D _laserDetectionLeft;
    private RaycastHit2D _laserDetectionRight;
    private float _raycastLength = 0.5f;
    [SerializeField]
    private GameObject _shieldVisual;

    void Start()
    {
        _enemyStartPosition = transform.position.x;

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

    void Update()
    {
        EnemyMovement();
        EndGame();
        CanFireLaser();
    }

    void EnemyStartPosition()
    {
        transform.position = new Vector3(Random.Range(-11.4f, 11.4f), 7.5f, 0f);

        _enemyStartPosition = transform.position.x;
    }

    void EnemyMovement()
    {
        if (_enemyStartPosition <= -9.5f)
        {
            EnemyMovementRight();
        }
        else if (_enemyStartPosition >= 9.5f)
        {
            EnemyMovementLeft();
        }
        else
        {
            EnemyMovementDown();
            LaserDetectionRaycast();
        }
    }

    void EnemyMovementDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8.5f)
        {
            EnemyStartPosition();
        }
    }

    void EnemyMovementRight()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        transform.Translate(Vector3.right * _speed * Time.deltaTime);

        if (transform.position.x > 11.2f || transform.position.y < -8.5f)
        {
            EnemyStartPosition();
        }
    }

    void EnemyMovementLeft()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        transform.Translate(Vector3.left * _speed * Time.deltaTime);

        if (transform.position.x < -11.2f || transform.position.y < -8.5f)
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
            _player.Damage();
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

        if (other.tag == "LaserBeam")
        {
            _collider1.enabled = false;
            _collider2.enabled = false;
            _explosionAudioSource.Play();
            UpdateScore();
            _enemyAnim.SetTrigger("OnEnemyDeath");
            _speed = 1.0f;
            Destroy(this.gameObject, 2.2f);
        }

        if (other.tag == "Shield")
        {
            _collider1.enabled = false;
            _collider2.enabled = false;
            _player.Damage();
            UpdateScore();
            _enemyAnim.SetTrigger("OnEnemyDeath");
            _explosionAudioSource.Play();
            _speed = 1.0f;
            Destroy(this.gameObject, 2.2f);
        }
    }

    private void UpdateScore()
    {
        _uiManager.AddPoints();
    }

    private void CanFireLaser()
    {
        if (transform.position.x > -10f && transform.position.x < 10f && transform.position.y < 6.5f)
        {
            _canFire = true;
        }
    }

    IEnumerator InstantiateEnemyLaserRoutine()
    {
        while (_collider1.enabled == true && _collider2.enabled == true)
        {
            yield return new WaitUntil(() => _canFire == true);

            Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            _enemyLaserAudioSource.Play();
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));

            if (_collider1.enabled == false || _collider2.enabled == false)
            {
                yield break;
            }
        }
    }

    private void EndGame()
    {
        if (_player == null)
        {
            Destroy(this.gameObject);
        }
    }

    private void LaserDetectionRaycast()
    {
        int _layerMask = 1 << 6;

        _laserDetectionLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 2f), Vector2.left, _raycastLength, _layerMask);

        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 2f), Vector2.left * _raycastLength, Color.red);

        if (_laserDetectionLeft.collider != null)
        {
            //Debug.Log(_laserDetectionLeft.collider);
            StartCoroutine(DodgeRight());
        }

        _laserDetectionRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 2f), Vector2.right, _raycastLength, _layerMask);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 2f), Vector2.right * _raycastLength, Color.red);

        if (_laserDetectionRight.collider != null)
        {
            //Debug.Log(_laserDetectionRight.collider);
            StartCoroutine(DodgeLeft());
        }
    }

    IEnumerator DodgeLeft()
    {
        Vector2 _currentPos = transform.position;
        Vector2 _destination = new Vector2(transform.position.x - 1, transform.position.y);
        float _t = 0f;

        while (_t < 1 && _collider1.enabled == true && _collider2.enabled == true)
        {
            _t += Time.deltaTime / 0.1f;
            transform.position = Vector2.Lerp(_currentPos, _destination, _t);
            yield return null;
        }

        if (_collider1.enabled == false || _collider2.enabled == false)
        {
            yield break;
        }
    }

    IEnumerator DodgeRight()
    {
        Vector2 _currentPos = transform.position;
        Vector2 _destination = new Vector2(transform.position.x + 1, transform.position.y);
        float _t = 0f;

        while (_t < 1 && _collider1.enabled == true && _collider2.enabled == true)
        {
            _t += Time.deltaTime / 0.1f;
            transform.position = Vector2.Lerp(_currentPos, _destination, _t);
            yield return null;
        }

        if (_collider1.enabled == false || _collider2.enabled == false)
        {
            yield break;
        }
    }
}
