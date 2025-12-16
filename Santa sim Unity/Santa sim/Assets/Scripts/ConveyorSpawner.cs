using UnityEngine;
using System.Collections;

public class ConveyorSpawner : MonoBehaviour
{
    public Transform spawnPoint;                  // where presents appear
    public GameObject[] presentPrefabs;           // prefabs to spawn (set in inspector)
    public float spawnInterval = 2f;              // seconds between spawns
    public Transform spawnParent;                 // parent for spawned objects (optional)
    private bool spawning = true;


    private void Start()
    {
        if (spawnPoint == null) spawnPoint = transform;
        StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
{
    spawning = false;
}




IEnumerator SpawnLoop()
{
    while (spawning)
    {
        if (GameManager.Instance.IsGameActive())
            SpawnRandomPresent();

        yield return new WaitForSeconds(spawnInterval);
    }
}


    public void SpawnRandomPresent()
    {
        if (presentPrefabs == null || presentPrefabs.Length == 0) return;

        GameObject prefab = presentPrefabs[Random.Range(0, presentPrefabs.Length)];
        GameObject go = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, spawnParent);
        // Optional: randomize slight position/rotation
    }
}
