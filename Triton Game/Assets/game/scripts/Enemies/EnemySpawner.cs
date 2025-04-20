using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject normalEnemyPrefab;
    public GameObject electricEnemyPrefab;
    public GameObject corruptEnemyPrefab;

    [Header("Cantidad total por tipo")]
    public int normalEnemyCount = 3;
    public int electricEnemyCount = 2;
    public int corruptEnemyCount = 1;

    [Header("Zona de aparición")]
    public float spawnRadius = 10f;
    public float verticalRange = 2f; // Altura máxima hacia arriba
    public Transform centerPoint;

    [Header("Intervalo entre enemigos")]
    public float spawnInterval = 2f;

    private List<GameObject> spawnQueue = new List<GameObject>();

    void Start()
    {
        // Llenar la cola de enemigos según cantidades
        for (int i = 0; i < normalEnemyCount; i++) spawnQueue.Add(normalEnemyPrefab);
        for (int i = 0; i < electricEnemyCount; i++) spawnQueue.Add(electricEnemyPrefab);
        for (int i = 0; i < corruptEnemyCount; i++) spawnQueue.Add(corruptEnemyPrefab);

        StartCoroutine(SpawnEnemiesOneByOne());
    }

    IEnumerator SpawnEnemiesOneByOne()
    {
        while (spawnQueue.Count > 0)
        {
            int index = Random.Range(0, spawnQueue.Count);
            GameObject enemyToSpawn = spawnQueue[index];
            spawnQueue.RemoveAt(index);

            Vector3 spawnPos = GetRandomPosition3D();
            Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomPosition3D()
    {
        Vector2 circle = Random.insideUnitCircle * spawnRadius;
        float height = Random.Range(0f, verticalRange); // Solo valores positivos
        Vector3 offset = new Vector3(circle.x, height, circle.y);
        return (centerPoint != null) ? centerPoint.position + offset : transform.position + offset;
    }
}
