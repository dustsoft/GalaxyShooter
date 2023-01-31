using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region VARIABLES
    [Header("Score Info")]
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _highScoreText;

    [Header("Wave System Info")]
    [SerializeField] GameObject _waveDisplay;
    [SerializeField] TextMeshProUGUI _waveText;
    public bool _waveOneStarted = false;
    public bool _waveTwoStarted = false;

    [Header("Game UI Info")]
    [SerializeField] TextMeshProUGUI _ammoText;
    [SerializeField] Image _livesImage;
    [SerializeField] Sprite[] _livesSprites;
    [SerializeField] Image _shieldsImage;
    [SerializeField] Sprite[] _shieldsSprite;
    [SerializeField] GameObject _fillBar;

    [Header("Misc UI Info")]
    [SerializeField] GameObject _gamePauseMenu;
    [SerializeField] TextMeshProUGUI _finalWarningText;
    [SerializeField] TextMeshProUGUI _gameOverText;

    GameManager _gameManager;
    SpawnManager _spawnManager;
    bool _finalChance = false;
    #endregion

    #region METHODS/FUNCTIONS
    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GAME MANAGER IS NULL");
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("SPAWN MANAGER IS NULL!");
        }
    }

    private void Update()
    {
        WaveSystemUI();
    }

    public void ChangeFillBarColorRed()
    {
        _fillBar.GetComponent<Image>().color = new Color32(255, 0, 41, 255);
    }

    public void ChangeFillBarColorBlue()
    {
        _fillBar.GetComponent<Image>().color = new Color32(134, 223, 255, 255);
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

    public void WaveSystemUI()
    {
        StartCoroutine(WaveRoutine());
    }

    public void FinalWarningSequnce()
    {
        _finalChance = true;
        StartCoroutine(FinalChanceFlicker());
    }
    #endregion

    #region COROUTINES

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

    IEnumerator WaveRoutine()
    {
        if (_spawnManager.waveNumber == 1 && _waveOneStarted == false)
        {
            _waveText.text = "WAVE 1";
            _waveDisplay.SetActive(true);
            yield return new WaitForSeconds(3.33f);
            _waveDisplay.SetActive(false);
            _waveOneStarted = true;
            StopCoroutine(WaveRoutine());
        }

        if (_spawnManager.waveNumber == 2 && _waveTwoStarted == false)
        {
            _waveText.text = "WAVE 2";
            _waveDisplay.SetActive(true);
            yield return new WaitForSeconds(3.33f);
            _waveDisplay.SetActive(false);
            _waveTwoStarted = true;
            StopCoroutine(WaveRoutine());
        }
    }
    #endregion
}
