using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    public float timeIncreaseRate = 0.02f;
    public float maxTimeScale = 2.0f;

    void Update()
    {
        
        if (Time.timeScale < maxTimeScale)
        {
            Time.timeScale += timeIncreaseRate * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

    public void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    // เผื่อเผลอเปลี่ยนฉากแล้วลืมรีเซ็ตเวลา
    void OnDestroy()
    {
        ResetTime();
    }
}