using UnityEngine;
using TMPro; // ใช้สำหรับ TextMeshPro
using UnityEngine.SceneManagement; // ใช้สำหรับการโหลด Scene (Restart)

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // ทำเป็น Singleton เพื่อให้สคริปต์อื่นเรียกใช้ง่ายๆ

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject gamePlayPanel;
    public GameObject gameOverPanel;

    [Header("UI Text")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hpText;

    [Header("Game Settings")]
    public int maxHP = 3; // กำหนดว่าชนได้กี่ครั้ง

    private int score = 0;
    private int currentHP;
    public bool isGameActive = false;

    private void Awake()
    {
        // ป้องกันไม่ให้มี GameManager ซ้อนกันหลายตัว
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // เริ่มเกมมา เปิดแค่ Main Menu และหยุดเวลาไว้
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        isGameActive = false;
        mainMenuPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f; // หยุดเกม
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
        Time.timeScale = 1f; // ให้เกมเริ่มเดิน
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

        if (currentHP <= 0)
        {
            GameOver();
        }
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
        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // หยุดเกมตอนตาย
    }

    public void RestartGame()
    {
        // รีโหลด Scene ปัจจุบันใหม่ทั้งหมด
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game!");
        Application.Quit();
    }
}