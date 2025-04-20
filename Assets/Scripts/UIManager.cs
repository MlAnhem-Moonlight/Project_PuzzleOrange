using Hyb.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : ManualSingletonMono<UIManager>
{
    //Transition
    [SerializeField] private GameObject transition;

    //Menu
    [SerializeField] private GameObject menu;

    //Game
    [SerializeField] private GameObject game;
    [SerializeField] private GameObject loss;
    [SerializeField] private GameObject pause;

    public void MenuScene()
    {
        this.menu.SetActive(true);
        this.game.SetActive(false);
        this.loss.SetActive(false);
        this.pause.SetActive(false);
    }

    public void GameScene()
    {
        this.menu.SetActive(false);
        this.game.SetActive(true);
        this.loss.SetActive(false);
        this.pause.SetActive(false);
    }

    public void LossScene()
    {
        this.loss.SetActive(true);
    }

    public void PauseScene()
    {
        this.pause.SetActive(true);
    }

    public IEnumerator OpenLoadScene()
    {
        GameManager.Instance.State = GameManager.GameState.Loading;
        this.transition.GetComponent<Animator>().Play("EndTransition");
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator CloseLoadScene(GameManager.GameState state)
    {
        GameManager.Instance.State = state;
        this.transition.GetComponent<Animator>().Play("StartTransition");
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator WaitLoadScene(string sceneName, GameManager.GameState state)
    {
        yield return StartCoroutine(OpenLoadScene());
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
        yield return StartCoroutine(CloseLoadScene(state));
    }

    public IEnumerator LoadScene(string sceneName, GameManager.GameState state)
    {
        yield return StartCoroutine(WaitLoadScene(sceneName, state));
    }
}
