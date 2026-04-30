using UnityEngine;

public class Destroy : MonoBehaviour
{
    bool hasCollided = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(hasCollided) return;
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null && GameManager.instance.isGameActive)
            {   
                hasCollided = true;
                Debug.Log("You Hit A Human!");
                GameManager.instance.TakeDamage(1);
                Destroy(gameObject);
            }
        }
    }
}
