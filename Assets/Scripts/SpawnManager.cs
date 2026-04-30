using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Array ของ obstacle prefab ที่จะสุ่มเกิด
    public Transform[] spawnPoint; // จุดที่ใช้เป็นตำแหน่งเกิดของ obstacle
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(RandomSpawn), 1f, 2f);
    }

    // Update is called once per frame
    void RandomSpawn()
    {
        int randomPrefabIndex = Random.Range(0, obstaclePrefabs.Length);
        int randomSpawnIndex = Random.Range(0, spawnPoint.Length);
        Instantiate(obstaclePrefabs[randomPrefabIndex], spawnPoint[randomSpawnIndex].position, Quaternion.identity);
    }
}
