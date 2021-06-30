using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 15.0f;
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
        LaserMovement();
        DestroyLaser();
        EndGame();
    }

    void LaserMovement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    void DestroyLaser()
    {
        if(transform.position.y > 7.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
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
