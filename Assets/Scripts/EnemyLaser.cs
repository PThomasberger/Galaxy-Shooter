using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6.0f;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }
    }

    // Update is called once per frame
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
        if (transform.position.y < -6.0f)
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
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
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
