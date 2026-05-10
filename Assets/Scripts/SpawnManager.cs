using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public Transform[] spawnPoint;

    void Start()
    {
        InvokeRepeating(nameof(RandomSpawn), 1f, 2f);
    }

    void RandomSpawn()
    {
        if (GameManager.instance == null || !GameManager.instance.isGameActive) return;

        int randomPrefabIndex = Random.Range(0, obstaclePrefabs.Length);
        int randomSpawnIndex = Random.Range(0, spawnPoint.Length);
        Instantiate(obstaclePrefabs[randomPrefabIndex], spawnPoint[randomSpawnIndex].position, Quaternion.identity);
    }
}