using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _thrusterSpeed = 7.5f;
    [SerializeField]
    private float _speedBoostSpeed = 10.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _thrusterVisual;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private GameObject _speedBoostVisual;
    [SerializeField]
    private GameObject _damageVisualLeft, _damageVisualRight;
    [SerializeField]
    private GameObject _scoreText;
    [SerializeField]
    private float _fireRate = .15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _shields = 3;
    [SerializeField]
    private int _enemyPoints;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private AudioSource _audioSourceLaser;
    [SerializeField]
    private AudioSource _audioSourcePowerUp;
    [SerializeField]
    private AudioSource _explosionAudioSource;
    
    
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _shieldVisual.SetActive(false);
        _thrusterVisual.SetActive(false);
        _speedBoostVisual.SetActive(false);
        _damageVisualRight.SetActive(false);
        _damageVisualLeft.SetActive(false);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
    }
    
    void Update()
    {
        CalculateMovement();
        InstantiateLaser();
    }

    void CalculateMovement()
    {
        //Movement Input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontalInput, verticalInput, 0);

        if (_isSpeedBoostActive == true)
        {
            transform.Translate(inputDirection * _speedBoostSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(inputDirection * _thrusterSpeed * Time.deltaTime);
            _thrusterVisual.SetActive(true);
        }
        else
        {
            _thrusterVisual.SetActive(false);
            transform.Translate(inputDirection * _speed * Time.deltaTime);
        }


        //Movement Boundries
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 1.5f), 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void InstantiateLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;


            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.79f, 0), Quaternion.identity);
            }

            _audioSourceLaser.Play();
        }
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _shields--;

            if (_shields == 2)
            {
                _shieldVisual.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else if (_shields == 1)
            {
                _shieldVisual.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }

            if (_shields < 1)
            {
                _isShieldActive = false;
                _shieldVisual.SetActive(false);
                _explosionAudioSource.Play();
            }
            return;
        }
        
        _lives--;

        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _damageVisualRight.SetActive(true);
            _explosionAudioSource.Play();
        }
        else if (_lives == 1)
        {
            _damageVisualLeft.SetActive(true);
            _explosionAudioSource.Play();
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            _uiManager.GameOverSequence();
        }
    }

    public void TripleShotActive()
    {
        _audioSourcePowerUp.Play();
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _audioSourcePowerUp.Play();
        _isSpeedBoostActive = true;
        _speedBoostVisual.SetActive(true);
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoostVisual.SetActive(false);
        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        _audioSourcePowerUp.Play();
        _isShieldActive = true;
        _shieldVisual.SetActive(true);
    }
}