using UnityEngine;

public class LevelMgr : MonoBehaviour
{
    public GameObject[] collectiblePrefabs;
    public GameObject[] jumperPrefabs;
    public GameObject[] enemyPrefabs;

    public float levelLength = 2500f;
    public Vector3 playerStartPosition = new Vector3(0f, 0.5f, -5f);
    public GameObject playerObject;
    public GameObject spawnLeafObject;

    void Start()
    {
        RestartLevel();
    }

    private void InstantiateLevel()
    {
        EmptySpawnLeafObject();

        for (float z = 0; z < levelLength; z += 40f)
        {
            GameObject prefabToInstantiate = collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)];
            if (prefabToInstantiate == null)
            {
                continue;
            }

            var collectibleData = prefabToInstantiate.GetComponent<CollectibleData>();
            float x = 0f;
            float y = 0f;
            if (collectibleData != null)
            {
                x = collectibleData.StartXPosition;
                y = collectibleData.nominalHeight;
            }
            GameObject go = Instantiate(prefabToInstantiate, new Vector3(x, y, z), Quaternion.identity);
            go.transform.parent = spawnLeafObject.transform;
        }
        
        for (float z = 10f; z < levelLength; z += 120f)
        {
            GameObject prefabToInstantiate = jumperPrefabs[Random.Range(0, jumperPrefabs.Length)];
            if (prefabToInstantiate == null)
            {
                continue;
            }

            var jumperData = prefabToInstantiate.GetComponent<JumperData>();
            float x = 0f;
            float y = 0f;
            if (jumperData != null)
            {
                x = jumperData.StartXPosition;
                y = jumperData.nominalHeight;
            }
            GameObject go = Instantiate(prefabToInstantiate, new Vector3(x, y, z), prefabToInstantiate.transform.rotation);
            go.transform.parent = spawnLeafObject.transform;
        }
        
        for (float z = 25f; z < levelLength; z += 80f)
        {
            GameObject prefabToInstantiate = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            if (prefabToInstantiate == null)
            {
                continue;
            }

            float x = 0f;
            float y = 1f;
            var enemyData = prefabToInstantiate.GetComponent<AttractorData>();
            if (enemyData != null)
            {
                x = enemyData.StartXPosition;
                y = enemyData.nominalHeight;
            }
            GameObject go = Instantiate(prefabToInstantiate, new Vector3(x, y, z), prefabToInstantiate.transform.rotation);
            var attractor = go.gameObject.GetComponent<EnemyAttractor>();
            if (attractor != null)
            {
                go.gameObject.GetComponent<EnemyAttractor>().attractor = playerObject;
            }
            go.transform.parent = spawnLeafObject.transform;
        }
    }

    private void EmptySpawnLeafObject()
    {
        if (spawnLeafObject != null)
        {
            Destroy(spawnLeafObject);
        }

        spawnLeafObject = new GameObject("spawnLeaf");
    }

    public void RestartLevel()
    {
        playerObject.transform.position = playerStartPosition;
        playerObject.GetComponent<PlayerCtrl>().Reset();
        InstantiateLevel();
    }
}
