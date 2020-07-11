using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject currentTile, previousTile;
    public Vector3 spawnCoords;
    public float spawnInterval, currentInterval;

    // Start is called before the first frame update
    void Start()
    {
        currentInterval = spawnInterval;
        SpawnTile(0);
    }

    // Update is called once per frame
    void Update()
    {
        currentInterval -= Time.deltaTime;

        if (currentInterval <= 0)
        {
            int index = Random.Range(0, prefabs.Length);
            SpawnTile(index);
        }
    }

    void SpawnTile(int index)
    {
        GameObject tile = Instantiate(prefabs[index], spawnCoords, Quaternion.identity);
        previousTile = currentTile;
        currentTile = tile;
        currentInterval = spawnInterval;
    }
}
