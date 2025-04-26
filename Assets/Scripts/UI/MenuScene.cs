using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    public string gameSceneName; // Store the scene name as a string

    public void PlayButton()
    {
        AudioManager.Instance.sfxSource.PlayOneShot(AudioManager.Instance.button);
        GameManager.Instance.ResetGame();

        // Use the scene name directly
        StartCoroutine(UIManager.Instance.LoadScene(gameSceneName, GameManager.GameState.Game));
    }

    public void HomeButton()
    {
        SceneManager.LoadScene(gameSceneName); // Use the scene name directly
    }

    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
