using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public Transform[] spawnPoints;
	public GameObject[] enemyPrefab;

	void Start()
	{
		SpawnNewEnemy();
	}

    private void OnEnable()
    {
    }

    void SpawnNewEnemy()
	{

		int randomSpawnP = Mathf.RoundToInt(Random.Range(0f, spawnPoints.Length - 1));
		int randomenemyprefab = Mathf.RoundToInt(Random.Range(0f, enemyPrefab.Length - 1));

		Instantiate(enemyPrefab[randomenemyprefab], spawnPoints[randomSpawnP].transform.position, Quaternion.identity);
	}
}
