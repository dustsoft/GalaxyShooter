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

    [SerializeField] TextMeshProUGUI _ammoText;

    [SerializeField] Image _livesImage;
    [SerializeField] Sprite[] _livesSprites;
    [SerializeField] Image _shieldsImage;
    [SerializeField] Sprite[] _shieldsSprite;


    [SerializeField] GameObject _gamePauseMenu;
 
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

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = ammo.ToString();
    }

    public void PauseMenu()
    {
        _gamePauseMenu.SetActive(true);
    }

    public void UnPauseMenu()
    {
        _gamePauseMenu.SetActive(false);
    }

    public void UpdateLives(int currentLives)
    {

        if (currentLives == -1)
        {
            _livesImage.sprite = _livesSprites[0];
            GameOverSequence();
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

    public void UpdateShieldsUI(int currentShieldHP)
    {
        _shieldsImage.sprite = _shieldsSprite[currentShieldHP];
    }

    public void GameOverSequence()
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
