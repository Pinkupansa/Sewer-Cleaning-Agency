using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    private void Start()
    {
        GameEvents.current.onPlayerInstantiated.AddListener(SpawnLoot);
    }
    private void SpawnLoot()
    {
        GameObject lootToSpawn = ItemDatabase.instance.RandomLoot();
        Instantiate(lootToSpawn, transform.position, Quaternion.identity);
        Debug.Log("Spawned Object");
        Destroy(gameObject);
    }
}
