using UnityEngine;

public class Board : MonoBehaviour
{
    public int largeur = 20;
    public int hauteur = 10;
    public int qMax = 4;
    public int nbFourmi = 10;
    public int nbMur = 10;
    public int nbGraines = 10;

    public GameObject murPrefab;
    public GameObject fourmiPrefab;
    public GameObject grainPrefab;
    public GameObject AntHill;

    public Transform AntHolder;
    public Transform WallHolder;
    public Transform GraineHolder;

    private AntConlonySimulation antSimulation;

    void Start()
    {
        // Créer une instance de la fourmilière
        antSimulation = new AntConlonySimulation(largeur, hauteur, qMax, nbMur, nbFourmi, nbGraines);

        int x, y;
        (x, y) = antSimulation.getAntColonyCoordinate();
        // Instancier le ANTHILL
        GameObject anthill = Instantiate(AntHill, new Vector3(x, 0, y), Quaternion.identity);


        // Générer les murs
        GenerateWalls();

        // Générer les fourmis
        GenerateAnts();

        // Générer les grains
        GenerateGrains();
    }

    void Update()
    {
        // Faire évoluer la fourmilière
        antSimulation.evolveTheAntColony();

        Debug.Log("nb fourmis =" + antSimulation.getAntsInColony().Count);
        Debug.Log("nb graines =" + antSimulation.GetTotalSeedOutColony());
        Debug.Log("nb graines Hill =" + antSimulation.GetTotalSeedInColony());

        // Recréer les fourmis et les grains
        GenerateAnts();
        GenerateGrains();
    }

    void GenerateWalls()
    {
        // Supprimer les murs existants
        foreach (Transform child in WallHolder)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < largeur + 2; x++)
        {
            for (int y = 0; y < hauteur + 2; y++)
            {
                if (antSimulation.IsAWallInCoordinate(x,y))
                {
                    GameObject wall = Instantiate(murPrefab, new Vector3(x, 0, y), Quaternion.identity);
                    wall.transform.SetParent(WallHolder);
                }
            }
        }
    }

    void GenerateAnts()
    {
        // Supprimer les fourmis existants
        foreach (Transform child in AntHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (Ant f in antSimulation.getAntsInColony())
        {
            GameObject ant = Instantiate(fourmiPrefab, new Vector3(f.GetX(), 0.5f, f.GetY()), Quaternion.identity);
            ant.transform.SetParent(AntHolder);
            if (f.IsCarryingSeed())
            {
                Renderer renderer = ant.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.blue;
                }
            }
        }
    }

    void GenerateGrains()
    {
        // Supprimer les Grains existants
        foreach (Transform child in GraineHolder)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < largeur + 2; x++)
        {
            for (int y = 0; y < hauteur + 2; y++)
            {
                int qte = antSimulation.getSeedQuantityOnCoordinate(x, y);

                if (qte > 0)
                {
                    GameObject graines = Instantiate(grainPrefab, new Vector3(x + 0.5f, 0.1f, y + 0.5f), Quaternion.identity);
                    graines.transform.SetParent(GraineHolder);

                    // Modifier la couleur de l'objet graine en fonction de la quantité de graines
                    Renderer renderer = graines.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        if (qte == 1)
                        {
                            // Utiliser une couleur unie si la quantité de graines est égale à 1
                            renderer.material.color = Color.yellow;
                        }
                        else
                        {
                            // Utiliser un dégradé de couleur jaune à vert en fonction de la quantité de graines
                            Color originalColor = Color.yellow; // Jaune
                            Color targetColor = Color.green; // Vert
                            float intensity = (float)(qte - 1) / (qMax - 1); // Normaliser la quantité de graines
                            renderer.material.color = Color.Lerp(originalColor, targetColor, intensity); // Interpoler entre la couleur jaune et la couleur verte
                        }
                    }
                }
            }
        }
    }


}
