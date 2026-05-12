using UnityEngine;

public class Ray : MonoBehaviour
{
    public int humanScore = 100;
    public int bombPenalty = -300;
    public Transform rayOrigin;
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance == null || !GameManager.instance.isGameActive) return;

        if (other.CompareTag("Human"))
        {
            Debug.Log("You Got A Human!");
            GameManager.instance.AddScore(humanScore);
            Absorbable victim = other.GetComponent<Absorbable>();
            if (victim != null)
            {
                victim.StartAbsorbing(rayOrigin, 0.5f);
            }
        }

        if (other.CompareTag("Bomb"))
        {
            Debug.Log("You Got A Bomb!");
            GameManager.instance.AddScore(bombPenalty);
            Destroy(other.gameObject);
        }
    }
}