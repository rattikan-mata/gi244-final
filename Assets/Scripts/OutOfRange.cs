using UnityEngine;

public class OutOfRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human") ||
            other.CompareTag("Obstacle") ||
            other.CompareTag("Bomb") ||
            other.CompareTag("MysteryBox"))
        {
            Destroy(other.gameObject);
        }
    }
}