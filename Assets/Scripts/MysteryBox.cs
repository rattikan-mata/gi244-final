using System.Collections;
using UnityEngine;
using TMPro;

public class MysteryBox : MonoBehaviour
{
    [Header("Speed Effect Settings")]
    public float speedUpMultiplier = 1.5f;
    public float speedDownMultiplier = 0.6f;
    public float effectDuration = 10f;

    [Header("UI")]
    public TextMeshProUGUI countdownText;

    private static Coroutine activeEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ray"))
        {
            if (GameManager.instance != null && GameManager.instance.isGameActive)
            {
                bool isSpeedUp = Random.value > 0.5f;
                float multiplier = isSpeedUp ? speedUpMultiplier : speedDownMultiplier;
                string label = isSpeedUp ? "⚡ Speed UP!" : "🐢 Speed DOWN";

                Debug.Log("Mystery Box! " + label);

                if (activeEffect != null)
                    StopCoroutine(activeEffect);

                activeEffect = StartCoroutine(ApplySpeedEffect(multiplier, label));
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplySpeedEffect(float multiplier, string label)
    {
        float originalScale = Time.timeScale;
        Time.timeScale = originalScale * multiplier;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        float remaining = effectDuration;
        if (countdownText != null)
            countdownText.gameObject.SetActive(true);

        while (remaining > 0f)
        {
            if (countdownText != null)
                countdownText.text = label + "\n" + Mathf.CeilToInt(remaining) + "s";

            yield return new WaitForSecondsRealtime(1f);
            remaining -= 1f;
        }

        Time.timeScale = Time.timeScale / multiplier;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (countdownText != null)
        {
            countdownText.text = "";
            countdownText.gameObject.SetActive(false);
        }

        activeEffect = null;
    }
}