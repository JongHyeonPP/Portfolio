using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public List<Transform> spawnPos;
    [SerializeField]private List<GameObject> enemyPool;
    private int spawnIdx = 0;
    private int spawnMax = 2;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField]  Vector3 playerPosition;
    void Awake()
    {
        enemyPool = new List<GameObject>();
        GameObject temp = GameObject.Find("SpawnPos");
        temp.GetComponentsInChildren<Transform>(spawnPos);
        spawnPos.RemoveAt(0);
        enemyPrefab = Resources.Load<GameObject>("Enemy");
        GameObject objectPool = new GameObject("ObjectPool_Enemy");
        for (int i = 0; i < spawnMax; i++)
        {
            enemyPool.Add(Instantiate(enemyPrefab,objectPool.transform));
            enemyPool[i].SetActive(false);
        }
        playerPosition = GameObject.FindWithTag("Player").transform.position;
    }
    private void Start()
    {
        StartCoroutine(CheckSpawn());
    }
    private IEnumerator CheckSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < enemyPool.Count; i++)
            {
                if (enemyPool[i].activeSelf)
                {
                    continue;
                }
                if (Vector3.Distance(spawnPos[spawnIdx].position, playerPosition) <1f)
                {
                    continue;
                }
                enemyPool[i].SetActive(true);
                
                enemyPool[i].transform.position = spawnPos[spawnIdx++].position;
                spawnIdx %= spawnMax;
                break;
            }
           
        }
    }
}