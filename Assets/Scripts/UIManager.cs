using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    private int _score = 0;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private AudioSource _audioSource;

    private GameManager _gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _scoreText.text = "Score: " + _score;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPoints()
    {
        _score += 10;
        _scoreText.text = "Score: " + _score;
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives >= 0)
        {
            _livesImage.sprite = _livesSprites[currentLives];
        }
        else
        {
            _livesImage.sprite = _livesSprites[0];
        }
    }

    public void GameOverSequence()
    {
        _audioSource.Play();
        StartCoroutine(GameOverTextFlash());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    IEnumerator GameOverTextFlash()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    
}
