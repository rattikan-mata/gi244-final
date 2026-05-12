using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    [Header("Speed Effect Settings")]
    public float speedUpMultiplier = 1.5f;
    public float speedDownMultiplier = 0.6f;
    public float effectDuration = 10f;

    [Header("Audio Settings")]
    public AudioClip boxSfx;
    private AudioSource playerAudio;

    void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ray"))
        {
            if (GameManager.instance != null && GameManager.instance.isGameActive)
            {
                bool isSpeedUp = Random.value > 0.5f;
                float multiplier = isSpeedUp ? speedUpMultiplier : speedDownMultiplier;
                string label = isSpeedUp ? "Speed UP!" : "SLOW";

                playerAudio.PlayOneShot(boxSfx);

                GameManager.instance.ApplySpeedEffect(multiplier, effectDuration, label);
            }
            Destroy(gameObject);
        }
    }
}