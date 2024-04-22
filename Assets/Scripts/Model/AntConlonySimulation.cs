using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;


public class AntConlonySimulation
{
    private GridColony gridColony;
    private AntColony colony;
    private static int GapAroundHill = 8;
    private int nbWallsSimulation, nbAntsSimulation, nbSeedsSimulation;

    public AntConlonySimulation(int width, int height, int quantityMaxSeedPerBlock, int nbMurs, int nbFourmis, int nbGraines)
    {
        System.Random rand = new System.Random();

        this.nbSeedsSimulation = nbGraines;
        this.nbAntsSimulation = nbFourmis;
        this.nbWallsSimulation = nbMurs;

        gridColony = new GridColony(width, height, quantityMaxSeedPerBlock);

        int x, y;
        (x, y) = InitPositionColony(rand);
        colony = new AntColony(x, y, gridColony);

        InitRandomEnvirnomentSimulation(rand);
    }

    private (int,int) InitPositionColony(System.Random rand)
    {
        int XCoordonne, YCoordonne;
        XCoordonne = rand.Next(GapAroundHill, gridColony.GetWidth() - GapAroundHill);
        YCoordonne = rand.Next(GapAroundHill, gridColony.GetHeight() - GapAroundHill);
        return (XCoordonne,YCoordonne);
    }

    public (int,int) GetAntColonyCoordinate()
    {
        return (colony.GetXColonyCoordinate(), colony.GetYColonyCoordinate());
    }

    public HashSet<Ant> GetAntsInColony()
    {
        return colony.GetTheAnts();
    }

    public int GetSeedQuantityAt(int x , int y)
    {
        return gridColony.GetSeedQuantity(x, y);
    }

    public void EvolveTheAntColony()
    {
        colony.Progress();
    }

    public bool IsAWallAt(int x , int y)
    {
        return gridColony.GetWall(x, y);    
    }

    public int GetTotalSeedOutColony()
    {
        return gridColony.CountTotalSeeds();
    }

    public int GetTotalSeedInColony()
    {
        return colony.GetSeedQuantity();
    }

    public int GetMaxSeedQuantityOnBlock()
    {
        return gridColony.GetMaxQuantityOnBlock();
    }

    private void InitRandomEnvirnomentSimulation(System.Random rand)
    {
        // Placer les murs aléatoirement
        PlaceRandomWalls(rand);

        // Placer les fourmis aléatoirement
        PlaceRandomAnts(rand);

        // Placer les graines aléatoirement
        PlaceRandomSeeds(rand);
    }

   

    private void PlaceRandomWalls(System.Random rand)
    {
        for (int i = 0; i < nbWallsSimulation; i++)
        {
            int x, y;
            do
            {
                x = rand.Next(gridColony.GetWidth());
                y = rand.Next(gridColony.GetHeight());
            } while (DistanceEuclidienne(x, y, colony.GetXColonyCoordinate(), colony.GetYColonyCoordinate()) < GapAroundHill);

            gridColony.SetWall(x, y, true);
        }
    }

    private void PlaceRandomAnts(System.Random rand)
    {
        int nbFourmisPlaces = 0;
        while (nbFourmisPlaces < nbAntsSimulation)
        {
            int x = rand.Next(gridColony.GetWidth());
            int y = rand.Next(gridColony.GetHeight());
            if ((!gridColony.GetWall(x, y) || !gridColony.ContainsAnt(x, y)))
            { 
                colony.AddNewAnt(x, y);
                nbFourmisPlaces++;
            }
        }
    }

    private void PlaceRandomSeeds(System.Random rand)
    {
        for (int i = 0; i < nbSeedsSimulation; i++)
        {
            int x, y;
            do
            {
                 x = rand.Next(gridColony.GetWidth());
                 y = rand.Next(gridColony.GetHeight());
            } while (DistanceEuclidienne(x, y, colony.GetXColonyCoordinate(), colony.GetYColonyCoordinate()) < GapAroundHill);

            if (!gridColony.GetWall(x, y))
            {
                int qte = rand.Next(gridColony.GetMaxQuantityOnBlock()  + 1);
                gridColony.SetSeedQuantity(x, y, qte);
            }
        }
    }

    private double DistanceEuclidienne(int x1, int y1, int x2, int y2)
    {
        return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
    }
}

