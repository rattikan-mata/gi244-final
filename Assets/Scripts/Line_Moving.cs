using UnityEngine;

public class Line_Moving : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;

    [Header("Position Settings")]
    public float endZ = -52f;
    public float startZ = 52f;

    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        if (transform.position.z <= endZ)
        {
            Vector3 resetPosition = transform.position;
            resetPosition.z = startZ;
            transform.position = resetPosition;
        }
    }
}