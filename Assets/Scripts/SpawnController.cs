using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float firstSpawnDelay = 0f;

    private Vector3 spawnPoint = Vector3.zero;
    private GameObject spawnGO = null;

    private void Start()
    {
        InvokeRepeating("SpawnObject", firstSpawnDelay, spawnRate);
        Player.OnPlayerDied += StopSpawning;
    }

    private void SpawnObject()
    {
        spawnGO = Pools.EnemyPool.Instance.GetRandomEnemy();

        if (spawnGO != null)
        {
            spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0F, 1F), 1F, transform.position.z));

            spawnGO.GetComponent<Rigidbody>().velocity = Vector3.zero;
            spawnGO.transform.position = spawnPoint;
            spawnGO.transform.rotation = Quaternion.identity;
            spawnGO.SetActive(true);
        }
    }

    private void StopSpawning()
    {
        Debug.Log("Campi, ya no spawneo más.");
        CancelInvoke();
    }

    private void OnDestroy()
    {
        Player.OnPlayerDied -= StopSpawning;
    }
}