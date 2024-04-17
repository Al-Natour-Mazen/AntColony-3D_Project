using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Board Settings")]
    public int width = 20;
    public int height = 10;

    [Header("Entity Counts")]
    public int antCount = 10;
    public int wallCountDensity = 10;
    public int seedCountDensity = 10;

    [Header("Entity Prefabs")]
    public GameObject wallPrefab;
    public GameObject antPrefab;
    public GameObject seedPrefab;
    public GameObject antHill;

    [Header("Transform Holders")]
    public Transform antHolder;
    public Transform wallHolder;
    public Transform seedHolder;
    public Transform colonyHolder;

    [Header("Seed Settings")]
    public int maxSeedQuantity = 4;

    private AntConlonySimulation antSimulation;

    void Start()
    {
        antSimulation = new AntConlonySimulation(width, height, maxSeedQuantity, wallCountDensity, antCount, seedCountDensity);

        InstantiateAnthill();
        GenerateWalls();
        GenerateAnts();
        GenerateSeeds();
    }

    void Update()
    {
        antSimulation.evolveTheAntColony();

        Debug.Log("nb fourmis =" + antSimulation.getAntsInColony().Count);
        Debug.Log("nb graines =" + antSimulation.GetTotalSeedOutColony());
        Debug.Log("nb graines Hill =" + antSimulation.GetTotalSeedInColony());


        GenerateAnts();
        GenerateSeeds();
    }

    void InstantiateAnthill()
    {
        int x, y;
        (x, y) = antSimulation.getAntColonyCoordinate();
        InstantiateGameObject(antHill, x, 0, y, colonyHolder);
    }

    void GenerateWalls()
    {
        ClearGameObjects(wallHolder);

        for (int x = 0; x < width + 2; x++)
        {
            for (int y = 0; y < height + 2; y++)
            {
                if (antSimulation.IsAWallAt(x, y))
                {
                    InstantiateGameObject(wallPrefab, x, 0, y, wallHolder);
                }
            }
        }
    }

    void GenerateAnts()
    {
        ClearGameObjects(antHolder);

        foreach (Ant ant in antSimulation.getAntsInColony())
        {
            GameObject newAnt = InstantiateGameObject(antPrefab, ant.GetX(), 0.5f, ant.GetY(), antHolder);
            Renderer renderer = newAnt.GetComponent<Renderer>();

            if (renderer != null && ant.IsCarryingSeed())
            {
                renderer.material.color = Color.blue;
            }
        }
    }

    void GenerateSeeds()
    {
        ClearGameObjects(seedHolder);

        for (int x = 0; x < width + 2; x++)
        {
            for (int y = 0; y < height + 2; y++)
            {
                int seedQuantity = antSimulation.getSeedQuantityAt(x, y);

                if (seedQuantity > 0)
                {
                    GameObject newSeed = InstantiateGameObject(seedPrefab, x + 0.5f, 0.1f, y + 0.5f, seedHolder);
                    Renderer renderer = newSeed.GetComponent<Renderer>();

                    if (renderer != null)
                    {
                        if (seedQuantity == 1)
                        {
                            renderer.material.color = Color.yellow;
                        }
                        else
                        {
                            Color originalColor = Color.yellow;
                            Color targetColor = Color.green;
                            float intensity = (float)(seedQuantity - 1) / (maxSeedQuantity - 1);
                            renderer.material.color = Color.Lerp(originalColor, targetColor, intensity);
                        }
                    }
                }
            }
        }
    }

    void ClearGameObjects(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    GameObject InstantiateGameObject(GameObject prefab, float x, float y, float z, Transform parent)
    {
        GameObject obj = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
        obj.transform.SetParent(parent);
        return obj;
    }
}