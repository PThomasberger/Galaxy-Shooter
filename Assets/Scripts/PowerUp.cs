using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _powerUpSpeed = 3.0f;
    [SerializeField] 
    private int _powerUpID; //0 = Triple Shot; 1 = Speed; 2 = Shields; 3 = Ammo Refill; 4 = Health; 5 = Laser Beam;
    private Player _player;
    [SerializeField]
    public float _collectSpeed = 7.0f;

    [SerializeField]
    private GameObject _explosionPrefab;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }
    }

    void Update()
    {
        PowerUpMovement();
        EndGame();
    }

    private void PowerUpMovement()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.C))
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _collectSpeed * Time.deltaTime);
        }

        if (transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                switch (_powerUpID)
                {
                    case 0:
                        _player.TripleShotActive();
                        break;
                    case 1:
                        _player.SpeedBoostActive();
                        break;
                    case 2:
                        _player.ShieldActive();
                        break;
                    case 3:
                        _player.AmmoRefillActive();
                        break;
                    case 4:
                        _player.HealthPickupActive();
                        break;
                    case 5:
                        _player.LaserBeamActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }

            Destroy(this.gameObject);
        }

        if (other.tag == "EnemyLaser" || other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _explosionPrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            BoxCollider2D _boxCollider = GetComponent<BoxCollider2D>();
            _boxCollider.enabled = false;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.15f);
        }
        

    }
    private void EndGame()
    {
        if (_player == null)
        {
            Destroy(this.gameObject);
        }
    }
}
