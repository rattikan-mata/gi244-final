using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    public float timeIncreaseRate = 0.02f;
    public float maxBaseTimeScale = 10.0f;

    void Update()
    {
        if (GameManager.instance == null || !GameManager.instance.isGameActive) return;
        
        if (GameManager.instance.baseTimeScale < maxBaseTimeScale)
        {
            float amountToAdd = timeIncreaseRate * Time.unscaledDeltaTime;
            GameManager.instance.AddAutoSpeed(amountToAdd);
        }
    }

    public void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    void OnDestroy()
    {
        ResetTime();
    }
}