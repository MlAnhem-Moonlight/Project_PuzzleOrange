using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LossScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private bool canClickToReset = false;

    private void OnEnable()
    {
        this.scoreText.text = PlayerPrefs.GetInt("Score", 0).ToString();
        this.highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        StartCoroutine(WaitBeforeAllowClick());
    }

    private IEnumerator WaitBeforeAllowClick()
    {
        yield return new WaitForSeconds(1f);
        canClickToReset = true;
    }

    private void Update()
    {
        if (canClickToReset && Input.GetMouseButtonDown(0))
        {
            ResetButton();
        }
    }

    public void ResetButton()
    {
        AudioManager.Instance.sfxSource.PlayOneShot(AudioManager.Instance.button);
        GameManager.Instance.ResetGame();
        StartCoroutine(UIManager.Instance.LoadScene("GameScene", GameManager.GameState.Game));
    }

    public void HomeScene()
    {
        AudioManager.Instance.sfxSource.PlayOneShot(AudioManager.Instance.button);
        GameManager.Instance.ResetGame();
        StartCoroutine(UIManager.Instance.LoadScene("GameScene", GameManager.GameState.Game));
    }
}
