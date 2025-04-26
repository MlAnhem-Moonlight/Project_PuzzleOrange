using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting; // Import TextMeshPro namespace

public class CheckWin : MonoBehaviour
{
    public GameObject[] gameObjects;
    public GameObject canvas;
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI timerText; 
    public float finishTime = 0f;

    private float timer = 0f; 
    private bool gameFinished = false; 


    void Start()
    {
        timer = 0f;
        winPanel.SetActive(false); 
        losePanel.SetActive(false);
        Time.timeScale = 1f; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameFinished)
        {
            
            timer += Time.deltaTime;

            
            timerText.text = $"Time: {timer:F2} seconds";

            
            if (timer >= finishTime)
            {
                gameFinished = true;
                Time.timeScale = 0f;
                canvas.SetActive(true);
                losePanel.SetActive(true); 
                return; 
            }


            foreach (var gameObject in gameObjects)
            {
                if (!gameObject.GetComponent<MoveObject>().complete)
                {
                    return; // Nếu chưa hoàn thành, tiếp tục chạy
                }
            }


            gameFinished = true;
            Time.timeScale = 0f;
            canvas.SetActive(true);
            winPanel.SetActive(true);
        }
    }
}
