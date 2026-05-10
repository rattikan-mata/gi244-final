using UnityEngine;

public class Ray : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance == null || !GameManager.instance.isGameActive) return;

        if (other.CompareTag("Human"))
        {
            Debug.Log("You Got A Human!");
            GameManager.instance.AddScore(100);
            Destroy(other.gameObject);
        }
    }
}