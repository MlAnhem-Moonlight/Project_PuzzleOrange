using Hyb.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManualSingletonMono<GameManager>
{
    public enum GameState {Menu, Game, Loss, Pause ,Loading}
    public GameState State;

    public float timeUpLevel = 30f;
    public float hard = 0;

    private void Update()
    {
        this.OnStateChanged();
        if(State == GameState.Game)
        {
            this.timeUpLevel -= Time.deltaTime;
            if (timeUpLevel > 0) return;
            timeUpLevel = 30f;
            hard += 0.5f;
        }    
    }

    private void OnStateChanged()
    {
        switch (State)
        {
            case GameState.Menu:
                UIManager.Instance.MenuScene();
                break;
            case GameState.Game:
                UIManager.Instance.GameScene();
                break;
            case GameState.Loss:
                UIManager.Instance.LossScene();
                break;
            case GameState.Pause:
                UIManager.Instance.PauseScene();
                break;
        }
    }

    public void ResetGame()
    {
        Time.timeScale = 1;
        timeUpLevel = 60f;
        hard = 0;
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.Save();
    }

    public void Score(int score)
    {
        int i = PlayerPrefs.GetInt("Score", 0);
        i+=score;
        PlayerPrefs.SetInt("Score", i);
        PlayerPrefs.Save();
    }

    public void HighScore()
    {
        if (PlayerPrefs.GetInt("Score", 0) < PlayerPrefs.GetInt("HighScore", 0)) return;
        PlayerPrefs.SetInt("HighScore", PlayerPrefs.GetInt("Score", 0));
        PlayerPrefs.Save();
    }

    public void EndGame(bool isWin)
    {
        StartCoroutine(EndLevel(isWin));
    }

    public IEnumerator EndLevel(bool isWin)
    {
        yield return new WaitForSeconds(0.5f);
        if (this.State != GameState.Game) yield break;
        if (isWin)
        {
            yield break;
        }
        else
        {
            this.State = GameState.Loss;
        }
    }
}
