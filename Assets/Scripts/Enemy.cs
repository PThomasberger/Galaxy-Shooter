using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private UIManager _uiManager;

    private Animator _enemyAnim;
    
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

            _enemyAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            Destroy(this.gameObject, 2.2f);
        }
        
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            
            UpdateScore();

            _enemyAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            Destroy(this.gameObject, 2.2f);
        }
    }

    private void UpdateScore()
    {
        _uiManager.AddPoints();
    }
}
