using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuScene : MonoBehaviour
{
    public SceneAsset gameScene;
    public void PlayButton()
    {
        AudioManager.Instance.sfxSource.PlayOneShot(AudioManager.Instance.button);
        GameManager.Instance.ResetGame();

        // Assuming gameScene has a property or method to get the scene name
        string sceneName = gameScene.name; // Replace 'name' with the correct property/method if different
        StartCoroutine(UIManager.Instance.LoadScene(sceneName, GameManager.GameState.Game));
    }
}
