using UnityEngine;

public class MoveBack : MonoBehaviour
{
    public float moveSpeed = 5f; // ปรับความเร็วในการเลื่อนได้ตรงนี้
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
    }
}
