﻿using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool developerMode;

    public Wave[] waves;
    public EnemyControllerNew enemy;

    CharacterStats playerEntity;
    Transform playerT;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    MapGenerator map;

    float timeBetweenCampingChecks = 2f;
    float campThresholdDistance = 1.5f;
    float nextCampCheckTime;
    Vector3 campPositionOld;
    bool isCamping;

    bool playerIsDisabled;

    public event System.Action<int> OnNewWave;

    private void Start()
    {
        playerEntity = FindObjectOfType<PlayerController>();
        playerT = playerEntity.transform;

        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campPositionOld = playerT.position;
        playerEntity.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();
        NextWave();
    }

    void Update()
    {
        if (!playerIsDisabled)
        {
            if (Time.time > nextCampCheckTime)
            {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;

                isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);
                campPositionOld = playerT.position;
            }

            if ((enemiesRemainingToSpawn > 0 || currentWave.infiniteWave) && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine("SpawnEnemy");
            }
        }

        if (developerMode)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StopCoroutine("SpawnEnemy");
                
                foreach(EnemyControllerNew enemy in FindObjectsOfType<EnemyControllerNew>())
                {
                    GameObject.Destroy(enemy.gameObject);
                }

                NextWave();
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;

        Transform spawningTile = map.GetRandomOpenTile();

        if (isCamping)
        {
            spawningTile = map.GetTileFromPosition(playerT.position);
        }

        Material tileMat = spawningTile.GetComponent<Renderer>().material;
        Color initialColor = tileMat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;

        while(spawnTimer < spawnDelay)
        {
            tileMat.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        EnemyControllerNew spawnedEnemy = Instantiate(enemy, spawningTile.position + Vector3.up, Quaternion.identity) as EnemyControllerNew;
        spawnedEnemy.OnDeath += OnEnemyDeath;
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth);

        if (tileMat.color != initialColor)
        {
            tileMat.color = initialColor;
        }
    }

    void OnPlayerDeath()
    {
        playerIsDisabled = true;
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        if(enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void ResetPlayerPosition()
    {
        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3; //the +Vector3.up part is made so, that the player would fall a bit above the ground to the map.
    }

    void NextWave()
    {
        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);
            }

            ResetPlayerPosition();
        }
    }

    [System.Serializable]
    public class Wave
    {
        public bool infiniteWave;
        public int enemyCount;
        public float timeBetweenSpawns;

        public float moveSpeed;
        public int hitsToKillPlayer;
        public float enemyHealth;
    }
}
