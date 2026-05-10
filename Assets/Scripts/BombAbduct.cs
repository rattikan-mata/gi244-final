using UnityEngine;

public class BombAbduct : MonoBehaviour
{
    public int scorePenalty = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ray"))
        {
            if (GameManager.instance != null && GameManager.instance.isGameActive)
            {
                Debug.Log("Bomb absorbed! Score -" + scorePenalty);
                GameManager.instance.AddScore(-scorePenalty);
            }
            Destroy(gameObject);
        }
    }
}