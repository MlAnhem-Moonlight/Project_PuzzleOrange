using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highText;

    private void Update()
    {
        this.scoreText.text = "Distance:" + PlayerPrefs.GetInt("Score", 0).ToString();
        this.highText.text = "Best:" + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    public void PauseButton()
    {
        Time.timeScale = 0f;
        AudioManager.Instance.sfxSource.PlayOneShot(AudioManager.Instance.button);
        GameManager.Instance.State = GameManager.GameState.Pause;
    }
}
