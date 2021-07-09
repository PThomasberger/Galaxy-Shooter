using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6.0f;
    private Player _player;

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
        EnemyLaserMovement();
        DestroyLaser();
        EndGame();
    }
    void EnemyLaserMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    void DestroyLaser()
    {
        if (transform.position.y < -10.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.Damage();
            Destroy(this.gameObject);
        }

        if (other.tag == "Shield")
        {
            _player.Damage();
            Destroy(this.gameObject, 2.2f);
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
