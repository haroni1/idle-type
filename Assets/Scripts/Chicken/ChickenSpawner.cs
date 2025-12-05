using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    public GameObject chickenPrefab;
    public Transform player;
    public float spawnRadius = 8f;
    public float spawnInterval = 2f;

    private float timer;

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.currentState != GameState.Day) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnChicken();
        }
    }

    private void SpawnChicken()
    {
        if (player == null)
        {
            Debug.LogWarning("ChickenSpawner: player가 설정되지 않음");
            return;
        }

        Vector2 offset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(offset.x, offset.y, 0f);

        Instantiate(chickenPrefab, spawnPos, Quaternion.identity);
    }
}
