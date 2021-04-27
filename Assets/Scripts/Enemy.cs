using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        EnemyStartPosition();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    void EnemyStartPosition()
    {
        transform.position = new Vector3(Random.Range(-9.4f, 9.4f), 7.5f, 0f);
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            EnemyStartPosition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if other is player
        //damage Player
        //destroy Enemy

        if(other.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
