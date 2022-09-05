using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] Image _livesImage;
    [SerializeField] Sprite[] _livesSprites;

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives == -1)
        {
            _livesImage.sprite = _livesSprites[0];
        }
        else
        {
            _livesImage.sprite = _livesSprites[currentLives];
        }
    }
}
