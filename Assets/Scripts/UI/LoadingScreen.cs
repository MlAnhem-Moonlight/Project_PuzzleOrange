using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
// Explicitly alias namespaces for disambiguation
using UI = UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;
    public UI.Slider progressBar; // Use Slider from UnityEngine.UI
    public string sceneName; // Use a string for the scene name
    public UI.Image hint; // Use Image from UnityEngine.UI
    public TMPro.TMP_Text text;

    private bool isLoadingComplete = false; // Track if loading has reached 90%
    private bool isSceneActivationTriggered = false; // To track user input
    private float originalTimeScale = 1f; // To store the original time scale

    private void Start()
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(false); // Hide the loading screen at the start
    }

    public void LoadScene()
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadAsynchronously(scene));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        // Freeze the game by setting time scale to 0 (except the loading process)
        originalTimeScale = Time.timeScale; // Save the current time scale
        Time.timeScale = 0; // Freeze all gameplay elements

        // Reset state variables
        isLoadingComplete = false;
        isSceneActivationTriggered = false;

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Prevent automatic scene activation

        if (loadingScreen != null)
            loadingScreen.SetActive(true); // Show the loading screen

        // First phase: Wait for loading to reach 90%
        while (!isLoadingComplete)
        {
            // Calculate progress manually
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Normalize progress between 0 and 0.9

            if (progressBar != null)
                progressBar.value = progress;

            // Check if loading has reached 90%
            if (operation.progress >= 0.9f)
            {
                text.SetText("Chạm vào màn hình để tiếp tục!");
                isLoadingComplete = true;
            }

            yield return null; // Wait for next frame
        }

        // Second phase: Wait for user touch input
        while (!isSceneActivationTriggered)
        {
            // Check for mouse click (for testing in editor)
            if (Input.GetMouseButtonDown(0))
            {
                isSceneActivationTriggered = true;
            }

            // Check for touch input (for mobile)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                isSceneActivationTriggered = true;
            }

            yield return null; // Wait for next frame
        }

        // User has touched the screen, activate the scene
        operation.allowSceneActivation = true;

        // Wait for the scene to fully load
        while (!operation.isDone)
        {
            yield return null;
        }

        // Restore the original time scale and hide the loading screen
        Time.timeScale = originalTimeScale; // Unfreeze gameplay
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }
}
