using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private UIManager _uiManager;


    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
            UpdateScore();

            Destroy(this.gameObject);
        }
        
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            //Add 10 to score
            UpdateScore();

            Destroy(this.gameObject);
        }
    }

    private void UpdateScore()
    {
        _uiManager.AddPoints();
    }
}
