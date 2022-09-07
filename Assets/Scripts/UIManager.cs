using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _highScoreText;
    [SerializeField] TextMeshProUGUI _finalWarningText;
    [SerializeField] TextMeshProUGUI _gameOverText;
    [SerializeField] Image _livesImage;
    [SerializeField] Sprite[] _livesSprites;
    GameManager _gameManager;
    bool _finalChance = false;

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GAME MANAGER IS NULL");
        }
    }

    public void UpdateScore(int playerScore, int highScore)
    {
        _scoreText.text = playerScore.ToString();
        _highScoreText.text = highScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {

        if (currentLives == -1)
        {
            _livesImage.sprite = _livesSprites[0];
            GameOverSequnce();
        }
        else
        {
            _livesImage.sprite = _livesSprites[currentLives];
        }

        if (currentLives == 0)
        {
            FinalWarningSequnce();
        }
    }

    public void GameOverSequnce()
    {
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverTextFlicker());
    }

    public void FinalWarningSequnce()
    {
        _finalChance = true;
        StartCoroutine(FinalChanceFlicker());
    }

    IEnumerator FinalChanceFlicker()
    {
        if (_finalChance == true)
        {
            _finalWarningText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            _finalWarningText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            _finalWarningText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            _finalWarningText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            _finalWarningText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            _finalWarningText.gameObject.SetActive(false);

            _finalChance = false;
        }
    }

    IEnumerator GameOverTextFlicker()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
