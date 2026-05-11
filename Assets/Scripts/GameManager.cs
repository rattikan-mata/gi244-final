using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject gamePlayPanel;
    public GameObject gameOverPanel;

    [Header("UI Text - HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI countdownText;

    [Header("UI Text - Game Over")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;

    [Header("Game Settings")]
    public int maxHP = 3;

    private int score = 0;
    private int currentHP;
    public bool isGameActive = false;

    private Coroutine speedEffectCoroutine;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (countdownText != null) countdownText.gameObject.SetActive(false);
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        isGameActive = false;
        mainMenuPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        isGameActive = true;
        score = 0;
        currentHP = maxHP;
        UpdateScoreText();
        UpdateHPText();
        mainMenuPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
        Time.timeScale = 1f;
    }

    public void AddScore(int amount)
    {
        if (!isGameActive) return;
        score += amount;
        UpdateScoreText();
    }

    public void TakeDamage(int damage)
    {
        if (!isGameActive) return;
        currentHP -= damage;
        UpdateHPText();
        if (currentHP <= 0) GameOver();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    private void UpdateHPText()
    {
        if (hpText != null) hpText.text = "HP: " + currentHP;
    }

    public void GameOver()
    {
        isGameActive = false;

        if (speedEffectCoroutine != null)
        {
            StopCoroutine(speedEffectCoroutine);
            speedEffectCoroutine = null;
        }
        if (countdownText != null) countdownText.gameObject.SetActive(false);

        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        bool isNewRecord = score > savedHighScore;
        if (isNewRecord)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            savedHighScore = score;
        }

        if (finalScoreText != null) finalScoreText.text = "Score: " + score;
        if (highScoreText != null) highScoreText.text = "Best: " + savedHighScore;

        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game!");
        Application.Quit();
    }

    public void ApplySpeedEffect(float multiplier, float duration, string label)
    {
        if (speedEffectCoroutine != null) StopCoroutine(speedEffectCoroutine);
        speedEffectCoroutine = StartCoroutine(SpeedEffectRoutine(multiplier, duration, label));
    }

    private IEnumerator SpeedEffectRoutine(float multiplier, float duration, string label)
    {
        float originalScale = Time.timeScale;
        Time.timeScale = originalScale * multiplier;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (countdownText != null) countdownText.gameObject.SetActive(true);

        float remaining = duration;
        while (remaining > 0f)
        {
            if (countdownText != null)
                countdownText.text = label + "\n" + Mathf.CeilToInt(remaining) + " s";
            yield return new WaitForSecondsRealtime(1f);
            remaining -= 1f;
        }

        Time.timeScale = originalScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (countdownText != null)
        {
            countdownText.text = "";
            countdownText.gameObject.SetActive(false);
        }
        speedEffectCoroutine = null;
    }
}