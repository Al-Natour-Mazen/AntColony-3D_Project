using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
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

    private int width, height;

    public void SetWidthBoard(int width)
    {
        this.width = width;
    }

    public void SetHeightBoard(int height)
    {
        this.height = height;
    }

    public void InstantiateAnthill(AntConlonySimulation antSimulation)
    {
        ClearGameObjects(colonyHolder);
        int x, y;
        (x, y) = antSimulation.GetAntColonyCoordinate();
        InstantiateGameObject(antHill, x, 1.5F, y, colonyHolder);
    }

    public void GenerateWalls(AntConlonySimulation antSimulation)
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

    public void GenerateAnts(AntConlonySimulation antSimulation)
    {
        ClearGameObjects(antHolder);

        foreach (Ant ant in antSimulation.GetAntsInColony())
        {
            GameObject newAnt = InstantiateGameObject(antPrefab, ant.GetX(), 0.5f, ant.GetY(), antHolder);
            Renderer renderer = newAnt.GetComponent<Renderer>();

            if (renderer != null && ant.IsCarryingSeed())
            {
                renderer.material.color = new Color32(0x00, 0x3A, 0xFD, 0xFF); // Correspond à la couleur hexadécimale #003AFD
            }
        }
    }

    public void GenerateSeeds(AntConlonySimulation antSimulation)
    {
        ClearGameObjects(seedHolder);

        for (int x = 0; x < width + 2; x++)
        {
            for (int y = 0; y < height + 2; y++)
            {
                int seedQuantity = antSimulation.GetSeedQuantityAt(x, y);

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
                            float intensity = (float)(seedQuantity - 1) / (antSimulation.GetMaxSeedQuantityOnBlock() - 1);
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