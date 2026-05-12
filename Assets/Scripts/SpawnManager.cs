using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public Transform[] spawnPoint;
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
        Instantiate(obstaclePrefabs[randomPrefabIndex], spawnPoint[randomSpawnIndex].position, obstaclePrefabs[randomPrefabIndex].transform.rotation);
    }
}
